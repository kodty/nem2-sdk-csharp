namespace io.nem2.sdk.src.Model.Transactions.Messages
{
    public abstract class IMessage
    {
        internal abstract byte GetMessageType();

        public abstract byte[] GetPayload();

        public abstract ushort GetLength();
    }
}
