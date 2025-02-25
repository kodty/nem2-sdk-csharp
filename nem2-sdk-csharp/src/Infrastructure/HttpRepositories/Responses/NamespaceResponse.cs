using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters;
using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class Alias
    {
        [JsonProperty("type")]
        public int Type { get; set; }


        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }


        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public class NamespaceDatum
    {
        [JsonProperty("id")]
        public string Id { get; set; }


        [JsonProperty("meta")]
        public NamespaceMeta Meta { get; set; }


        [JsonProperty("namespace")]
        public NamespaceData Namespace { get; set; }
    }

    public class NamespaceMeta
    {
        [JsonProperty("active")]
        public bool Active { get; set; }


        [JsonProperty("index")]
        public int Index { get; set; }
    }

    public class NamespaceData
    {
        [JsonProperty("version")]
        public int Version { get; set; }


        [JsonProperty("registrationType")]
        public int RegistrationType { get; set; }


        [JsonProperty("depth")]
        public int Depth { get; set; }


        [JsonProperty("level0")]
        public string Level0 { get; set; }


        [JsonProperty("level1")]
        public string Level1 { get; set; }


        [JsonProperty("level2")]
        public string Level2 { get; set; }


        [JsonProperty("alias")]
        public Alias Alias { get; set; }


        [JsonProperty("parentId")]
        public string ParentId { get; set; }


        [JsonProperty("ownerAddress")]
        public string OwnerAddress { get; set; }


        [JsonProperty("startHeight")]
        public ulong StartHeight { get; set; }


        [JsonProperty("endHeight")]
        public ulong EndHeight { get; set; }
    }

    public class Namespaces
    {
        [JsonProperty("data")]
        public List<NamespaceDatum> Data { get; set; }


        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
