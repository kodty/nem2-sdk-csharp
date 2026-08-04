using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.AccountRestrictions
{
    //AccountMosaic, AccountAddress, AccountOperation
    public class AccountRestrictionsTransaction : TransactionExtension
    {
        internal override void Extend(DataSerializer serializer)
        {
            serializer.SerializeProperty(RestrictionFlags);
            serializer.SerializeProperty(RestrictionsAdditionsCount);
            serializer.SerializeProperty(RestrictionsDeletionsCount);
            serializer.SerializeProperty(new byte[4]);
            serializer.SerializeProperty(_RestrictionAdditions);
            serializer.SerializeProperty(_RestrictionDeletions);
        }

        public AccountRestrictionsTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] restrictionAdditions, string[] restrictionsDeletions)
        {
            RestrictionFlags = restrictionFlags;
            _RestrictionAdditions = [];
            _RestrictionDeletions = [];
            RestrictionAdditions = restrictionAdditions;
            RestrictionsAdditionsCount = (byte)restrictionAdditions.Count();
            RestrictionDeletions = restrictionsDeletions;
            RestrictionsDeletionsCount = (byte)restrictionsDeletions.Count();

            Type = type;
        }

        private TransactionTypes.Types Type { get; set; }
        public ushort RestrictionFlags { get; set; }
        public byte RestrictionsAdditionsCount { get; set; }
        public byte RestrictionsDeletionsCount { get; set; }
        public byte[] _RestrictionAdditions{ get; set; }
        public byte[] _RestrictionDeletions{ get; set; }       

        private string[]? RestrictionDeletions { get { return null; } set => _RestrictionDeletions = ConvertFrom(value); }
        private string[]? RestrictionAdditions { get { return null; } set => _RestrictionAdditions = ConvertFrom(value); }

        private static byte[] ConvertFrom(string[] value)
        {
            int len = 0;

            foreach (var item in value)
            {
                if (item.IsHex()) len += item.Length / 2;
                if (item.IsBase32()) len += 24;
            }

            byte[] bitValues = new byte[len];

            int offset = 0;

            foreach (var item in value)
            {
                byte[] decoded = new byte[24];

                if (item.IsBase32())
                    decoded = AddressEncoder.DecodeAddress(item);

                if (item.IsHex())
                    decoded = item.FromHex();

                Buffer.BlockCopy(decoded, 0, bitValues, offset, decoded.Length);

                offset += decoded.Length;
            }

            return bitValues;
        }

        internal override int AddSize()
        {
            return (8 + _RestrictionAdditions.Length + _RestrictionDeletions.Length);
        }

        internal override byte SetVersion()
        {
            return 0x01;
        }

        internal override TransactionTypes.Types SetType()
        {
            return Type;
        }
    }
}
