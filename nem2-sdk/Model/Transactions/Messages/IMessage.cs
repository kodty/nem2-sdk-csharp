namespace io.nem2.sdk.Model.Transactions.Messages
{
    public abstract class IMessage
    {
        internal abstract byte GetMessageType();

        internal abstract byte GetEncodingType();

        public abstract byte[] GetPayload();

        public abstract ushort GetLength();
    }
}
