using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.src.Export;
using System.Diagnostics;

namespace Unit_Tests.Encoding
{
    internal class AddressEncodingTests
    {
        [Test]
        public void Base32EncodeAddress()
        {
            var testString = "68258605CB5ABC592FE691190202CDFD6DDEE659A6BB30B8";
            var hexString = "6861F18F6330A43595047D81B6E820BE0EBAB3A9D7A5007D";

            Assert.That(AddressEncoder.EncodeAddress(testString), Is.EqualTo("NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA"));
            Assert.That(AddressEncoder.EncodeAddress(hexString), Is.EqualTo("NBQ7DD3DGCSDLFIEPWA3N2BAXYHLVM5J26SQA7I"));
        }

        [Test]
        public void HexDecodeBase32Address()
        {
            Debug.WriteLine(AddressEncoder.DecodeAddress("ND4HZXKWPLSC3CEY4ELLIRB56TX4PQGTMZH6GLI").ToHexLower());
            var hexString = "NBQ7DD3DGCSDLFIEPWA3N2BAXYHLVM5J26SQA7I";

            Assert.That(AddressEncoder.DecodeAddress(hexString).EncodeHexString(), Is.EqualTo("6861F18F6330A43595047D81B6E820BE0EBAB3A9D7A5007D"));
        }
    }
}
