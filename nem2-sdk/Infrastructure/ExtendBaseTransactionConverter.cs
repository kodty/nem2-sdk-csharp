using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coppery
{
    public class ExtendBaseTransactionConverter : JsonConverter<BaseTransaction>
    {
        public override BaseTransaction? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var localReader = reader;

            var transaction = JsonSerializer.Deserialize<BaseTransaction>(ref localReader, options);

            if (transaction.Type == 16718)
            {
                var localLocalReader = reader;

                var composedTransaction = JsonSerializer.Deserialize<NamespaceRegistration>(ref localLocalReader, options);

                transaction.Type += composedTransaction.RegistrationType;
            }

            var t_type = transaction.Type.GetTypeValue();

            return (BaseTransaction)JsonSerializer.Deserialize(ref reader, t_type, options);
        }

        public override void Write(Utf8JsonWriter writer, BaseTransaction? value, JsonSerializerOptions options)
        {
            //serialize a DateTime? object
        }
    }
}
