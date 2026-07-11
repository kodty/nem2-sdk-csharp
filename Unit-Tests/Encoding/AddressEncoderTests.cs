using Coppery;
using io.nem2.sdk.src.Core.Utils;
using System.Diagnostics;

namespace Unit_Tests.Encoding
{
    internal class AddressEncodingTests
    {
        [Test]
        public void Base32EncodeAddress()
        {//&targetAddress=
            var testString = "31D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC";
            //var hexString = "687CBC80535BCEB01042608CF7A207BCC3A7C4318DF1BBF6";
            
            Assert.That(AddressEncoder.EncodeAddress(testString), Is.EqualTo("GHMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA"));
            //Assert.That(AddressEncoder.EncodeAddress(hexString), Is.EqualTo("NBQ7DD3DGCSDLFIEPWA3N2BAXYHLVM5J26SQA7I"));
        }

        [Test]
        public void HexDecodeBase32Address()

        {
            var address = "TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA";

            Assert.That(AddressEncoder.DecodeAddress(address).ToHex(), Is.EqualTo("98D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC"));
        }

        [Test]
        public void HexDecodeBase32Address1()

        {
            var address = "TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA";

            Assert.That(AddressEncoder.DecodeAddress(address).ToHex(), Is.EqualTo("98D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC"));
        }
    }
}
