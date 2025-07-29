namespace io.nem2.sdk.src.Model2.Articles
{
    public class MosaicInfo
    {
        public bool IsActive { get; }

        public int Index { get; }

        public string MetaId { get; }

        public NamespaceId NamespaceId { get; }

        public MosaicId MosaicId { get; }

        public ulong Supply { get; }

        public ulong Height { get; }
       
        public string OwnerPublicKey { get; }

        public MosaicProperties Properties { get; }

        public bool IsExpired => !IsActive;

        public bool IsSupplyMutable => Properties.IsSupplyMutable;

        public bool IsTransferable => Properties.IsTransferable;

        public bool IsLevyMutable => Properties.IsLevyMutable;

        public ulong Duration => Properties.Duration;

        public int Divisibility => Properties.Divisibility;

        public MosaicInfo(bool active, int index, string metaId, NamespaceId namespaceId, MosaicId mosaicId, ulong supply, ulong height, string ownerPublicKey, MosaicProperties properties)
        {   
            IsActive = active;
            Index = index;
            MetaId = metaId;
            NamespaceId = namespaceId;
            MosaicId = mosaicId;
            Supply = supply;
            Height = height;
            OwnerPublicKey = ownerPublicKey;
            Properties = properties;
        }
    }
}
