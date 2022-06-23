using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opt.Saga.Core
{
    public class KeyValueObject
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class SagaStepEndpoint
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public List<KeyValueObject> Headers { get; set; }
        // [JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
        // public JObject? Headers { get; set; }
        //public SagaStepEndpoint()
        //{
        //    Headers = new();
        //}
        //public EndpointConfig(string url, HttpMethod method)
        //{
        //    Url = url;
        //    Method = method;
        //}
        //internal HttpMethod GetHttpMethod()
        //{
        //    return new HttpMethod(Method.ToUpper());
        //}
    }


    //public class DictionaryStringObjectJsonConverter : JsonConverter<Dictionary<string, string>>
    //{
    //    public override Dictionary<string, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        if (reader.TokenType != JsonTokenType.StartObject)
    //        {
    //            throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");
    //        }

    //        var dictionary = new Dictionary<string, string>();
    //        while (reader.Read())
    //        {
    //            if (reader.TokenType == JsonTokenType.EndObject)
    //            {
    //                return dictionary;
    //            }

    //            if (reader.TokenType != JsonTokenType.PropertyName)
    //            {
    //                throw new JsonException("JsonTokenType was not PropertyName");
    //            }

    //            var propertyName = reader.GetString();

    //            if (string.IsNullOrWhiteSpace(propertyName))
    //            {
    //                throw new JsonException("Failed to get property name");
    //            }

    //            reader.Read();

    //            dictionary.Add(propertyName, ExtractValue(ref reader, options));
    //        }

    //        return dictionary;
    //    }

    //    public override void Write(Utf8JsonWriter writer, Dictionary<string, string> value, JsonSerializerOptions options)
    //    {
    //        JsonSerializer.Serialize(writer, value, options);
    //    }

    //    private string ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
    //    {
    //        switch (reader.TokenType)
    //        {
    //            case JsonTokenType.String:
    //                //if (reader.TryGetDateTime(out var date))
    //                //{
    //                //    return date.to;
    //                //}
    //                return reader.GetString();
    //            //case JsonTokenType.False:
    //            //    return "false";
    //            //case JsonTokenType.True:
    //            //    return true.ToString();
    //            //case JsonTokenType.Null:
    //            //    return null;
    //            //case JsonTokenType.Number:
    //            //    if (reader.TryGetInt64(out var result))
    //            //    {
    //            //        return result.tos;
    //            //    }
    //            //    return reader.GetDecimal().ToString();
    //            //case JsonTokenType.StartObject:
    //            //    return Read(ref reader, null, options);
    //            //case JsonTokenType.StartArray:
    //            //    var list = new List<object>();
    //            //    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
    //            //    {
    //            //        list.Add(ExtractValue(ref reader, options));
    //            //    }
    //            //    return list;
    //            default:
    //                throw new JsonException($"'{reader.TokenType}' is not supported");
    //        }
    //    }
    //}
}
