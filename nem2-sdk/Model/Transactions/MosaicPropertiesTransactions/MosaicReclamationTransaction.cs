using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Transactions
{
    public class MosaicReclamationTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(DebtorImposed);
            serializer.SerializeProperty(MosaicId);
            serializer.SerializeProperty(Amount);
        }

        public MosaicReclamationTransaction(Address debtorImposed, ulong mosaicId, ulong amount)
        {
            DebtorImposed = Address.DecodeAddress(debtorImposed.Plain);
            MosaicId = mosaicId;
            Amount = amount;          
        }

        public byte[] DebtorImposed { get; set; }

        public ulong MosaicId { get; set; }

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
