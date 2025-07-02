using System.ComponentModel;
using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;
using TweetNaclSharp.Core.Extensions;

namespace io.nem2.sdk.Model.Transactions
{
    public class AggregateTransaction : Transaction
    {
        public List<Transaction> InnerTransactions { get; }

        public List<AggregateTransactionCosignature> Cosignatures { get; }

        public AggregateTransaction(NetworkType.Types networkType, int version, TransactionTypes.Types transactionType, Deadline deadline, ulong fee,  List<Transaction> innerTransactions, List<AggregateTransactionCosignature> cosignatures)
         : this(networkType, version, transactionType, deadline, fee,innerTransactions, cosignatures, null, null){

        }

        public AggregateTransaction(NetworkType.Types networkType, int version, TransactionTypes.Types transactionType, Deadline deadline, ulong fee, List<Transaction> innerTransactions, List<AggregateTransactionCosignature> cosignatures, string signature, PublicAccount signer)
        {
            InnerTransactions = innerTransactions;
            Cosignatures = cosignatures;
            Deadline = deadline;
            NetworkType = networkType;
            Fee = fee;
            TransactionType = transactionType;
            Version = version;
            Signature = signature;
            Signer = signer;
            //TransactionInfo = transactionInfo;
        }

        public static AggregateTransaction CreateComplete(NetworkType.Types networkType, Deadline deadline, List<Transaction> innerTransactions)
        {
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType, typeof(NetworkType.Types));

            return new AggregateTransaction(networkType, 2, TransactionTypes.Types.AGGREGATE_COMPLETE, deadline, 0, innerTransactions, new List<AggregateTransactionCosignature>());
        }

        public static AggregateTransaction CreateBonded(NetworkType.Types networkType, Deadline deadline, List<Transaction> innerTransactions, List<AggregateTransactionCosignature> cosignatures)
        {
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType, typeof(NetworkType.Types));

           
            return new AggregateTransaction(networkType, 2, TransactionTypes.Types.AGGREGATE_BONDED, deadline, 0, innerTransactions, cosignatures);
        }

        public SignedTransaction SignWithAggregateCosigners(SecretKeyPair initiatorAccount, List<Account> cosignatories, string networkGenHash)
        {
            if (initiatorAccount == null) throw new ArgumentNullException(nameof(initiatorAccount));
            if (cosignatories == null) throw new ArgumentNullException(nameof(cosignatories));

            var signedTransaction = SignWith(initiatorAccount, networkGenHash.FromHex());

            var payload = signedTransaction.Payload.FromHex();

            foreach (var cosignatory in cosignatories)
            {
                var bytes = signedTransaction.Hash.FromHex();

                var signatureBytes = TransactionExtensions.SignHash(cosignatory.KeyPair, bytes);

                payload = payload.Concat(cosignatory.KeyPair.PublicKey.Concat(signatureBytes)).ToArray();

                Cosignatures.Add(new AggregateTransactionCosignature(signatureBytes.ToHexLower(), new PublicAccount(cosignatory.KeyPair.PublicKey.ToHexLower(), src.Model.Network.NetworkType.Types.MIJIN_TEST)));  
            }

            payload = BitConverter.GetBytes(payload.Length).Concat(payload.SubArray(4, payload.Length - 4).ToArray()).ToArray();

            return SignedTransaction.Create(payload, new byte[] { }, signedTransaction.Hash.FromHex(), initiatorAccount.PublicKey, new byte[] { }, TransactionType);
        }

        public bool SignedByAccount(PublicAccount publicAccount)
        {
            if (publicAccount == null) throw new ArgumentNullException(nameof(publicAccount));

            return Signer.PublicKey == publicAccount.PublicKey || Cosignatures.Any(e => e.Signer.PublicKey == publicAccount.PublicKey);
        }

        internal override byte[] GenerateBytes()
        {
            var transactionsBytes = new byte[0];

            transactionsBytes = InnerTransactions.Aggregate(transactionsBytes, (current, innerTransaction) => current.Concat(innerTransaction.ToAggregate()).ToArray());

            ushort size = (ushort)(120 + 4 + transactionsBytes.Length);

            var serializer = new DataSerializer();

            serializer.WriteUlong(size);

            serializer.Reserve(64);
            serializer.WriteBytes(GetSigner());
            serializer.Reserve(4);
            serializer.WriteByte((byte)Version);
            serializer.WriteByte(NetworkType.GetNetworkByte());
            serializer.WriteUShort(TransactionType.GetValue());
            serializer.WriteUlong(Fee);
            serializer.WriteUlong(Deadline.Ticks);
            serializer.WriteUInt((uint)transactionsBytes.Length);
            serializer.Reserve(4);
            serializer.WriteBytes(transactionsBytes);

            return serializer.Bytes;
        }
    }
}
