namespace io.nem2.sdk.src.Model2.Articles
{
    public static class NamespaceTypes
    {
        public enum Types
        {
            SubNamespace = 0x01,

            RootNamespace = 0x00,          
        }

        public static byte GetValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new ArgumentException(nameof(type) + (int)type + typeof(Types));

            return (byte)type;
        }

        public static Types GetRawValue(byte type)
        {
            switch (type)
            {
                case 0x01:
                    return Types.SubNamespace;
                case 0x00:
                    return Types.RootNamespace;
                default:
                    throw new ArgumentException("invalid transaction type.");
            }
        }
    }
}
