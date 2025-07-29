using System.ComponentModel;

namespace io.nem2.sdk.src.Model2
{
    /// <summary>
    /// Class TransactionTypes.
    /// </summary>
    public static class HashType
    {
        /// <summary>
        /// Enum Types
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// The transfer type
            /// </summary>
            SHA3_512 = 0x00,

        }

        /// <summary>
        /// Gets the value of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The int16 value of the type.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static byte GetHashTypeValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(Types));

            return (byte)type;
        }

        /// <summary>
        /// Gets the type for the given value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The Type associated with the given int16 value.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
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


