using System.Text.RegularExpressions;

namespace CopperCurve
{
    public static class DataValidationUtils
    {
        public static bool IsHex(this string value, int len)
        {
            return Regex.Match(value, @"[0-9a-fA-F]{" + len + "}").Success;
        }

        public static bool IsHex(this string value)
        {
            return Regex.Match(value, @"[0-9a-fA-F]{" + value.Length + "}").Success;
        }

        public static bool IsBase32(this string value, int len)
        {
            return Regex.Match(value, @"[2-7a-zA-Z]{" + len + "}").Success;
        }

        public static bool IsBase32(this string value)
        {
            return Regex.Match(value, @"[2-7a-zA-Z]{" + value.Length + "}").Success;
        }
    }
}
