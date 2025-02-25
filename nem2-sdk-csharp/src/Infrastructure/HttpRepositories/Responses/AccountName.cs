using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters;
using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class AccountName
    {
        [JsonProperty("address")]
        public string Address { get; set; }


        [JsonProperty("names")]
        public List<string> Names { get; set; }
    }

    public class NamespaceName
    {
        [JsonProperty("parentId")]
        public string ParentId { get; set; }


        [JsonProperty("id")]
        public string Id { get; set; }


        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MosaicName
    {
        [JsonProperty("mosaicId")]
        public string mosaicId { get; set; }


        [JsonProperty("names")]
        public List<string> names { get; set; }
    }
}
