using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.Buffers.Model
{
    public class ReceiptDatum
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("statement")]
        public TransactionStatement Statement { get; set; }
    }

    public class Meta
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }

    public class Receipt
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("senderAddress")]
        public string SenderAddress { get; set; }

        [JsonProperty("recipientAddress")]
        public string RecipientAddress { get; set; }
    }

    public class TransactionStatements
    {
        [JsonProperty("data")]
        public List<TransactionData> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class Source
    {
        [JsonProperty("primaryId")]
        public long PrimaryId { get; set; }

        [JsonProperty("secondaryId")]
        public long SecondaryId { get; set; }
    }

    public class TransactionStatement
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("receipts")]
        public List<Receipt> Receipts { get; set; }
    }
  
    public class AddressDatum
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("statement")]
        public AddressStatement Statement { get; set; }
    }

    public class ResolutionEntry
    {
        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("resolved")]
        public string Resolved { get; set; }
    }

    public class AddressStatements
    {
        [JsonProperty("data")]
        public List<AddressDatum> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class AddressStatement
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("unresolved")]
        public string Unresolved { get; set; }

        [JsonProperty("resolutionEntries")]
        public List<ResolutionEntry> ResolutionEntries { get; set; }
    }

    public class MosaicDatum
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("statement")]
        public MosaicStatement Statement { get; set; }
    }

    public class MosaicStatements
    {
        [JsonProperty("data")]
        public List<MosaicDatum> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class MosaicStatement
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("unresolved")]
        public string Unresolved { get; set; }

        [JsonProperty("resolutionEntries")]
        public List<ResolutionEntry> ResolutionEntries { get; set; }
    }


}
