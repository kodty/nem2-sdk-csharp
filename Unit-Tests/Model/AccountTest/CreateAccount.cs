using Integration_Tests;
using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;
using System.Diagnostics;

namespace test.Model.AccountTest
{
    public class CreateAccount
    {
        [Test]
        public void CreateNewAccount()
        {
            var charCount = new int[32];

            char[] Base32Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();
            int[] sectorFound = new int[8];

            for (int i = 0; i < 100000000; i++)
            {
                var acc = Account.GenerateNewAccount(NetworkType.Types.TEST_NET);
                Debug.WriteLine(acc.Address.Plain);
                for (int e = 0; e < acc.Address.Plain.Length; e++)
                {
                    charCount[Base32Characters.ToList().IndexOf(acc.Address.Plain[e])]++;
                }

                if (acc.Address.Plain.Substring(0, 5).Contains('A'))
                {
                    sectorFound[0]++;
                }
                if (acc.Address.Plain.Substring(5, 5).Contains('A'))
                {
                    sectorFound[1]++;
                }
                if (acc.Address.Plain.Substring(10, 5).Contains('A'))
                {
                    sectorFound[2]++;
                }
                if (acc.Address.Plain.Substring(15, 5).Contains('A'))
                {
                    sectorFound[3]++;
                }
                if (acc.Address.Plain.Substring(20, 5).Contains('A'))
                {
                    sectorFound[4]++;
                }
                if (acc.Address.Plain.Substring(25, 5).Contains('A'))
                {
                    sectorFound[5]++;
                }
                if (acc.Address.Plain.Substring(30, 5).Contains('A'))
                {
                    sectorFound[6]++;
                }
                if (acc.Address.Plain.Substring(35, 4).Contains('A'))
                {
                    sectorFound[7]++;
                }
                Assert.AreEqual(64, acc.PublicAccount.PublicKey.Length);

                if (i > 300 && acc.Address.Plain.EndsWith("A")) break;
            }

            
            foreach (var item in charCount)
            {
                Debug.WriteLine(item);
            }
            Debug.WriteLine("");
            Array.Sort(charCount);

            foreach (var item in charCount)
            {
                Debug.WriteLine(item);
            }

            Debug.WriteLine("");

            foreach (var item in sectorFound)
            {
                Debug.WriteLine(item);
            }
        }

        [Test] 
        public void CreateNewAccountFromKey()
        {
            var acc = Account.CreateFromPrivateKey("575DBB3062267EFF57C970A336EBBC8FBCFE12C5BD3ED7BC11EB0481D7704CED", NetworkType.Types.TEST_NET);

            Assert.AreEqual("2E834140FD66CF87B254A693A2C7862C819217B676D3943267156625E816EC6F", acc.PublicAccount.PublicKey);             
        }

        [Test]
        public void TestPublicAccount1()
        {
            var pubAccount = new PublicAccount("87C45C6A2C87589786549BAD91568E56822507CA1D85D5B0E86B6F555231A4F8", NetworkType.Types.TEST_NET);
            
            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TCSJY245ZPSF5SSC4OBBKGOLQ3VEPSRIBTVXTTQ"));
            Debug.WriteLine(AddressEncoder.DecodeAddress(pubAccount.Address.Plain).ToHexLower());
            
        }


        [Test]
        public void TestPublicAccount2()
        {
            var pubAccount = new PublicAccount("17D0F0B4DF56A44DA77C38D377D2AE4F4A0BEA320B78D033B377890D318D0DC0", NetworkType.Types.TEST_NET);

            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TAMYTGVH3UEVZRQSD64LGSMPKNTKMASOIDNYROI"));
            Debug.WriteLine(pubAccount.Address.Plain);
        }

        [Test]
        public void TestPublicAccount3()
        {
            var pubAccount = new PublicAccount("F2ABE3C9CBFCC7E1AE9A3142856EA69B6153D7F7540AFC389A99E920072B7C67", NetworkType.Types.TEST_NET);

            Assert.That(pubAccount.Address.Plain, Is.EqualTo("TDSRY7NDJHLDNZVDB64I3VJ5GAJ5UOFNXMNQQZA"));
            Debug.WriteLine(pubAccount.Address.Plain);
        }
    }
}
