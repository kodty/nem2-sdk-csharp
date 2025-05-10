using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.src.Export;

namespace Unit_Tests.Encoding
{
    internal class AddressEncodingTests
    {
        [Test]
        public void Base32EncodeAddress()
        {    
            var hexString = "6861F18F6330A43595047D81B6E820BE0EBAB3A9D7A5007D";

            Assert.That(AddressEncoder.EncodeAddress(hexString), Is.EqualTo("NBQ7DD3DGCSDLFIEPWA3N2BAXYHLVM5J26SQA7I"));
        }

        [Test]
        public void HexDecodeBase32Address()
        {
          
            var hexString = "NBQ7DD3DGCSDLFIEPWA3N2BAXYHLVM5J26SQA7I";

            Assert.That(AddressEncoder.DecodeAddress(hexString).EncodeHexString(), Is.EqualTo("6861F18F6330A43595047D81B6E820BE0EBAB3A9D7A5007D"));
        }
    }
}
