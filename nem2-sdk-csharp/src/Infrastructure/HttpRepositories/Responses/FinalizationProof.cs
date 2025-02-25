using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class MessageGroup
    {
        [JsonProperty("stage")]
        public int Stage { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("hashes")]
        public List<object> Hashes { get; set; }

        [JsonProperty("signatures")]
        public List<object> Signatures { get; set; }
    }

    public class FinalizationProof
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("finalizationEpoch")]
        public int FinalizationEpoch { get; set; }

        [JsonProperty("finalizationPoint")]
        public int FinalizationPoint { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("messageGroups")]
        public List<MessageGroup> MessageGroups { get; set; }
    }
}
