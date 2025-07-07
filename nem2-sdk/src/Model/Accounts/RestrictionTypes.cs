using System.ComponentModel;

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

        public static Types GetRestrictionValue(this int type)
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

        public static List<Types> ExtractRestrictionFlags(this int value)
        {
            var values = new List<Types>();

            char[] actualBitwise = Convert.ToString(value, 2).PadLeft(16, '0').ToCharArray(0, 16);

            for (var x = 0; x < actualBitwise.Length; x++)
            {
                if (actualBitwise[x] == '1')
                {
                    string bitwiseType = new string('0', x) + '1' + new string('0', actualBitwise.Length - (1 + x));

                    values.Add(Convert.ToInt32(bitwiseType, 2).GetRestrictionValue());
                }
            }

            return values;
        }
    }
}
