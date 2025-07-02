namespace io.nem2.sdk.Core.Crypto.Chaos.NaCl.Internal.Ed25519ref10
{
    internal static partial class ScalarOperations
    {
        internal static void sc_clamp(byte[] s, int offset)
        {
            s[offset + 0] &= 248;
            s[offset + 31] &= 127;
            s[offset + 31] |= 64;
        }
    }
}