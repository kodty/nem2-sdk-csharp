using io.nem2.sdk.src.Model2;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Unit_Tests.Crypto
{
    internal class KeyPairTests
    {
        [Test]
        public static void TestKeyPair()
        {
            var text = File.ReadAllLines("C:\\Users\\kaili\\Documents\\Bitbucket-Repositories\\nem2-sdk-csharp\\Unit-Tests\\Crypto\\Vectors\\TextFile1.txt");

            var keys = JsonObject.Parse(String.Concat(text));

            for (var i = 0; i < 10000; i++)
            {
                var kp = SecretKeyPair.CreateFromPrivateKey(keys[i]["privateKey"].GetValue<string>());

                Assert.That(kp.PublicKeyString, Is.EqualTo(keys[i]["publicKey"].GetValue<string>()));
            }   
        }   
    }
}
