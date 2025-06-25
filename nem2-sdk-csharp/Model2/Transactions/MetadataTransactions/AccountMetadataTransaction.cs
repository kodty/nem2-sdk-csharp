using io.nem2.sdk.src.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.nem2.sdk.Model2.Transactions.MetadataTransactions
{
    public class AccountMetadataTransaction1 : Transaction1
    {

        public AccountMetadataTransaction1(string targetAddress, string scopedKey, short valueSizeDelta, short valueSize, byte[] value)
        {
            TargetAddress = AddressEncoder.DecodeAddress(targetAddress);
            ScopedMetadataKey = scopedKey;
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            Value = value;
        }
        public byte[] TargetAddress { get; set; }

        public string ScopedMetadataKey { get; set; }

        public short ValueSizeDelta { get; set; }

        public short ValueSize { get; set; }

        public byte[] Value { get; set; }
    }
}
