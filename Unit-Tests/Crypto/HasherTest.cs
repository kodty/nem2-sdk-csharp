using io.nem2.sdk.src.Export;
using Org.BouncyCastle.Crypto.Digests;

namespace Unit_Tests.Crypto
{
    internal class HasherTest
    {
        [Test] 
        public void TestHash()
        {
           
            byte[] payload = "A6151D4904E18EC288243028CEDA30556E6C42096AF7150D6A7232CA5DBA52BD2192E23DAA5FA2BEA3D4BD95EFA2389CD193FCD3376E70A5C097B32C1C62C80AF9D710211545F7CDDDF63747420281D64529477C61E721273CFD78F8890ABB4070E97BAA52AC8FF61C26D195FC54C077DEF7A3F6F79B36E046C1A83CE9674BA1983EC2FB58947DE616DD797D6499B0385D5E8A213DB9AD5078A8E0C940FF0CB6BF92357EA5609F778C3D1FB1E7E36C35DB873361E2BE5C125EA7148EFF4A035B0CCE880A41190B2E22924AD9D1B82433D9C023924F2311315F07B88BFD42850047BF3BE785C4CE11C09D7E02065D30F6324365F93C5E7E423A07D754EB314B5FE9DB4614275BE4BE26AF017ABDC9C338D01368226FE9AF1FB1F815E7317BDBB30A0F36DC69".FromHex();
            var original = "85FEF4EEC0B798E6F4CF29EB5B8D3F3096885EB88865DD62D5D0BD63ADE67384";

            var hash = new byte[32];
            
            var sha3Hasher = new Sha3Digest(256);
            sha3Hasher.BlockUpdate(payload, 0, payload.Length);
            sha3Hasher.DoFinal(hash, 0);

            Assert.That(hash.ToHex().ToUpper(), Is.EqualTo(original));

            for (int i = 0; i < hash.Length; i++)
            {
                Assert.That(original.FromHex()[i], Is.EqualTo(hash[i]));
            }

            Assert.That(original.FromHex(), Is.EqualTo(hash));
        }

        [Test] 
        public void TestHash1()
        {
            byte[] signature = "6ba8b1c24df6ace04c8f9b435011ecb1fb4269b4485b18c3d3ac0a19037b71573913b8a4dd7c1b2c8dcf8817e1c08bc2e32428c3de0893ea3ca34f71c12efa0b".FromHex();
            byte[] signer = "91d5dcb54e185d3700dd88283d9dc8c3edc58a18305bb2b933bba252b516b452".FromHex();
            byte[] genHash = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4".FromHex();
            byte[] txData = "000000000198415400000000000000640000001344a8041a98d9807ac250198ea57d689a7239dfa3b52e1506a3f71fdc000501000000000072c0212e67a08bce00000000000003e868656c6c6f".FromHex();
            
            string actualHash = "3F2BE873F569828C88CD0DE37BB31C998FA0AAEB3308A1FFBF3D01CE49E8E9F7";

            var concatonatedTransaction = signature.Concat(signer).Concat(genHash).Concat(txData).ToArray();

            var hash = new byte[32];
            var sha3Hasher = new Sha3Digest(256);

            sha3Hasher.BlockUpdate(concatonatedTransaction, 0, concatonatedTransaction.Length);

            sha3Hasher.DoFinal(hash, 0);

            Assert.That(hash.ToHex(), Is.EqualTo(actualHash));

        }
    }
}
