using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.src.Model2.Transactions.Messages
{
    public class EmptyMessage : IMessage
    {
        private byte Type { get; }

        private EmptyMessage()
        {
            Type = MessageType.Type.UNENCRYPTED.GetValue();          
        }

        public static EmptyMessage Create()
        {
            return new EmptyMessage();
        }

        public override byte[] GetPayload()
        {
            return new byte[]{};
        }

        public override ushort GetLength()
        {
            return 0;
        }

        internal override byte GetMessageType()
        {
            return Type;
        }
    }
}
