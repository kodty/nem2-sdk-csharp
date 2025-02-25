using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class MerkleRoot
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }


        [JsonProperty("tree")]
        public Tree[] Tree { get; set; }
    }

    public class Tree
    {
        [JsonProperty("type")]
        public int Type { get; set; }


        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("encodedPath")]
        public string EncodedPath { get; set; }


        [JsonProperty("nibbleCount")]
        public int NibbleCount { get; set; }


        [JsonProperty("linkCount")]
        public int LinkCount { get; set; }


        [JsonProperty("linkMask")]
        public string LinkMask { get; set; }


        [JsonProperty("links")]
        public Links[] Links { get; set; }


        [JsonProperty("branchHash")]
        public string BranchHash { get; set; }
    }

    public class Links
    {
        [JsonProperty("bit")]
        public string Bit { get; set; }


        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
