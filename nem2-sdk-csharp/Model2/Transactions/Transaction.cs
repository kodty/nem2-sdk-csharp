using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Model2.Transactions
{
    public class Transaction1
    {
        public Transaction1()
        {

        }
        public Transaction1(TransactionTypes.Types type)
        {
            Type = type;
        }

        public EntityBody EntityBody { get; set; }

        public TransactionTypes.Types Type { get; set; }

        public ulong Fee { get; set; }

        public Deadline Deadline { get; set; }
    }
}
