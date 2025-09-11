namespace io.nem2.sdk.src.Model.Transactions
{
    public class Transaction
    {
        internal uint Size { get; set; }

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
                if (_Fee != value && !Embedded)
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
                if (_Deadline != value && !Embedded)
                {
                    _Deadline = value;
                }
            }
        }
        public Transaction(bool embedded)
        {
            Embedded = embedded;

            Size += 48;
            if (!embedded)
                Size += 8;
        }

        public Transaction(TransactionTypes.Types type, bool embedded)
        {
            Embedded = embedded;
            Type = type.GetValue();

            Size += 48;
            if (!embedded)
                Size += 8;
        }

        public UnsignedTransaction Embed(string account)
        {
            return TransactionExtensions.PrepareEmbedded(this, account);
        }

        public SignedTransaction WrapVerified(SecretKeyPair account, string genHash)
        {
            return TransactionExtensions.PrepareVerified(this, account, genHash);
        }   
    }
}
