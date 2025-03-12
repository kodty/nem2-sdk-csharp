using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class Metadata
    {
        public string Id { get; set; }

        public MetadataEntry MetadataEntry { get; set; }
    }

    public class MetadataEntry
    {
        public int Version { get; set; }

        public string CompositeHash { get; set; }

        public string SourceAddress { get; set; }

        public string TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public string TargetId { get; set; }

        public int MetadataType { get; set; }

        public string Value { get; set; }

        public int ValueSize { get; set; }

    }
}
