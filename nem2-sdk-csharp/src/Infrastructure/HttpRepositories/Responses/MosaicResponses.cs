using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.JsonConverters;
using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class MosaicIds
    {
        [JsonProperty("mosaicIds")]
        public List<string> Mosaic_Ids { get; set; }
    }
    public class TransferMosaic
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }
    }

    public class MosaicEvent
    {
        [JsonProperty("mosaic")]
        public Mosaic MosaicInfo { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Mosaic
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("supply")]
        public ulong Supply { get; set; }

        [JsonProperty("startHeight")]
        public ulong StartHeight { get; set; }

        [JsonProperty("ownerAddress")]
        public string OwnerAddress { get; set; }

        [JsonProperty("revision")]
        public int Revision { get; set; }

        [JsonProperty("flags")]
        public int Flags { get; set; }

        [JsonProperty("divisibility")]
        public int Divisibility { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }
    }

    public class Mosaics
    {
        [JsonProperty("data")]
        public List<MosaicEvent> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
