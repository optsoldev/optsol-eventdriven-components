using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Caching.Abstractions;
using Microsoft.Extensions.Caching.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Opt.Saga.Core.Abstractions;
using Opt.Saga.Core.Client.ServiceModel;
using Opt.Saga.Core.Messaging;
using Opt.Saga.Core.Respositories;
using Opt.Saga.Core.Validator;
using Optsol.EventDriven.Components.Core.Domain;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using System.Text.Json;

namespace Opt.Saga.Core
{
    public delegate void OperationHandler(object sender, SagaProcessEventArgs message);

    public class SagaFlow
    {
        public string FlowKey { get; set; }
        public string ValidateRequestType { get; set; }
        public SagaContext Context { get; set; }
        public AggregateRouteKeys RouteKeys { get; set; }

        public SagaStep Passos { get; set; }
        public List<SagaStep> Steps { get; set; }

        public bool ConcatOutputs { get; set; }

        private Dictionary<string, Action> WhenCompletedActions;
        private bool _hasErrors;

        public SagaFlow()
        {
            //this.Steps = new Queue<SagaStep>();
            this.WhenCompletedActions = new Dictionary<string, Action>();
            //JsonConstructor 
        }
        public SagaFlow(SagaContext context)
        {
            this.Context = context;
            //  this.Steps = new Queue<SagaStep>();
            this.WhenCompletedActions = new Dictionary<string, Action>();
        }

        public static SagaFlow Create(SagaContext context)
        {
            SagaFlow sagaFlow = new SagaFlow(context);
            return sagaFlow;
        }
        public static SagaFlow CreateFromConfigurationFile()
        {
            var json = File.ReadAllText(Path.Combine("configuration", "flow.json"));
            var flow = SagaJsonCoverter.DeserializeObject<SagaFlow>(json);
            return flow;
        }
        public SagaFlow UpdateContext(SagaContext context)
        {
            this.Context = context;
            return this;
        }
        public SagaFlow AddOutputConverter<TOutputConverter>() where TOutputConverter : IOutputConverter, new()
        {
            this.OutputConverter = new TOutputConverter();
            return this;
        }
        public SagaFlow AddDefaultOutputConverter()
        {
            this.AddOutputConverter<DefaultOutputConverter>();
            return this;
        }
        public JObject FormatOuput()
        {
            if (this.OutputConverter is not null)
            {
                if (this.RouteKeys is null)
                {
                    Context.LogInformation($"OuputConverter of type {OutputConverter.GetType().Name } was configured but it will not be executed because a value for RouteKeys was not found");
                    Context.LogInformation($"OuputConverter of type {OutputConverter.GetType().Name } was configured but it will not be executed because a value for RouteKeys was not found");
                }
                return OutputConverter.Convert(this.RouteKeys, this.Context.Outputs);
            }
            JObject obj = new JObject();
            foreach (var output in Context.Outputs)
            {
                obj[output.Key] = output.Value;
            }
            return obj;
        }
        public Task ExecuteAsyncSteps()
        {
            var asyncSteps = Steps.Where(x => x.Async).OptForEachAsync(20, async asyncStep =>
            {
                Context.LogInformation($"executing step {asyncStep.StepName}");
                var result = await asyncStep.Execute(this.Context, CancellationToken.None);
                Context.LogInformation($"step {asyncStep.StepName} executed with result :  {result.EventType}");
                Context.AddOutput(asyncStep.StepName, result);
                if (result.EventType == SagaEventResultType.Failure)
                {
                    Context.LogInformation($"step {asyncStep.StepName} exewcuted with errors. ");
                    Context.LogInformation($"execution process stopped ");

                    _hasErrors = true;
                }
            });
            return asyncSteps;
        }
        public async Task ExecuteSyncSteps()
        {
            var _Steps = new Queue<SagaStep>(Steps.Where(x => !x.Async));

            while (_Steps.Any())
            {
                var step = _Steps.Dequeue();
                Context.LogInformation($"executing step {step.StepName}");
                Context.CurrentStep = step.StepName;
                var result = await step.Execute(this.Context, CancellationToken.None);
                Context.LogInformation($"step {step.StepName} executed with result :  {result.EventType}");
                Context.AddOutput(step.StepName, result);
                if (result.EventType == SagaEventResultType.Failure)
                {
                    Context.LogInformation($"step {step.StepName} exewcuted with errors. ");
                    Context.LogInformation($"execution process stopped ");

                    _hasErrors = true;
                    break;
                }

            }
        }
        public bool ValidateRequest()
        {
            if (string.IsNullOrEmpty(this.ValidateRequestType))
                return true;

            return Context.ValidateRequest(ValidateRequestType);
        }
        public void ExecuteCompletedActions()
        {
            if (WhenCompletedActions.Any())
            {
                Context.LogInformation($"executing WHenCompletd actions");
                foreach (var action in WhenCompletedActions)
                {
                    Context.LogInformation($"executing action {action.Key}");
                    action.Value();
                }
            }
        }
        public async Task Start()
        {
            Context.StartChronometer();
            CancellationTokenSource cts = new CancellationTokenSource();

            using (Context)
            {
                try
                {
                    if (!ValidateRequest())
                    {
                        throw new InvalidOperationException("the request body is invalid");
                    }

                    // var activity = new SagaActivity(Guid.Parse(Context.TransactionId), "", FlowKey, SagaStatus.Executing, DateTime.Now);
                    // Context.Repository.Add(activity);

                    Context.LogInformation($"Flow {FlowKey} was started");
                    Context.LogInformation($"RequestBody {Environment.NewLine} {Context.ClientRequest}");

                    Context.LogInformation($"Has {this.Steps.Count} steps");

                    var asyncSteps = ExecuteAsyncSteps();

                    var syncSteps = ExecuteSyncSteps();

                    await asyncSteps;
                    await syncSteps;


                    ExecuteCompletedActions();





                    if (!_hasErrors)
                    {
                        PostOnResultSignalR signalR = new PostOnResultSignalR();
                        // await signalR.Execute(this.Context, CancellationToken.None);
                        Status = SagaStatus.Success;
                        Context.LogInformation($"Saga flow execution completed. ExecutionTime : {Context.GetExecutionTime()}");
                    }
                    else
                    {
                        Status = SagaStatus.Failure;

                        Context.LogInformation($"Saga flow execution completed with errors. ExecutionTime : {Context.GetExecutionTime()}");
                    }
                    if (PrintOutputs)
                    {
                        if (ConcatOutputs)
                        {
                            Context.LogInformation($"Printing formatted saga flow output");
                            Context.LogInformation(FormatOuput().ToString());

                        }
                        else
                        {
                            Context.LogInformation($"Prints steps outputs");

                            foreach (var stepOutput in Context.Outputs)
                            {
                                Context.LogInformation($"output of step {stepOutput.Key} : {Environment.NewLine} {stepOutput.Value} ");

                            }
                        }
                    }
                    //activity.SagaStatus = Status;
                    //activity.FinishedAt = DateTime.Now;
                    //  Context.Repository.Update(activity);

                }
                catch (Exception e)
                {
                    if (PrintOutputs)
                    {
                        if (ConcatOutputs)
                        {
                            Context.LogInformation($"Printing formatted saga flow output");
                            Context.LogInformation(FormatOuput().ToString());

                        }
                        else
                        {
                            Context.LogInformation($"Prints steps outputs");

                            foreach (var stepOutput in Context.Outputs)
                            {
                                Context.LogInformation($"output of step {stepOutput.Key} : {Environment.NewLine} {stepOutput.Value} ");

                            }
                        }
                    }
                    Context.AddError(e.Message);
                    Context.Logger.LogCritical(e.Message);
                }
            }

        }
        private void RenderOutputs()
        {
            if (PrintOutputs)
            {
                if (ConcatOutputs)
                {
                    Context.LogInformation($"Printing formatted saga flow output");
                    Context.LogInformation(FormatOuput().ToString());

                }
                else
                {
                    Context.LogInformation($"Prints steps outputs");

                    foreach (var stepOutput in Context.Outputs)
                    {
                        Context.LogInformation($"output of step {stepOutput.Key} : {Environment.NewLine} {stepOutput.Value} ");

                    }
                }
            }
        }

        public JObject GetOutput() => this.FormatOuput();

        public SagaFlow(SagaContext context, IEnumerable<SagaStep> steps, IDictionary<string, Action> whenCompletedActions)
        {
            Context = context;
            //Steps = new Queue<SagaStep>(steps);
            WhenCompletedActions = new Dictionary<string, Action>(whenCompletedActions);
        }
        public bool PrintOutputs { get; set; }
        public IOutputConverter OutputConverter { get; private set; }
        public SagaStatus Status { get; set; }

        internal void EnablePrintOutput() => PrintOutputs = true;
        public SagaFlow PrintOutput()
        {
            this.EnablePrintOutput();
            return this;
        }
        public SagaFlow AddStep(SagaStep step)
        {
            Steps.Add(step);

            return new SagaFlow(this.Context, Steps, this.WhenCompletedActions);
        }
        public SagaFlow WhenCompleted(string name, Action action)
        {
            WhenCompletedActions.Add(name, action);

            return new SagaFlow(this.Context, Steps, this.WhenCompletedActions);
        }
        public SagaFlow AddStep<Step>() where Step : SagaStep, new()
        {
            Steps.Add(new Step());

            return new SagaFlow(this.Context, Steps, WhenCompletedActions);
        }

        internal IEnumerable<string> GetMessages()
        {
            return Context.Errors;
        }
    }
 
}
