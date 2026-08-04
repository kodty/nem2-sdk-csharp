using Coppery;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicReclamationTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(DebtorImposed);
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(Amount);
        }

        public MosaicReclamationTransaction(Address debtorImposed, string mosaicId, ulong amount)
        {
            DebtorImposed = AddressEncoder.DecodeAddress(debtorImposed.Plain);
            MosaicId = mosaicId.FromHex().Reverse().ToArray();
            Amount = amount;          
        }

        public byte[] DebtorImposed { get; set; }

        public byte[] MosaicId { get; set; }

        public ulong Amount { get; set; }

        internal override int AddSize()
        {
            return 40;
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return TransactionTypes.Types.TOKEN_RECLAMATION;
        }
    }
}
