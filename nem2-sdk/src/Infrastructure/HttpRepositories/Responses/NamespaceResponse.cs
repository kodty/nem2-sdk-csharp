
namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class Alias
    {
        public int Type { get; set; }

        public string MosaicId { get; set; }

        public string Address { get; set; }
    }

    public class NamespaceData
    {
        public string Id { get; set; }

        public NamespaceMeta Meta { get; set; }

        public Namespace Namespace { get; set; }
    }

    public class NamespaceMeta
    {
        public bool Active { get; set; }

        public int Index { get; set; }
    }

    public class Namespace
    {
        public int Version { get; set; }

        public int RegistrationType { get; set; }

        public int Depth { get; set; }

        public string Level0 { get; set; }

        public string Level1 { get; set; }

        public string Level2 { get; set; }

        public Alias Alias { get; set; }

        public string ParentId { get; set; }

        public string OwnerAddress { get; set; }

        public ulong StartHeight { get; set; }

        public ulong EndHeight { get; set; }
    }

    public class Namespace_Ids
    {
        public List<string> namespaceIds { get; set; }
    }
}
