namespace io.nem2.sdk.src.Model2
{
    public static class NetworkType
    {
        public enum Types
        {
            MAIN_NET = 104,

            TEST_NET = 152,

            MIJIN = 96,

            MIJIN_TEST = 144
        }

        public static byte GetNetworkByte(this Types type)
        {
            return (byte)type;
        }
        public static Types GetNetwork(string name)
        {
            switch (name)
            {
                case "mijinTest":
                    return Types.MIJIN_TEST;
                case "mijin":
                    return Types.MIJIN;
                case "testnet":
                    return Types.TEST_NET;
                case "mainnet":
                    return Types.MAIN_NET;
                default:
                    throw new ArgumentException("invalid network name.");
            }
        }
        public static Types GetNetworkValue(this byte value)
        {
            switch ((int)value)
            {
                case 144:
                    return Types.MIJIN_TEST;
                case 142:
                    return Types.MIJIN;
                case 96:
                    return Types.TEST_NET;
                case 104:
                    return Types.MAIN_NET;
                default:
                    throw new ArgumentException("invalid network name.");
            }
        }
    }
}
