namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class ExtendedBlockInfo
    {
        public ExtendedMeta Meta { get; set; }

        public Block Block { get; set; }

        public string Id { get; set; }
    }

    public class BlockInfo
    {
        public Meta Meta { get; set; }

        public Block Block { get; set; }
    }

    public class Block
    {

        public string Signature { get; set; }

        public string SignerPublicKey { get; set; }

        public int Version { get; set; }

        public byte Network { get; set; }

        public int Type { get; set; }
     
        public ulong Height { get; set; }
   
        public ulong Timestamp { get; set; }

        public ulong Difficulty { get; set; }

        public string ProofGamma { get; set; }

        public string ProofVerificationHash { get; set; }

        public string ProofScalar { get; set; }

        public string PreviousBlockHash { get; set; }

        public string TransactionsHash { get; set; }

        public string ReceiptsHash { get; set; }

        public string StateHash { get; set; }

        public string BeneficiaryAddress { get; set; }

        public int FeeMultiplier { get; set; }
    }

    public class Meta
    {
        public string Hash { get; set; }

        public string GenerationHash { get; set; }
    }
    public class ExtendedMeta
    {
        public string Hash { get; set; }

        public string GenerationHash { get; set; }

        public ulong TotalFee { get; set; }

        public int TotalTransactionsCount { get; set; }

        public List<string> StateHashSubCacheMerkleRoots { get; set; }

        public int TransactionsCount { get; set; }

        public int StatementsCount { get; set; }
    }
}
