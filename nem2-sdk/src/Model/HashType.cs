using System.ComponentModel;

namespace io.nem2.sdk.src.Model
{
    public static class HashType
    {
        public enum Types
        {
            SHA3_512 = 0x00,

        }

        public static byte GetHashTypeValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(Types));

            return (byte)type;
        }

        public static Types GetRawValue(byte type)
        {
            switch (type)
            {
                case 0x00:
                    return Types.SHA3_512;                   
                default:
                    throw new ArgumentException("invalid transaction type.");
            }
        }
    }
}


