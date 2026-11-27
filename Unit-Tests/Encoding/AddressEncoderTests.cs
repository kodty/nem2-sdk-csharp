using io.nem2.sdk.Infrastructure;
using io.nem2.sdk.Model.Accounts;

namespace Unit_Tests.Encoding
{
    internal class AddressEncodingTests
    {
        [Test]
        public void Base32EncodeAddresstest()
        {//&targetAddress=
            var testString = "68F35B3CA84DB724B948E7A6B2DAB065E2FD4D51139BA3A6";
            //var hexString = "687CBC80535BCEB01042608CF7A207BCC3A7C4318DF1BBF6";

            Assert.That(Address.EncodeAddress(testString), Is.EqualTo("."));
            Assert.That(Address.EncodeAddress(testString).IsBase32(39));
        }//


        [Test]
        public void Base32EncodeAddress()
        {//&targetAddress=
            var testString = "31D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC";
            //var hexString = "687CBC80535BCEB01042608CF7A207BCC3A7C4318DF1BBF6";
            
            Assert.That(Address.EncodeAddress(testString), Is.EqualTo("GHMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA"));
            Assert.That(Address.EncodeAddress(testString).IsBase32(39));
        }//68F35B3CA84DB724B948E7A6B2DAB065E2FD4D51139BA3A6

        [Test]
        public void HexDecodeBase32Address()
        {
            var address = "TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA";

            Assert.That(Address.DecodeAddress(address).ToHex(), Is.EqualTo("98D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC"));
            Assert.That(Address.DecodeAddress(address).ToHex().IsHex(32));
        }

        [Test]
        public void HexDecodeBase32Address1()

        {
            var address = "TDMYA6WCKAMY5JL5NCNHEOO7UO2S4FIGUP3R7XA";

            Assert.That(Address.DecodeAddress(address).ToHex(), Is.EqualTo("98D9807AC250198EA57D689A7239DFA3B52E1506A3F71FDC"));
            Assert.That(Address.DecodeAddress(address).ToHex().IsHex(32));
        }
    }
}
