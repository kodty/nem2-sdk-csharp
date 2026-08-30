using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Model.Articles;

namespace io.nem2.sdk.Model.Transactions
{
    public class MosaicDefinitionTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(Duration);
            serializer.SerializeProperty(Nonce);
            serializer.SerializeProperty(Flags);
            serializer.SerializeProperty(Divisibility);
        }

        public MosaicDefinitionTransaction(ulong id, uint nonce, MosaicProperties properties) 
        {
            MosaicId = id;
            Duration = properties.Duration;
            Flags = properties.GetFlags();
            Nonce = nonce;
            Divisibility = properties.Divisibility;
        }

        public ulong MosaicId { get; set; }

        public ulong Duration { get; set; }

        public uint Nonce { get; set; }

        public byte Flags { get; set; }

        public byte Divisibility { get; set; }

        internal override int AddSize()
        {
            return 22;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.MOSAIC_DEFINITION;
        }
    }
}
