using CopperCurve;
using io.nem2.sdk.src.Model.Accounts;
using Org.BouncyCastle.Crypto.Digests;
using TweetNaclSharp.Core.Extensions;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class Transaction
    {
        public UnsignedTransaction Embed(PublicAccount account)
        {
            return PrepareEmbeddedTransaction(GetType(), account);
        }

        public SignedTransaction WrapVerified(SecretKeyPair account)
        {
            return PrepareTransaction(GetType(), account);
        }

        internal byte[] Serialize(Type type, object obj, bool embedded)
        {
            DataSerializer serializer = new DataSerializer();

            serializer.Serialize(type, obj, embedded);

            return serializer.Bytes;
        }

        internal UnsignedTransaction PrepareEmbeddedTransaction(Type type, PublicAccount account)
        {
            this.EntityBody.Signer = account.PublicKey;

            byte[] body = Serialize(type, this, true);

            byte[] reserved = new byte[4];

            return new UnsignedTransaction()
            {
                Payload = Size.ConvertFromUInt32().Concat(reserved).Concat(body).ToArray()
            };
        }

        internal SignedTransaction PrepareTransaction(Type type, SecretKeyPair keyPair)
        {
            this.EntityBody.Signer = keyPair.PublicKey;

            var body = Serialize(type, this, false);

            var genHashBytes = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4".FromHex();

            var signingBytes = new byte[32 + body.Length - 32 - 4];

            Array.Copy(body, 32 + 4, signingBytes, 32, body.Length - 32 - 4);

            for (int x = 0; x < 32; x++)
                signingBytes[x] = genHashBytes[x];

            var sig = keyPair.Sign(signingBytes);

            var VerifiableEntity = new VerifiableEntity()
            {
                Size = Size,
                VerifiableEntityHeaderReserved = 0,
                Signature = sig
            };

            var header = Serialize(typeof(VerifiableEntity), VerifiableEntity, false);

            var pl = header.Concat(body).ToArray();

            return new SignedTransaction()
            {
                Payload = pl,
                SignedBytes = signingBytes,
                Signer = keyPair.PublicKeyString,
                Signature = sig.ToHex(),
                Hash = HashTransaction(pl)
            };
        }

        internal string HashTransaction(byte[] payload)
        {
            var signature = payload.SubArray(4 + 4, 64);

            var signer = payload.SubArray(4 + 4 + 64, 32);

            var genHash = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4".FromHex();

            var transactionData = payload.SubArray(4 + 4 + 64 + 32, payload.Length - (4 + 4 + 64 + 32));

            var final = signature.Concat(signer).Concat(genHash).Concat(transactionData).ToArray();

            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);

            sha3Hasher.BlockUpdate(final, 0, final.Length);

            sha3Hasher.DoFinal(hash, 0);

            return hash.ToHex();
        }

        public Transaction(bool embedded)
        {
            Embedded = embedded;

            Size += 48;
            if (!embedded)
                Size += 80;
        }

        public Transaction(TransactionTypes.Types type, bool embedded)
        {
            Embedded = embedded;
            Type = type.GetValue();

            Size += 48;
            if (!embedded)
                Size += 80;
        }

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
    }
}
