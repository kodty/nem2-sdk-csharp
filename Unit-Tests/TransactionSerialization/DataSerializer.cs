using System.Diagnostics;
using io.nem2.sdk.src.Infrastructure.Buffers.NativeBuffer;

namespace Unit_Tests.TransactionSerialization
{
    internal class DataSerializerTests
    {
        [Test]
        public void Serializer()
        {
            var serializer = new TransactionDataSerializer(8 + 32 + 4 + 2 + 1);

            serializer.WriteUlong(10000000000);
            serializer.WriteHexString("");
            serializer.WriteUint(10000);
            serializer.WriteUshort(100);
            serializer.WriteByte(1);

            Debug.WriteLine(Convert.ToHexString(serializer.Bytes));
        }
    }
}
