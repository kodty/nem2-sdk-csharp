namespace io.nem2.sdk.src.Model.Transactions.Messages
{
    public static class MessageType
    {
        public enum Type
        {
            ENCRYPTED = 0x01,

            UNENCRYPTED = 0x00
        }

        public static byte GetValue(this Type type)
        {
            return (byte)type;
        }

        public static Type GetRawValue(byte value)
        {
            return value == 0x01 ? Type.ENCRYPTED : Type.UNENCRYPTED;
        }
    }
}
