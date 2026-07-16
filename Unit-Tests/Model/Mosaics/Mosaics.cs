using Coppery;
using io.nem2.sdk.Utils;
using System.Diagnostics;

namespace Unit_Tests.Model.Mosaics
{
    internal class Mosaics
    {
        [Test]
        public static void NamespaceCreate()
        {
            var symbolId = IdGenerator.GenerateId(0, "symbol", true);

            var mosaicId = IdGenerator.GenerateMosaicId(AddressEncoder.DecodeAddress("TATNE7Q5BITMUTRRN6IB4I7FLSDRDWZA37JGO5Q"), 812613930);

            var symbolXem = IdGenerator.GenerateId(symbolId, "xym", true);

            var root = IdGenerator.GenerateId(0, "testspace84324595869", true);

            var level2 = IdGenerator.GenerateId(root, "space12387246", true);

            Assert.That(DataConverter.ConvertFrom(root).ToHex(), Is.EqualTo("943B8E28D1D0BCAC"));

            Assert.That(DataConverter.ConvertFrom(level2).ToHex(), Is.EqualTo("FF2C791E29E6FE80"));

            Assert.AreEqual(DataConverter.ConvertFrom(mosaicId).ToHex(), "570FB3ED9379624C"); // new mosaic id

            Assert.That(DataConverter.ConvertFrom(symbolXem).ToHex(), Is.EqualTo("E74B99BA41F4AFEE")); // namespace ID
            
            Assert.That(DataConverter.ConvertFrom(symbolId).ToHex(), Is.EqualTo("A95F1F8A96159516"));
        }

        [Test]
        public static void Test16bitDataConverter()
        {
            var bytes = DataConverter.ConvertFrom(16961);

            Assert.That(bytes.ToHex(), Is.EqualTo("4142")); // little endian
            Assert.That(bytes.ConvertTo<ushort>(), Is.EqualTo(16961));
        }

        [Test]
        public static void Test32bitDataConverter()
        {
            var bytes = DataConverter.ConvertFrom(16961);

            Assert.That(bytes.ToHex(), Is.EqualTo("4142")); // little endian
            Assert.That(bytes.ConvertTo<uint>(), Is.EqualTo(16961)); 
        }

        [Test]
        public static void Test64bitDataConverter()
        {
            var array = DataConverter.ConvertFrom(812613930);

            Assert.That("2A816F30", Is.EqualTo(array.ToHex())); // little endian
        }

        [Test]
        public static void GenerateMosaicId()
        { 
            var decoded = AddressEncoder.DecodeAddress("TATNE7Q5BITMUTRRN6IB4I7FLSDRDWZA37JGO5Q");

            var id = IdGenerator.GenerateMosaicId(decoded, 713125680);
            var id2 = IdGenerator.GenerateMosaicId(decoded, 729902896);

            Assert.That(id != id2);
           
        }

        [Test]
        public static void ConvertUlongToHexId()
        {
            var id = DataConverter.ToHex(DataConverter.ConvertFrom(95442763262823));
        }
    }
}
