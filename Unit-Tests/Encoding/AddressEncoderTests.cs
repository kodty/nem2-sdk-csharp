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
        {//&targetAddress=
            var testString = "684730D07E8EF59C26C3259696730C75F6E7216730E8C9C8";
            var hexString = "687CBC80535BCEB01042608CF7A207BCC3A7C4318DF1BBF6";
            Debug.WriteLine(AddressEncoder.EncodeAddress(testString));
            Assert.That(AddressEncoder.EncodeAddress(testString), Is.EqualTo("NASYMBOLLK6FSL7GSEMQEAWN7VW55ZSZU25TBOA"));
            Assert.That(AddressEncoder.EncodeAddress(hexString), Is.EqualTo("NBQ7DD3DGCSDLFIEPWA3N2BAXYHLVM5J26SQA7I"));
        }

        [Test]
        public void HexDecodeBase32Address()
        {
            Debug.WriteLine(AddressEncoder.DecodeAddress("TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA").Length);
            var hexString = "TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA";

            Assert.That(AddressEncoder.DecodeAddress(hexString).ToHexUpper(), Is.EqualTo("98D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC"));
        }
    }
}
