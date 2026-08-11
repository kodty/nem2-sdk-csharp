using Coppery;
using Org.BouncyCastle.Crypto.Digests;

namespace Unit_Tests.Crypto
{
    internal class MerkleRootHashTests
    {
        [Test]
        public void TestMerkleTree()
        {
            var hash0 = "36C8213162CDBC78767CF43D4E06DDBE0D3367B6CEAEAEB577A50E2052441BC8".FromHex();
            var hash1 = "8A316E48F35CDADD3F827663F7535E840289A16A43E7134B053A86773E474C28".FromHex();
            var hash2 = "6D80E71F00DFB73B358B772AD453AEB652AE347D3E098AE269005A88DA0B84A7".FromHex();
            var hash3 = "2AE2CA59B5BB29721BFB79FE113929B6E52891CAA29CBF562EBEDC46903FF681".FromHex();
            var hash4 = "421D6B68A6DF8BB1D5C9ACF7ED44515E77945D42A491BECE68DA009B551EE6CE".FromHex();
            var hash5 = "7A1711AF5C402CFEFF87F6DA4B9C738100A7AC3EDAD38D698DF36CA3FE883480".FromHex();
            var hash6 = "1E6516B2CC617E919FAE0CF8472BEB2BFF598F19C7A7A7DC260BC6715382822C".FromHex();
            var hash7 = "410330530D04A277A7C96C1E4F34184FDEB0FFDA63563EFD796C404D7A6E5A20".FromHex();

            var even = new byte[][] { hash0, hash1, hash2, hash3, hash4, hash5, hash6, hash7 };
            var odd = new byte[][] { hash0, hash1, hash2, hash3, hash4 };

            var evenresult = CalculateMerkleRoot(even);
            var oddresult = CalculateMerkleRoot(odd);

            Assert.That(evenresult.ToHex() == "7D853079F5F9EE30BDAE49C4956AF20CDF989647AFE971C069AC263DA1FFDF7E");
            Assert.That(oddresult.ToHex() == "DEFB4BF7ACF2145500087A02C88F8D1FCF27B8DEF4E0FDABE09413D87A3F0D09");
        }


        private byte[][] Reduce(byte[][] originalHashes)
        {
            byte[][] reducedHashes = new byte[originalHashes.Length / 2][];

            var sha3Hasher = new Sha3Digest(256);
            
            for (uint i = 0; i < originalHashes.Length; i += 2)
            {
                sha3Hasher.BlockUpdate(originalHashes[i], 0, 32);
                sha3Hasher.BlockUpdate(originalHashes[i + 1], 0, 32);

                reducedHashes[i / 2] = new byte[32];
                sha3Hasher.DoFinal(reducedHashes[i / 2]);
            }

            return reducedHashes;
        }

        private byte[] CalculateMerkleRoot(byte[][] embeddedTransactionHashes)
        {
            byte[][] hashes = embeddedTransactionHashes;

            for (int x = 0; x < 3; x++)
            {
                if (hashes.Length % 2 != 0)
                {
                    hashes = [ .. hashes, hashes[hashes.Length - 1] ];
                }

                hashes = Reduce(hashes);
            }
               
            return hashes[0];
        }
    }
}
