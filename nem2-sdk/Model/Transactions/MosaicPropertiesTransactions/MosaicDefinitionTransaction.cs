using Coppery;
using io.nem2.sdk.Model.Articles;
using System.Reflection;

namespace io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicDefinitionTransaction : VerifiableTransaction
    {
        public override PropertyInfo[] RetrieveProperties()
        {
            return
            [
                .. BaseProperties,
                GetType().GetProperty("MosaicId"),
                GetType().GetProperty("Duration"),
                GetType().GetProperty("Nonce"),
                GetType().GetProperty("Flags"),
                GetType().GetProperty("Divisibility")
            ];
        }

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

        public byte[] MosaicId { get; set; }

        public ulong Duration { get; set; }

        public uint Nonce { get; set; }

        public byte Flags { get; set; }

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
