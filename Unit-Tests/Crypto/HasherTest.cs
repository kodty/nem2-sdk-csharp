using Coppery;
using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using TweetNaclSharp.Core.Extensions;

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
        public static void TestHash2()
        {
            string payload = "019854416A5D22B404000000B52E115A0200000098E21944E27CE919474CE22D4145725E322766E1A278E414050001000000000072C0212E67A08BCEE80300000000000068656C6C6F";
            byte[] signature = "16AB50DDDF9FC8958AB1D90700EFD9B96CB78B77BCF5F1E41CABE125A726ECF4FFB39FCB47808644AEBEA6D9F9B8007897C84226FCFEAFBC63438CE01EAC5306".FromHex();
            string signer = "91D5DCB54E185D3700DD88283D9DC8C3EDC58A18305BB2B933BBA252B516B452";
            string genHash = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4";

            string actualHash = "EC91D6E9ECB3BD4663AAB29A9AC589983C50A1D7FE6338F37026CF82D55FDB80";

            
            var final = TransactionExtensions.HashTransaction(signature, signer.FromHex(), genHash.FromHex(), payload.FromHex());

            Assert.AreEqual(final, actualHash);
        }
    }
}
