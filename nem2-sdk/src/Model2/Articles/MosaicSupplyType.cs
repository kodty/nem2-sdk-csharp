namespace io.nem2.sdk.src.Model2.Articles
{
    public static class MosaicSupplyType
    {
        public enum Type
        {
            DECREASE = 0,

            INCREASE = 1
        }

        public static byte GetValue(this Type type)
        {
            return (byte) type;
        }

        public static Type GetRawValue(byte value)
        {
            switch (value)
            {
                case 0:
                    return Type.DECREASE;
                case 1:
                    return Type.INCREASE;
                default:
                    throw new ArgumentException(value + " is not a valid value");
            }
        }
    }
}


