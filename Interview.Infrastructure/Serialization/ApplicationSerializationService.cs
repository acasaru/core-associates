using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Interview.Application.Interfaces;

namespace Interview.Infrastructure.Serialization
{
    public class ApplicationSerializationService : IApplicationSerializationService
    {
        private readonly JsonSerializerSettings _serializationSettings;

        public ApplicationSerializationService()
        {
            _serializationSettings = new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public string Serialize<TObject>(TObject objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, _serializationSettings);
        }

        public TObject Deserialize<TObject>(string jsonRepresentation)
        {
            return jsonRepresentation != null ?
                JsonConvert.DeserializeObject<TObject>(jsonRepresentation, _serializationSettings):
                default;
        }
    }
}
