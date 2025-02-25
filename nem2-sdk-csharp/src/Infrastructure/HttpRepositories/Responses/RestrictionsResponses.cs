using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class MRestrictionData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mosaicRestrictionEntry")]
        public MosaicRestrictionEntry MosaicRestrictionEntry { get; set; }
    }

    public class MosaicRestrictionEntry
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("compositeHash")]
        public string CompositeHash { get; set; }

        [JsonProperty("entryType")]
        public int EntryType { get; set; }

        [JsonProperty("mosaicId")]
        public string MosaicId { get; set; }

        [JsonProperty("targetAddress")]
        public string TargetAddress { get; set; }

        [JsonProperty("restrictions")]
        public List<MosaicRestriction> Restrictions { get; set; }
    }

    public class Pagination
    {
        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
    }

    public class MosaicRestriction
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class MosaicRestrictions
    {
        [JsonProperty("data")]
        public List<MRestrictionData> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AccountRestrictions
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("restrictions")]
        public List<AccRestriction> Restrictions { get; set; }
    }

    public class ARestrictionData
    {
        [JsonProperty("accountRestrictions")]
        public AccountRestrictions AccountRestrictions { get; set; }
    }
    public class AccRestriction
    {
        [JsonProperty("restrictionFlags")]
        public int RestrictionFlags { get; set; }

        [JsonProperty("values")]
        public List<string> Values { get; set; }
    }

    public class AccountsRestrictions
    {
        [JsonProperty("data")]
        public List<ARestrictionData> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }


}
