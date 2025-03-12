using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class MRestrictionData
    {
        public string Id { get; set; }

        public MosaicRestrictionEntry MosaicRestrictionEntry { get; set; }
    }

    public class MosaicRestrictionEntry
    {
        public int Version { get; set; }

        public string CompositeHash { get; set; }

        public int EntryType { get; set; }

        public string MosaicId { get; set; }

        public string TargetAddress { get; set; }

        public List<MosaicRestriction> Restrictions { get; set; }
    }

    public class Pagination
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }

    public class MosaicRestriction
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class MosaicRestrictions
    {
        [JsonProperty("data")]
        public List<MRestrictionData> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class AccountRestrictions
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("restrictions")]
        public List<Restrictions> Restrictions { get; set; }
    }

    public class RestrictionData
    {
        public AccountRestrictions AccountRestrictions { get; set; }
    }

    public class Restrictions
    {
        public int RestrictionFlags { get; set; }

        public List<string> Values { get; set; }
    }

    public class AccountsRestrictions
    {
        [JsonProperty("data")]
        public List<RestrictionData> Data { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }


}
