using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using io.nem2.sdk.src.Model.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters
{
    public class TransactionConverter : JsonConverter
    {
        public TransactionConverter(Type instanceType, QueryModel model = null)
        { 
            InstanceType = instanceType;  
            Model = model;
        }

        Type InstanceType { get; set; }
        QueryModel Model { get; set; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ObjectComposer.GenerateObject(InstanceType, JToken.Load(reader));       
        }
     
        public override bool CanConvert(Type objectType)
        {
            return typeof(TransactionData).IsAssignableFrom(objectType);
        }

        // avoids self object referencing loop, no idea why and couldnt be bothered figuring it out.
        // Deserialization of object still works with JsonConvert but original Json structure from API is reconfigured.
        // Issue not present when deserialization parameterised with standard JsonConverter
        public override bool CanWrite
        {
            get { return false; }
        }

        // CanWrite prevents triggering of WriteJson
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {  
            serializer.Serialize(writer, (value));
        }
 
    }
}
