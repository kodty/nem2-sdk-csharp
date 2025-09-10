using Coppery;
using System.Diagnostics;
using io.nem2.sdk.src.Core.Utils;

namespace Unit_Tests.Model.Mosaics
{
    internal class Mosaics
    {
        [Test]
        public static void NamespaceCreate()
        {
            var symbolId = IdGenerator.GenerateId(0, "symbol");

            var symbolId1 = IdGenerator.GenerateId(0, "xym");

            Assert.That(DataConverter.ConvertFrom(symbolId).ToHex(), Is.EqualTo("A95F1F8A96159516"));
            
            Assert.That(DataConverter.ConvertFrom(symbolId1).ToHex(), Is.EqualTo("84CB6A45853E78C4"));
            
            var xymId = IdGenerator.GenerateId(symbolId, "xym");

            Assert.That(DataConverter.ConvertFrom(xymId).ToHex(), Is.EqualTo("E74B99BA41F4AFEE"));
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

            var id = IdGenerator.GenerateId(decoded, 713125680);
            var id2 = IdGenerator.GenerateId(decoded, 729902896);

            Assert.That(DataConverter.ConvertFrom(id).ToHex(), Is.EqualTo("570FB3ED9379624C"));
            Assert.That(DataConverter.ConvertFrom(id2).ToHex(), !Is.EqualTo("570FB3ED9379624C"));
        }

        [Test]
        public static void ConvertUlongToHexId()
        {
            var id = DataConverter.ToHex(DataConverter.ConvertFrom(95442763262823));
        }
    }
}
