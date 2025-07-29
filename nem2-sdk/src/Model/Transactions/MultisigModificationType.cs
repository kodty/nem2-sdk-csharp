using System.ComponentModel;

namespace io.nem2.sdk.src.Model.Transactions
{
    public static class MultisigCosignatoryModificationType
    {
        public enum Type
        {
            Add = 0x00,

            Remove = 0x01
        }

        public static byte GetValue(this Type value)
        {
            if (!Enum.IsDefined(typeof(Type), value))
                throw new InvalidEnumArgumentException(nameof(value), (int) value, typeof(Type));

            return (byte) value;
        }

        public static Type GetRawValue(byte value)
        {
            if (value != Type.Add.GetValue() && value != Type.Add.GetValue()) throw new ArgumentOutOfRangeException(nameof(value));

            return value == Type.Add.GetValue() ? Type.Add : Type.Remove;
        }
    }
}
