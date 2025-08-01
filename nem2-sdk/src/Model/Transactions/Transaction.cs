using CopperCurve;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using TweetNaclSharp;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class Transaction
    {
        public Transaction(bool embedded)
        {
            Embedded = embedded;
        }
        public Transaction(TransactionTypes.Types type, bool embedded)
        {
            Embedded = embedded;
            Type = type.GetValue();
        }

        public required EntityBody EntityBody { get; set; }

        public required ushort Type { get; set; }

        private bool Embedded { get; set; }

        public byte[] Fee
        {
            get
            {
                if (Embedded != true)
                {
                    return Fee;
                }
                else return new byte[] { };
            }
            set
            {
                if (Fee != value)
                {
                    Fee = value;
                }
            }
        }

        public byte[] Deadline
        {
            get
            {
                if (Embedded != true)
                {
                    return Deadline;
                }
                else return new byte[] { };
            }
            set
            {
                if (Deadline != value)
                {
                    Deadline = value;
                }
            }
        }
    }
}
