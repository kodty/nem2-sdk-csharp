using CopperCurve;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Diagnostics;
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

        public EntityBody EntityBody { get; set; }

        public ushort Type { get; set; }

        private bool Embedded { get; set; }

        private byte[] _Fee { get; set; }
        public byte[] Fee
        {
            get
            {
                if (Embedded)
                {
                    return new byte[] { };
                }
                else return _Fee;
            }
            set
            {
                if (Fee != value)
                {
                    _Fee = value;
                }
            }
        }

        private byte[] _Deadline { get; set; }
        public byte[] Deadline
        {
            get
            {
                if (Embedded)
                {
                    return new byte[] { };
                }
                else return _Deadline;
            }
            set
            {
                if (_Deadline != value)
                {
                    _Deadline = value;
                }
            }
        }
    }
}
