using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Mapping;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters
{
    internal class EventConverter : JsonConverter
    {
        public EventConverter(Type instanceType)
        {
            InstanceType = instanceType;
        }

        internal Type InstanceType { get; set; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ObjectComposer.GenerateObject(InstanceType, JToken.Load(reader));
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(object).IsAssignableFrom(objectType);
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
