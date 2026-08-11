using System.Text;

namespace io.nem2.sdk.Model.Transactions.Messages
{
    public class PlainMessage : IMessage
    {
        private byte Type { get; }

        private byte[] Payload { get; }

        private PlainMessage(byte[] payload)
        {
            Type = MessageType.Type.UNENCRYPTED.GetValue();
            Payload = payload;
        }

        public static PlainMessage Create(string payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            return new PlainMessage(Encoding.UTF8.GetBytes(payload));
        }

        public override byte[] GetPayload()
        {
            return [.. new byte[] {Type}, .. Payload];
        }

        public override ushort GetLength()
        {
            return (ushort)(Payload.Length + 1);
        }

        internal bool IsEncrypted()
        {
            return Type == 0;     
        }

        public string GetStringPayload()
        {
            return Encoding.UTF8.GetString([.. new byte[] { Type }, .. Payload]);
        }
    }
}
