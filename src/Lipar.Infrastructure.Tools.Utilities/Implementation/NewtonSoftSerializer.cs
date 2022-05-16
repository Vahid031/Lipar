using Lipar.Core.Contract.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Lipar.Infrastructure.Tools.Utilities.Implementation
{
    public class NewtonSoftSerializer : IJson
    {
        public string SerializeObject<TInput>(TInput input) =>
           input == null ? string.Empty :
           JsonConvert.SerializeObject(input, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        public TOutput DeserializeObject<TOutput>(string input) =>
            string.IsNullOrWhiteSpace(input) ? default :
            JsonConvert.DeserializeObject<TOutput>(input);

        public object DeserializeObject(string input, Type type) =>
            string.IsNullOrWhiteSpace(input) ? null :
            JsonConvert.DeserializeObject(input, type);
    }
}
