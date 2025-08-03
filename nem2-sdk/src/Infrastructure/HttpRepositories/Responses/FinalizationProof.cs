namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class Signature
    {
        public Root Root { get; set; }
        public Bottom Bottom { get; set; }
    }

    public class Root
    {
        public string ParentPublicKey { get; set; }
        public string Signature { get; set; }
    }

    public class Bottom
    {
        public string ParentPublicKey { get; set; }
        public string Signature { get; set; }
    }

    public class MessageGroup
    {
        public int Stage { get; set; }
        public ulong Height { get; set; }
        public List<string> Hashes { get; set; }
        public List<Signature> Signatures { get; set; }
    }

    public class FinalizationProof
    {
        public int Version { get; set; }
        public int FinalizationEpoch { get; set; }
        public int FinalizationPoint { get; set; }
        public ulong Height { get; set; }
        public string Hash { get; set; }
        public List<MessageGroup> MessageGroups { get; set; }
    }
}
