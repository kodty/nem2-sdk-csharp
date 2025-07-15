namespace io.nem2.sdk.src.Model2.Transactions
{
    public class Transaction1
    {
        public Transaction1()
        {

        }
        public Transaction1(TransactionTypes.Types type)
        {
            Type = type.GetValue();
        }

        public EntityBody EntityBody { get; set; }

        public ushort Type { get; set; }

        public ulong Fee { get; set; }

        public ulong Deadline { get; set; }
    }
}
