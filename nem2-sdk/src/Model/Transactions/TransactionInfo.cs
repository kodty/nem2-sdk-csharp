namespace io.nem2.sdk.Model.Transactions
{
    public class TransactionInfo
    {

        public ulong Height { get; }

        public int? Index { get; }

        public string Id { get; }

        public string Hash { get; }

        public string MerkleComponentHash { get; }

        public string AggregateHash { get; }

        public string AggregateId { get; }

        internal TransactionInfo(ulong height, int? index, string id, string hash, string merkleComponentHash, string aggregateHash, string aggregateId)
        {
            Height = height;
            Index = index;
            Id = id;
            Hash = hash;
            MerkleComponentHash = merkleComponentHash;
            AggregateHash = aggregateHash;
            AggregateId = aggregateId;
        }

        public static TransactionInfo CreateAggregate(ulong height, int? index, string id, string aggregateHash, string aggregateId)
        {
            return new TransactionInfo(height, index, id, null, null, aggregateHash, aggregateId);
        }

        public static TransactionInfo Create(ulong height, int? index, string id, string hash, string merkleComponentHash)
        {
            return new TransactionInfo(height, index, id, hash, merkleComponentHash, null, null);
        }

        public static TransactionInfo Create(ulong height, string hash, string merkleComponentHash)
        {
            return new TransactionInfo(height, null, null, hash, merkleComponentHash, null, null);
        }
    }
}
