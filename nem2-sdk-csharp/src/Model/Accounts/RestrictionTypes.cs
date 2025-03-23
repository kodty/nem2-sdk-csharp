using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace io.nem2.sdk.Model.Accounts
{
    public static class RestrictionTypes
    {
        public enum Types
        {
            ADDRESS = 0x1,
            MOSAIC_ID = 0x2,
            TRANSACTION_TYPE = 0x4,
            OUTGOING = 0x4000,
            BLOCK = 0x8000,
        }

        public static ushort GetValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new InvalidEnumArgumentException(nameof(type), (ushort)type, typeof(Types));

            return (ushort)type;
        }

        public static Types GetRawValue(this int type)
        {
            switch (type)
            {
                case 0x1:
                    return Types.ADDRESS;
                case 0x2:
                    return Types.MOSAIC_ID;
                case 0x4:
                    return Types.TRANSACTION_TYPE;
                case 0x4000:
                    return Types.OUTGOING;
                case 0x8000:
                    return Types.BLOCK;
                default:
                    throw new ArgumentException("invalid transaction type.");
            }
        }
    }
}
