using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.src.Export;

namespace Unit_Tests.Model.Mosaics
{
    internal class Mosaics
    {
        [Test]
        public static void NamespaceCreate()
        {
            var symbolId = IdGenerator.GenerateId(0, "symbol");

            Assert.That(DataConverter.ConvertFromUInt64(symbolId).ToHexUpper(), Is.EqualTo("A95F1F8A96159516"));

            var xymId = IdGenerator.GenerateId(symbolId, "xym");
            Assert.That(DataConverter.ConvertFromUInt64(xymId).ToHexUpper(), Is.EqualTo("E74B99BA41F4AFEE"));
        }

        [Test]
        public static void Test32bitDataConverter()
        {
            var bytes = DataConverter.ConvertFromUInt32(812613930);
            Assert.That(bytes.ToHexUpper(), Is.EqualTo("2A816F30"));
            Assert.That(bytes.ConvertToUInt32(), Is.EqualTo(812613930)); 
        }

        [Test]
        public static void Test64bitDataConverter()
        {
            var bytes = DataConverter.ConvertFromUInt64(812613930);
            Assert.That(bytes.ToHexUpper(), Is.EqualTo("2A816F3000000000"));
            Assert.That(DataConverter.ConvertToUInt64(bytes), Is.EqualTo(812613930));
        }

        [Test]
        public static void GenerateMosaicId()
        { 
            var decoded = AddressEncoder.DecodeAddress("TATNE7Q5BITMUTRRN6IB4I7FLSDRDWZA37JGO5Q");

            var id = IdGenerator.GenerateId(decoded, 713125680);
            var id2 = IdGenerator.GenerateId(decoded, 729902896);

            Assert.That(DataConverter.ConvertFromUInt64(id).ToHexUpper(), Is.EqualTo("570FB3ED9379624C"));
            Assert.That(DataConverter.ConvertFromUInt64(id2).ToHexUpper(), !Is.EqualTo("570FB3ED9379624C"));
        }  
    }
}
