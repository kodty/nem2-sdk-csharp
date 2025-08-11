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

            Assert.That(DataConverter.ConvertFromUInt64(symbolId).ToHex(), Is.EqualTo("A95F1F8A96159516"));
            
            Assert.That(DataConverter.ConvertFromUInt64(symbolId1).ToHex(), Is.EqualTo("84CB6A45853E78C4"));
            
            var xymId = IdGenerator.GenerateId(symbolId, "xym");

            Assert.That(DataConverter.ConvertFromUInt64(xymId).ToHex(), Is.EqualTo("E74B99BA41F4AFEE"));
        }

        [Test]
        public static void Test16bitDataConverter()
        {
            var bytes = DataConverter.ConvertFromUInt16(16961);

            Assert.That(bytes.ToHex(), Is.EqualTo("4142")); // little endian
            Assert.That(bytes.ConvertToUInt32(), Is.EqualTo(16961));
        }

        [Test]
        public static void Test32bitDataConverter()
        {
            var bytes = DataConverter.ConvertFromUInt32(16961);

            Assert.That(bytes.ToHex(), Is.EqualTo("41420000")); // little endian
            Assert.That(bytes.ConvertToUInt32(), Is.EqualTo(16961)); 
        }

        [Test]
        public static void Test64bitDataConverter()
        {
            var array = ((ulong)812613930).ConvertFromUInt64();

            Assert.That("2A816F3000000000", Is.EqualTo(array.ToHex())); // little endian
        }

        [Test]
        public static void GenerateMosaicId()
        { 
            var decoded = AddressEncoder.DecodeAddress("TATNE7Q5BITMUTRRN6IB4I7FLSDRDWZA37JGO5Q");

            var id = IdGenerator.GenerateId(decoded, 713125680);
            var id2 = IdGenerator.GenerateId(decoded, 729902896);

            Assert.That(DataConverter.ConvertFromUInt64(id).ToHex(), Is.EqualTo("570FB3ED9379624C"));
            Assert.That(DataConverter.ConvertFromUInt64(id2).ToHex(), !Is.EqualTo("570FB3ED9379624C"));
        }

        [Test]
        public static void ConvertUlongToHexId()
        {
            var id = DataConverter.ToHex(DataConverter.ConvertFromUInt64(95442763262823));
        }
    }
}
