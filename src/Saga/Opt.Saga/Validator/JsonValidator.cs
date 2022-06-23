using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;

namespace Opt.Saga.Core.Validator
{
    public class JsonSchemaValidator
    {
        private readonly SchemaGenerator schemaGenerator;

        public JsonSchemaValidator(SchemaGenerator schemaGenerator)
        {
            this.schemaGenerator = schemaGenerator;
            
        }
        public Dictionary<string, JSchema> Schemas = new();
        public void Add(params Type[] types)
        {

            foreach (var type in types)
            {
                JSchema schemaGen = schemaGenerator.Generate(type);
                Schemas.Add(type.Name, schemaGen);
            }
        }
        public bool Validate(string type, string json, out IList<string> errors)
        {
            var schema = this.Schemas[type];
            if (schema == null)
                throw new ArgumentException($"Nenhum json schema encontrado para o tipo {type}");

            bool valid = JObject.Parse(json).IsValid(schema, out errors);
            return valid;
        }
    }
    public class SchemaGenerator
    {
        public JSchema Generate(Type type)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            generator.ContractResolver = new CamelCasePropertyNamesContractResolver();

            JSchema schemaGen = generator.Generate(type);
            return schemaGen;
        }
    }
}
