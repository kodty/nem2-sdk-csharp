namespace io.nem2.sdk.Model.Transactions.Messages
{
    public static class MessageType
    {
        public enum Type
        {
            ENCRYPTED = 0x01,

            UNENCRYPTED = 0x00
        }

        public enum CipherEncoding
        {
            CBC = 0,
            GCMSIV = 1
        }

        public static byte GetValue(this Type type)
        {
            return (byte)type;
        }

        public static byte GetValue(this CipherEncoding type)
        {
            return (byte)type;
        }

        public static Type GetRawValue(byte value)
        {
            return value == 0x01 ? Type.ENCRYPTED : Type.UNENCRYPTED;
        }
    }
}
