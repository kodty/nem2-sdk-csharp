using CopperCurve;
using System.Diagnostics;
using System.Text;

namespace io.nem2.sdk.src.Model.Transactions.AccountRestrictions
{
    //AccountMosaic, AccountAddress, AccountOperation
    public class AccountRestrictionsTransaction : Transaction
    {
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
        }

        public ushort RestrictionFlags { get; set; }
        public byte RestrictionsAdditionsCount { get; set; }
        public byte RestrictionsDeletionsCount { get; set; }
        public uint Account_​restriction_​transaction_​body_​reserved_​1 { get; set; }
        public byte[] _RestrictionAdditions{ get; set; }
        public byte[] _RestrictionDeletions{ get; set; }
       
        private string[] RestrictionDeletions
        {
            get
            {
                return RestrictionDeletions;
            }
            set
            {
                _RestrictionDeletions.Concat(CompileValues(value));
            }
        }    

        private string[] RestrictionAdditions
        {
            get 
            { 
                return RestrictionAdditions; 
            }
            set
            {
                _RestrictionAdditions.Concat(CompileValues(value));
            }
        }

        private static byte[] CompileValues(string[] value)
        {
            byte[] bitValues = new byte[] { };
            int offset = 0;

            foreach (var item in value)
            {
                byte[] decoded = new byte[24];

                if (item.IsBase32())
                    decoded = AddressEncoder.DecodeAddress(item);

                if (item.IsHex())
                    decoded = item.FromHex();

                offset += LocalBlockCopy(ref decoded, ref bitValues, item, offset);
            }

            return bitValues;
        }

        private static int LocalBlockCopy(ref byte[] src, ref byte[] destination, string item, int offset )
        {
            Array.Resize(ref src, destination.Length + src.Length);

            Buffer.BlockCopy(src, 0, destination, offset, src.Length);

            return src.Length;
        }
    }
}
