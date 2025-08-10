using CopperCurve;
using io.nem2.sdk.src.Model.Articles;
using System.Text;

namespace io.nem2.sdk.src.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicDefinitionTransaction : Transaction
    {
        public MosaicDefinitionTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded) { }

        public MosaicDefinitionTransaction(string mosaicName, string namespaceId, string mosaicId, MosaicProperties properties, bool embedded) : base(embedded) {

            MosaicName = Encoding.UTF8.GetBytes(mosaicName);
            NamespaceId = namespaceId.FromHex();
            MosaicId = mosaicId.FromHex();
            Properties = properties.GetFlags();
        }

        public byte[] MosaicName { get; set; }

        public byte[] NamespaceId { get; set; }

        public byte[] MosaicId { get; set; }

        public byte Properties { get; set; }
    }
}
