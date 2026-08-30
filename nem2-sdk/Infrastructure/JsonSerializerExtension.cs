using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace io.nem2.sdk.Infrastructure
{
    internal static class JsonSerializerExtension
    {
        internal static T Deserialize<T>(string e)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            return JsonSerializer.Deserialize<T>(e, options);
        }

        internal static T Deserialize<T>(JsonNode e)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            return JsonSerializer.Deserialize<T>(e, options);
        }
    }
}
