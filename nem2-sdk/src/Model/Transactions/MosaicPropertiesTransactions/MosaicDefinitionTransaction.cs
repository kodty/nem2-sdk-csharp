using io.nem2.sdk.src.Model.Articles;

namespace io.nem2.sdk.src.Model.Transactions.MosaicPropertiesTransactions
{
    public class MosaicDefinitionTransaction1 : Transaction
    {
        public MosaicDefinitionTransaction1(string mosaicName, NamespaceId namespaceId, MosaicId mosaicId, MosaicProperties properties) {

            MosaicName = mosaicName;
            NamespaceId = namespaceId;
            MosaicId = mosaicId;
            Properties = properties.GetFlags();
        }

        public string MosaicName { get; }

        public NamespaceId NamespaceId { get; }

        public MosaicId MosaicId { get; }

        public byte Properties { get; }

    }
}
