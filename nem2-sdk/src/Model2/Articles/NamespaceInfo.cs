using io.nem2.sdk.src.Model2.Accounts;

namespace io.nem2.sdk.src.Model2.Articles
{
    public class NamespaceInfo
    {
        public bool IsActive { get; }

        public int Index { get; }

        public string MetaId { get; }

        public NamespaceId ParentId { get; }

        public NamespaceTypes.Types NamespaceType { get; }

        public int Depth { get; }

        public List<NamespaceId> Levels { get; }
  
        public ulong StartHeight { get; }

        public ulong EndHeight { get; }

        public PublicAccount Owner { get; }

        public bool IsExpired => !IsActive;

        public NamespaceInfo(bool active, int index, string metaId, NamespaceTypes.Types namespaceId, int depth, List<NamespaceId> levels, NamespaceId parentId, ulong startHeight, ulong endHeight, PublicAccount owner)
        {
            IsActive = active;
            Owner = owner;
            Index = index;
            MetaId = metaId;
            NamespaceType = namespaceId;
            ParentId = parentId;
            StartHeight = startHeight;
            EndHeight = endHeight;
            Depth = depth;
            Levels = levels;
        }
    }
}
