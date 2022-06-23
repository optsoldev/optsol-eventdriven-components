using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class SagaJsonCoverter
    {
        public static string SerializeObject(object data)
        {
            var options = new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };


            dynamic json = System.Text.Json.JsonSerializer.Serialize(data, options);
            return json.ToString();
        }
        public static TResult DeserializeObject<TResult>(string data, JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
            }

            return System.Text.Json.JsonSerializer.Deserialize<TResult>(data, options);
        }
    }
}
