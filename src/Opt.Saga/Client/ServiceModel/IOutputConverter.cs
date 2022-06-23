using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Opt.Saga.Core.Client.ServiceModel
{
    public interface IOutputConverter
    {
        JObject Convert(AggregateRouteKeys keys, IDictionary<string, string> data);
    }   

    public class DefaultOutputConverter : IOutputConverter
    {

        public JObject Convert(AggregateRouteKeys keys, IDictionary<string, string> data)
        {
            JObject obj = new JObject();

            var outputs = data
                 .Where(x => keys.Any(a => a.FlowKey == x.Key));


            var query = (from o in outputs
                         join k in keys on o.Key equals k.FlowKey
                         select new
                         {
                             k.FlowKey,
                             k.ResponsePropertyName,
                             o.Value
                         });

            foreach (var x in query)
            {
                var @event = SagaJsonCoverter.DeserializeObject<SagaProcessEventArgs>(x.Value);
                try
                {
                    obj[x.ResponsePropertyName] = JObject.Parse(@event.Message.Content);
                }
                catch (JsonReaderException)
                {

                    obj[x.ResponsePropertyName] = @event.Message.Content;

                }

            }

            return obj;




        }
    }

}