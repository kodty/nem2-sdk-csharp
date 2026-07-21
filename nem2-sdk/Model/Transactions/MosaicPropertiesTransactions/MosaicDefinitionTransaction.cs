using Coppery;
using io.nem2.sdk.Model.Articles;

namespace io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicDefinitionTransaction : VerifiableTransaction
    {
        public MosaicDefinitionTransaction(TransactionTypes.Types type, bool embedded) : base(TransactionTypes.Types.MOSAIC_DEFINITION, embedded) { }

        public MosaicDefinitionTransaction(string id, uint nonce, MosaicProperties properties, bool embedded) : base(TransactionTypes.Types.MOSAIC_DEFINITION, embedded) 
        {
            Version = 0x01;
            MosaicId = id.FromHex().Reverse().ToArray();
            Duration = properties.Duration;
            Flags = properties.GetFlags();
            Nonce = nonce;
            Divisibility = properties.Divisibility;

            Size += 22;
        }

        [Order(12)]
        public byte[] MosaicId { get; set; }

        [Order(13)]
        public ulong Duration { get; set; }

        [Order(14)]
        public uint Nonce { get; set; }

        [Order(14)]
        public byte Flags { get; set; }

        [Order(15)]
        public byte Divisibility { get; set; }

        public override MosaicDefinitionTransaction SetSigner(string signer)
        {
            Signer = signer.FromHex();

            return this;
        }

        public override void SetVersion(byte version)
        {
            if (version > 3) throw new Exception("invalid version");

            Version = version;
        }
    }
}
