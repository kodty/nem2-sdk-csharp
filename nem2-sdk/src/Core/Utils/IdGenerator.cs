using System.Text;
using Coppery;
using Org.BouncyCastle.Crypto.Digests;


namespace io.nem2.sdk.src.Core.Utils
{
    public static class IdGenerator
    {
        internal struct Constants
        {
            internal static long NamespaceBaseId = 0;
            internal static int NamespaceMaxDepth = 3;
            internal static string NamePattern = "/^[a-z0-9] [a-z0-9-_]*$/";
        }

        public static ulong GenerateId(byte[] hexAddress, uint nonce)
        {
            return ReturnId(hexAddress, nonce.ConvertFromUInt32().Reverse().ToArray());
        }

        
        public static ulong GenerateId(ulong parentId, string name)
        {
            var n = Encoding.UTF8.GetBytes(name);

            return ReturnId(n, parentId.ConvertFromUInt64().Reverse().ToArray(), true);
        }

        private static ulong ReturnId(byte[] n, byte[] p, bool nsFlag = false)
        {
            var hash = new Sha3Digest(256);

            hash.BlockUpdate(p, 0, p.Length);
            hash.BlockUpdate(n, 0, n.Length);

            var result = new byte[32];

            hash.DoFinal(result, 0);

            if(nsFlag)
                result[7] ^= (1 << 7);

            result = result.Take(8).Reverse().ToArray();

            return result.ConvertToUInt64();
        }    
    }
}
