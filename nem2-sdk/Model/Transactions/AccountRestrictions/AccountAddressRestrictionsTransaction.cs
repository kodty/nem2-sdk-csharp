using Coppery;
using io.nem2.sdk.Utils;

namespace io.nem2.sdk.Model.Transactions.AccountRestrictions
{
    //AccountMosaic, AccountAddress, AccountOperation
    public class AccountRestrictionsTransaction : Transaction
    {
        public AccountRestrictionsTransaction(TransactionTypes.Types type, bool embedded) : base(type, embedded)
        {

        }

        public AccountRestrictionsTransaction(TransactionTypes.Types type, ushort restrictionFlags, string[] restrictionAdditions, string[] restrictionsDeletions, bool embedded) : base(type, embedded)
        {
            RestrictionFlags = restrictionFlags;
            _RestrictionAdditions = [];
            _RestrictionDeletions = [];
            RestrictionAdditions = restrictionAdditions;
            RestrictionsAdditionsCount = (byte)restrictionAdditions.Count();
            RestrictionDeletions = restrictionsDeletions;
            RestrictionsDeletionsCount = (byte)restrictionsDeletions.Count();
            Account_​restriction_​transaction_​body_​reserved_​1 = 0;

            Size += (uint)(8 + _RestrictionAdditions.Length + _RestrictionDeletions.Length);
        }

        public ushort RestrictionFlags { get; set; }
        public byte RestrictionsAdditionsCount { get; set; }
        public byte RestrictionsDeletionsCount { get; set; }
        public uint Account_​restriction_​transaction_​body_​reserved_​1 { get; set; }
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
    }
}
