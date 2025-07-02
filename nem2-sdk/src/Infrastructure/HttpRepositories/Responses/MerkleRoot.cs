namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class MerklePath
    {
        public string Hash { get; set; }
        public string Position { get; set; }
    }

    public class MerkleRoot
    {
        public string Raw { get; set; }

        public List<Tree> Tree { get; set; }
    }

    public class Tree
    {
        public int Type { get; set; }

        public string Path { get; set; }

        public string EncodedPath { get; set; }

        public int NibbleCount { get; set; }

        public string LinkMask { get; set; }

        public List<LinkBit> Links { get; set; }

        public string BranchHash { get; set; }
       
        public string Value { get; set; }

        public string LeafHash { get; set; }
    }

    public class LinkBit
    {
        public string Bit { get; set; }

        public string Link { get; set; }
    }
}
