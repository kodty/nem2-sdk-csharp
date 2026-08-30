namespace io.nem2.sdk.Model.Transactions
{
    public abstract class IMessage
    {
        public abstract byte[] GetPayload();

        public abstract ushort GetLength();
    }
}
