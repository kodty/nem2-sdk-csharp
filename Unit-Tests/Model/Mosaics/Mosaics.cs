using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.src.Export;
using System.Diagnostics;

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
            var bytes = DataConverter.ConvertFromUInt32(16961);
            Assert.That(bytes.ToHexUpper(), Is.EqualTo("41420000")); // little endian
            Assert.That(bytes.ConvertToUInt32(), Is.EqualTo(16961)); 
        }

        [Test]
        public static void Test64bitDataConverter()
        {

            var array = ((ulong)812613930).ConvertToUIntArray();
            var p1 = Convert.ToString(array[0], 16).ToUpper();
            var p2 = array[1] == 0 ? String.Empty : Convert.ToString(array[1], 16).ToUpper();

            Assert.That("2A816F30", Is.EqualTo(p1));
            Assert.That("", Is.EqualTo(p2));

            var result = String.Join("", [p1, p2]);

            Assert.That(result, Is.EqualTo("2A816F30"));
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
