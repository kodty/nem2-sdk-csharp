using io.nem2.sdk.Infrastructure.Responses;
using io.nem2.sdk.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coppery
{
    public class ExtendEmbeddedBaseTransactionConverter : JsonConverter<EmbeddedBaseTransaction>
    {
        public override EmbeddedBaseTransaction? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var localReader = reader;

            var transaction = JsonSerializer.Deserialize<EmbeddedBaseTransaction>(ref localReader, options);

            if (transaction.Type == 16718)
            {
                var localLocalReader = reader;

                var composedTransaction = JsonSerializer.Deserialize<EmbeddedNamespaceRegistration>(ref localLocalReader, options);

                transaction.Type += composedTransaction.RegistrationType;
            }

            var t_type = transaction.Type.GetEmbeddedTypeValue();

            return (EmbeddedBaseTransaction)JsonSerializer.Deserialize(ref reader, t_type, options);
        }

        public override void Write(Utf8JsonWriter writer, EmbeddedBaseTransaction? value, JsonSerializerOptions options)
        {
            //serialize a DateTime? object
        }
    }
}
