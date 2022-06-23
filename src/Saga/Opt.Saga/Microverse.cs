using Newtonsoft.Json.Linq;

namespace Opt.Saga.Core
{
    public class Microverse
    {
        public MicroverseFlows Flows { get; set; }
        bool HasFlows => !(Flows is null && !Flows.Any());
        public List<global::SagaEndpoint> Endpoints { get; set; }
        public SagaFlow this[string flowKey]
        {
            get { return Get(flowKey); }
        }
        public Microverse()
        {
            Flows = new();
        }

        public SagaFlow CreateFromConfigurationFile(string flowKey)
        {
            var json = File.ReadAllText(Path.Combine("configuration", "flow.json"));

            foreach (var item in this.Endpoints)
            {
                json = json.Replace(String.Concat("{{", item.Key, "}}"), item.Value);
            }
            if (json is null) throw new ArgumentException($"No flow found for the key {flowKey}");

            var microverse = SagaJsonCoverter.DeserializeObject<Microverse>(JObject.Parse(json)["Saga"].ToString());
            if (microverse.HasFlows)
            {
                try
                {
                    var flow = microverse.Flows.First(x => x.FlowKey == flowKey);
                    if (flow is null)
                        throw new ArgumentException($"can not found flow to the flow key {flowKey}");
                    return flow;
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentException($"can not found flow to the flow key {flowKey}");

                }
            }
            throw new ArgumentException($"No MicroverseFlows found in this Microverse =/ ");

        }
        public SagaFlow Get(string flowKey)
        {
            

            if (HasFlows)
            {
                try
                {
                    var sagaFlow = this.Flows.First(e => e.FlowKey == flowKey);
                    var json = SagaJsonCoverter.SerializeObject(sagaFlow);
                    foreach (var item in this.Endpoints)
                    {
                        json = json.Replace(item.Key, item.Value);
                    }
                    return SagaJsonCoverter.DeserializeObject<SagaFlow>(json);
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentException($"can not found flow to the flow key {flowKey}");

                }
            }
            throw new ArgumentException($"No MicroverseFlows found in this Microverse =/ ");
        }
    }
    //public class SagaEndpointResolver : ISagaEndpointResolver
    //{
    //    internal readonly List<SagaEndpoint> Endpoints;
    //    private readonly IEnumerable<SagaEndpoint> endpoints;

    //    public SagaEndpointResolver(IEnumerable<SagaEndpoint> endpoints)
    //    {
    //        this.endpoints = endpoints;
    //    }


    //    public string Resolve(string endpointValue)
    //    {

    //        var endpointKey = endpointValue;
    //    }
    //}
    public interface ISagaEndpointResolver
    {
        string Resolve(string endpointValue);
        void Map(params global::SagaEndpoint[] endpoints);
    }

    public class MicroverseFlows : List<SagaFlow> { }
}
