using System.Security.Cryptography;

namespace io.nem2.sdk.Core.Crypto.Chaso.NaCl.Internal.Ed25519ref10
{
    internal static partial class Ed25519Operations
    {
        internal static void crypto_sign_keypair(byte[] pk, int pkoffset, byte[] sk, int skoffset, byte[] seed, int seedoffset)
        {
            GroupElementP3 A;
            int i;
            
            Array.Copy(seed, 0, sk, 0, 32);
            
            byte[] h = new byte[64];

            SHA512.HashData(seed, h);

            ScalarOperations.sc_clamp(h, 0);
            GroupOperations.ge_scalarmult_base(out A, h, 0);
            GroupOperations.ge_p3_tobytes(pk, 0, ref A);

            for (i = 0; i < 32; i++) sk[32 + i] = pk[i];

            Array.Clear(h, 0, h.Length);
        }
    }
}
