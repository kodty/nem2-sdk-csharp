using System.Text;
using Coppery;
using Org.BouncyCastle.Crypto.Digests;


namespace io.nem2.sdk.Utils
{
    public static class IdGenerator
    {
        internal struct Constants
        {
            internal static long NamespaceBaseId = 0;
            internal static int NamespaceMaxDepth = 3;
            internal static string NamePattern = "/^[a-z0-9] [a-z0-9-_]*$/";
        }

        public static ulong GenerateMosaicId(byte[] hexAddress, uint nonce)
        {
            return ReturnId(hexAddress, DataConverter.ConvertFrom(nonce), false);
        }
  
        public static ulong GenerateId(ulong parentId, string name, bool isNamespace)
        {
            var n = Encoding.UTF8.GetBytes(name);

            return ReturnId(n, DataConverter.ConvertFrom(parentId).Reverse().ToArray(), isNamespace);
        }

        public static ulong ReturnId(byte[] n, byte[] p, bool isNamespace)
        {
            var hash = new Sha3Digest(256);

            hash.BlockUpdate(p, 0, p.Length);
            hash.BlockUpdate(n, 0, n.Length);

            var result = new byte[32];

            hash.DoFinal(result, 0);

            result = result.Take(8).Reverse().ToArray();

            if (isNamespace) result[0] |= 128;

            return result.ConvertTo<ulong>();
        }    
    }
}
