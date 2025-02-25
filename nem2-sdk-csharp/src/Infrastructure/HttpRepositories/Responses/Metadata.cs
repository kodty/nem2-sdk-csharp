using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class Metadata
    {
        [JsonProperty("id")] 
        public string Id { get; set; }

        [JsonProperty("metadataEntry")]
        public MetadataEntry MetadataEntry { get; set; }
    }

    public class MetadataEntry
    {
        [JsonProperty("version")]
        public int Version { get; set; }


        [JsonProperty("compositeHash")]
        public string CompositeHash { get; set; }


        [JsonProperty("sourceAddress")]
        public string SourceAddress { get; set; }


        [JsonProperty("targetAddress")]
        public string TargetAddress { get; set; }


        [JsonProperty("scopedMetadataKey")]
        public string ScopedMetadataKey { get; set; }


        [JsonProperty("targetId")]
        public string TargetId { get; set; }


        [JsonProperty("metadataType")]
        public int MetadataType { get; set; }


        [JsonProperty("value")]
        public string Value { get; set; }


        [JsonProperty("valueSize")]
        public int ValueSize { get; set; }

    }

    public class MetadataCollection
    {
        [JsonProperty("data")]
        public List<Metadata> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
