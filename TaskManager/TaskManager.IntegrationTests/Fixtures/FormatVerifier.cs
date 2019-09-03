using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;

namespace TaskManager.IntegrationTests.Fixtures
{
    public static class FormatVerifier
    {
        public static bool StringIsValidAs(string input, Type type)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(type);

            var resultingObject = JToken.Parse(input);

            return resultingObject.IsValid(schema);
        }
    }
}