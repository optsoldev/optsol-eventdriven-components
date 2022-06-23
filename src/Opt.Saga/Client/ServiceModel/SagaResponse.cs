using Funq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Optsol.EventDriven.Components.Core.Domain;
using Optsol.EventDriven.Components.Core.Domain.Entities;
using ServiceStack;
using ServiceStack.Api.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Opt.Saga.Core.Controllers;
using Opt.Saga.Core.Client.ServiceModel;
using Opt.Saga.Core;
using Opt.Saga.Core.Validator;

namespace Opt.Saga.Core.Client.ServiceModel
{
    public class SagaResponse : ApiResponse
    {
        //public string TransactionId { get; set; }
        //public HttpStatusCode StatusCode { get; set; }

        //public static SagaResponse Ok => new SagaResponse { StatusCode = HttpStatusCode.OK };

        //public string Output { get; set; }
        public string ExecutionTime { get; set; }
        public DateTime RequestTimespan { get; internal set; }
    }
    public class ApiResponse
    {
        public string TransactionId { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
        public IEnumerable<string> Messages { get; set; }

    };
}

[Route("/saga-client")]
public class SagaRequest
{
    public object Body { get; set; }
    public string FlowKey { get; set; }

    public override string ToString() => SagaJsonCoverter.SerializeObject(Body);
    public SagaRequest()
    {

    }

}
public partial class SagaClientService : Service
{
    public ILogger<SagaClientHost> Logger { get; }
    public JsonSchemaValidator Validator { get; }
    public IDomainEventConverter DomainEventConverter { get; }
    private readonly HostsOptions HostsOptions;

    public SagaClientService(ILogger<SagaClientHost> logger, JsonSchemaValidator validator, Microsoft.Extensions.Options.IOptions<HostsOptions> hostsOptions)
    {
        Logger = logger;
        Validator = validator;
        HostsOptions = hostsOptions.Value;
    }
    [AddHeader(ContentType = "application/json")]
    public SagaResponse Post(SagaRequest request)
    {
        var flow = SagaFlow.CreateFromConfigurationFile();

        var context = new SagaContext(request, Guid.NewGuid().ToString(), Logger, Validator, flow.Context.DefaultTimeOut, null, HostsOptions);

        flow.UpdateContext(context);

        new TaskFactory().StartNew(async () => flow.Start().Wait(), TaskCreationOptions.AttachedToParent);

        return new SagaResponse { StatusCode = HttpStatusCode.OK, TransactionId = context.TransactionId };
    }
    public SagaResponse Get(SagaRequest request)
    {
        return new SagaResponse();
    }
}
public class SagaClientHost : AppHostBase

{
    public SagaClientHost() : base("SagaClientHost", typeof(SagaClientService).Assembly) { }
    public SagaClientHost(params Assembly[] assemblies) : base("SagaClientHost", assemblies) { }

    public override void Configure(Container container)
    {

        Plugins.Add(new SwaggerFeature());
    }
}
public static class MvcExtensions
{
    public static JsonDocument JsonDocumentFromObject(object value, JsonSerializerOptions options = null)
    {
        if (value is string valueStr)
        {
            try { return JsonDocument.Parse(valueStr); }
            catch { }
        }

        byte[] bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value, options);
        return JsonDocument.Parse(bytes);
    }

    public static JsonElement JsonElementFromObject(object value, JsonSerializerOptions options = null)
    {
        JsonElement result;
        using (JsonDocument doc = JsonDocumentFromObject(value, options))
        {
            result = doc.RootElement.Clone();
        }
        return result;
    }
    /// <summary>
    /// Finds the appropriate controllers
    /// </summary>
    /// <param name="partManager">The manager for the parts</param>
    /// <param name="controllerTypes">The controller types that are allowed. </param>
    public static void UseSpecificControllers(this ApplicationPartManager partManager, params Type[] controllerTypes)
    {
        partManager.FeatureProviders.Add(new InternalControllerFeatureProvider());
        partManager.ApplicationParts.Clear();
        partManager.ApplicationParts.Add(new SelectedControllersApplicationParts(controllerTypes));
    }

    public static void UseMicroverseApi(this IMvcCoreBuilder mvcCoreBuilder)
    {
        mvcCoreBuilder.UseSpecificControllers(typeof(SagaClientController));
    }

    /// <summary>
    /// Only allow selected controllers
    /// </summary>
    /// <param name="mvcCoreBuilder">The builder that configures mvc core</param>
    /// <param name="controllerTypes">The controller types that are allowed. </param>
    public static IMvcCoreBuilder UseSpecificControllers(this IMvcCoreBuilder mvcCoreBuilder, params Type[] controllerTypes) => mvcCoreBuilder.ConfigureApplicationPartManager(partManager => partManager.UseSpecificControllers(controllerTypes));

    /// <summary>
    /// Only instantiates selected controllers, not all of them. Prevents application scanning for controllers. 
    /// </summary>
    private class SelectedControllersApplicationParts : ApplicationPart, IApplicationPartTypeProvider
    {
        public SelectedControllersApplicationParts()
        {
            Name = "Only allow selected controllers";
        }

        public SelectedControllersApplicationParts(Type[] types)
        {
            Types = types.Select(x => x.GetTypeInfo()).ToArray();
        }

        public override string Name { get; }

        public IEnumerable<TypeInfo> Types { get; }
    }

    /// <summary>
    /// Ensure that internal controllers are also allowed. The default ControllerFeatureProvider hides internal controllers, but this one allows it. 
    /// </summary>
    private class InternalControllerFeatureProvider : ControllerFeatureProvider
    {
        private const string ControllerTypeNameSuffix = "Controller";

        /// <summary>
        /// Determines if a given <paramref name="typeInfo"/> is a controller. The default ControllerFeatureProvider hides internal controllers, but this one allows it. 
        /// </summary>
        /// <param name="typeInfo">The <see cref="TypeInfo"/> candidate.</param>
        /// <returns><code>true</code> if the type is a controller; otherwise <code>false</code>.</returns>
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(Microsoft.AspNetCore.Mvc.NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase) &&
                       !typeInfo.IsDefined(typeof(Microsoft.AspNetCore.Mvc.ControllerAttribute)))
            {
                return false;
            }

            return true;
        }
    }
}
public static class SagaHostingExtensions
{
    public static IApplicationBuilder UseOptSagaClient(this IApplicationBuilder app)
    {
        // app.UseServiceStack(new SagaClientHost());
        return app;
    }
    public static IApplicationBuilder UseOptSagaClient(this IApplicationBuilder app, params Assembly[] assemblies)
    {
        app.UseServiceStack(new SagaClientHost(assemblies));
        return app;
    }
    public static IServiceCollection AddSagaRequest<TRequest>(this IServiceCollection services) where TRequest : SagaRequest
    {
        return services.AddScoped<SagaRequest, TRequest>();
    }
    public static IServiceCollection AddEvents(this IServiceCollection services, params Type[] events)
    {
        var register = new DomainEventRegister();
        foreach (var evt in events)
        {
            register.Register(evt);
        }
        services.AddSignalR();
        services.AddSingleton<IDomainEventRegister>(register);

        services.AddSingleton<IDomainEventConverter, DomainEventConverter>();
        return services;
    }
    public static IServiceCollection AddOutputConverter<TOutputConverter>(this IServiceCollection services, TOutputConverter outputConverter) where TOutputConverter : class, IOutputConverter
        => services.AddScoped<IOutputConverter, TOutputConverter>();
}


