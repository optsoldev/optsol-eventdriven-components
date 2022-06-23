using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class SagaStep
    {
        public bool Async { get; set; }

        public List<QueryStringParam>? QueryStringInputs { get; set; }

        public string StepName { get; set; }
        public SagaStepEndpoint Endpoint { get; set; }
        public bool AsyncEvents { get; set; }
        public string RequestPropertyName { get; set; }
        public IEnumerable<InputMapping>? InputMappings { get; set; }
        public bool PrettyQueryString { get; set; }
        private string MappingRequestInputsToQueryString(SagaContext context)
        {
            if (QueryStringInputs is not null && QueryStringInputs.Any())
            {

                var sagaRequest = JObject.Parse(context.ClientRequest.ToString());
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
                var qs = new StringBuilder();
                foreach (var map in this.QueryStringInputs)
                {
                    if (string.IsNullOrEmpty(map.DefaultValue))
                    {
                        string _value = sagaRequest[map.Path].ToString();
                        if (PrettyQueryString)
                        {
                            qs.Append(string.Concat("/", _value));
                        }
                        else queryString.Add(map.Param, _value);
                    }
                    else
                    {
                        if (PrettyQueryString)
                            qs.Append(string.Concat("/", map.DefaultValue));
                        else
                            queryString.Add(map.Param, map.DefaultValue);
                    }


                }
                if (PrettyQueryString)
                    return qs.ToString();
                else
                    return "?" + queryString.ToString();
            }
            return null;

        }
        private void SetTransactionId()
        {

        }
        private string PrepareRequestBody(SagaContext context)
        {

            var body = GetRequestBody(context);

            if (body is null)
                return string.Empty;

            SetInputData(body, context);
            return body.ToString();
        }
        private void SetInputData(JObject requestBody, SagaContext context)
        {
            //requestBody["Transaction"] = context.TransactionId;

            if ((InputMappings?.Any()) == true)
            {
                foreach (var mapping in InputMappings)
                {
                    UpdateProperties(mapping, context, requestBody);
                }
            }
        }
        private JObject GetMessageContent(string stepName, SagaContext context)
        {
            var input = context.Outputs[stepName];
            if (input is not null)
            {
                dynamic eventData = JObject.Parse(input);
                var content = eventData.message.content;//.Content;
                return JObject.Parse(content.ToString());
            }
            return null;

        }
        private void UpdateProperties(InputMapping mapping, SagaContext context, JObject requestData)
        {
            context.LogInformation($"process with transactionId {context.TransactionId} - Updating {StepName} request data with {mapping.StepName} step result data");
            var inputObj = GetMessageContent(mapping.StepName, context);
            foreach (var prop in mapping.PropertyChanges)
            {
                context.LogInformation($"process with transactionId {context.TransactionId} - Updating {StepName}.{prop.To} data with {mapping.StepName}.{prop.From} step result with value {inputObj[prop.From]}");

                requestData[prop.To] = inputObj[prop.From];

                context.LogInformation($"process with transactionId {context.TransactionId} - Updated {StepName}.{prop.To} data with {mapping.StepName}.{prop.From} step result with value {inputObj[prop.From]}");


            }

        }

        public virtual async Task<SagaProcessEventArgs> Execute(SagaContext context, CancellationToken cancellationToken)
        {

            var stepRequestBody = PrepareRequestBody(context);

            var response = await MakeRequest(context, cancellationToken, stepRequestBody, context.HostsOptions, MappingRequestInputsToQueryString(context));
            var responseData = (await response.Content.ReadAsStringAsync());

            if (response.IsSuccessStatusCode)
            {
                if (this.AsyncEvents)
                {
                    var eventHandler = new SagaEventHandler();

                    context.LogInformation($"process with transactionId {context.TransactionId} - wainting for event");


                    return await eventHandler.Execute(context, cancellationToken);
                }
                else
                {
                    return new SagaProcessEventArgs(response.IsSuccessStatusCode ? SagaEventResultType.Success : SagaEventResultType.Success, responseData);
                }
            }
            else
            {
                return new SagaProcessEventArgs(response.IsSuccessStatusCode ? SagaEventResultType.Success : SagaEventResultType.Failure, responseData);
            }
        }

        private async Task<HttpResponseMessage> MakeRequest(SagaContext context, CancellationToken cancellationToken, string content, HostsOptions hostsOptions, string queryStringParams = null)
        {

            content = Encoding.UTF8.GetString(UTF8Encoding.UTF8.GetBytes(content));
            if (!cancellationToken.IsCancellationRequested)
            {


                context.SagaHttpClient.AddDefaultRequestHeaders("Transaction", context.TransactionId);
                if (Endpoint.Headers is not null && Endpoint.Headers.Any())
                    foreach (var header in Endpoint.Headers)
                    {
                        context.SagaHttpClient.AddDefaultRequestHeaders(header.Key, header.Value);

                    }

                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    Method = new HttpMethod(Endpoint.Method),
                };

                var url = Endpoint.Url;
                //Regex regex = new Regex(@"\(([^()]+)\)*");
                //foreach (Match match in regex.Matches(Endpoint.Url))
                //{
                //    var key = match.Value.Replace("(", "").Replace(")", "");

                //    if (hostsOptions.ContainsKey(key))
                //    {
                //        url = url.Replace($"({key})", hostsOptions[key]);
                //    }
                //    else
                //    {
                //        throw new ArgumentNullException($"Url host parameter {match.Value} not found.");
                //    }
                //}

                requestMessage.RequestUri = queryStringParams is null ? new Uri(url) : new Uri($"{url}{queryStringParams}");


                if (requestMessage.Method != HttpMethod.Get && requestMessage.Method != HttpMethod.Delete)
                    requestMessage.Content = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);

                try
                {
                    context.LogInformation($"making {requestMessage.Method} request to endpoint {requestMessage.RequestUri}");

                    var response = await context.SagaHttpClient.SendAsync(requestMessage);

                    context.LogInformation($"request to endpoint {requestMessage.RequestUri} was completed");

                    if (response.IsSuccessStatusCode)
                    {
                        context.LogInformation($"request was OK");

                    }
                    else
                    {
                        context.LogInformation($"request was NOTOK");
                    }
                    context.LogInformation($"response");
                    context.LogInformation($"{ (await response.Content.ReadAsStringAsync()) }");
                    return response;
                }
                catch (Exception e)
                {

                    throw e;
                }

            }
            return null;
        }
        private JObject GetRequestBody(SagaContext context)
        {
            if (string.IsNullOrEmpty(RequestPropertyName))
                return null;

            var body = JObject.Parse(context.ClientRequest.ToString());
            body = JObject.FromObject(body.ToObject<ExpandoObject>(), Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

            var processRequestBody = body[RequestPropertyName];
            if (processRequestBody is not null)
                return JObject.Parse(processRequestBody.ToString());
            throw new ArgumentException($"process with transactionId {context.TransactionId} - Node {RequestPropertyName} not found in request body");
        }
    }

}
