using TweetNaclSharp;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class Transaction
    {
        public Transaction()
        {

        }
        public Transaction(TransactionTypes.Types type)
        {
            Type = type.GetValue();
        }

        public EntityBody EntityBody { get; set; }

        public ushort Type { get; set; }

        public ulong Fee { get; set; }

        public ulong Deadline { get; set; }

    }
}
