using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.nem2.sdk.Model2.Transactions.MosaicPropertiesTransactions
{
    public class MosaicDefinitionTransaction1 : Transaction1
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
