namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class AccountData
    {
       public AccountInfo Account {  get; set; }
       public string Id { get; set; }
    }

    public class AccountInfo
    {
        public int Version { get; set; }

        public string Address { get; set; }

        public ulong AddressHeight { get; set; }

        public string PublicKey { get; set; }

        public ulong PublicKeyHeight { get; set; }

        public int AccountType { get; set; }

        public SupplementalPublicKeys SupplementalPublicKeys { get; set; }

        public List<ActivityBucket> ActivityBuckets { get; set; }

        public List<MosaicTransfer> Mosaics { get; set; }

        public ulong Importance { get; set; }

        public ulong ImportanceHeight { get; set; }
    }

    public class ActivityBucket
    {
        public ulong StartHeight { get; set; }

        public ulong TotalFeesPaid { get; set; }

        public int BeneficiaryCount { get; set; }

        public ulong RawScore { get; set; }
    }

    public class Linked
    {
        public string PublicKey { get; set; }
    }
    public class Node
    {
        public string PublicKey { get; set; }
    }
    public class VRF
    {
        public string PublicKey { get; set; }
    }

    public class SupplementalPublicKeys
    {
        public Linked Linked { get; set; }

        public Node Node { get; set; }

        public VRF Vrf { get; set; }

        public Voting Voting { get; set; }
        
    }

    public class Voting
    {
        public List<VotingKeys> PublicKeys { get; set; }
    }
    public class VotingKeys
    {
        public string PublicKey { get; set; }

        public int StartEpoch { get; set; }

        public int EndEpoch { get; set; }
    }
}
