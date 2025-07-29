namespace io.nem2.sdk.src.Model2.Transactions
{
    public class AggregateTransaction1 : Transaction1
    {
        public AggregateTransaction1(string transactionsHash, byte[] embeddedTransactions, byte[] cosignatures, TransactionTypes.Types type) : base(type) {
            TransactionsHash = transactionsHash;
            EmbeddedTransactions = embeddedTransactions;
            Cosignatures = cosignatures;
        }

        public string TransactionsHash { get; set; }

        public int TransactionsCount { get; set; }
        public int Aggregate_​transaction_​header_​reserved_​1 { get; set; }
         
        public byte[] EmbeddedTransactions { get; set; }
        public byte[] Cosignatures { get; set; }
    }
}


/*
 * 
 * using System.ComponentModel;
using CopperCurve;
using io.nem2.sdk.src.Model2;
using io.nem2.sdk.src.Model2.Accounts;
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
        }

        private static AggregateTransaction CreateComplete(NetworkType.Types networkType, Deadline deadline, List<Transaction> innerTransactions)
        {
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType, typeof(NetworkType.Types));

            return new AggregateTransaction(networkType, 2, TransactionTypes.Types.AGGREGATE_COMPLETE, deadline, 0, innerTransactions, new List<AggregateTransactionCosignature>());
        }

        private static AggregateTransaction CreateBonded(NetworkType.Types networkType, Deadline deadline, List<Transaction> innerTransactions, List<AggregateTransactionCosignature> cosignatures)
        {
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType, typeof(NetworkType.Types));

           
            return new AggregateTransaction(networkType, 2, TransactionTypes.Types.AGGREGATE_BONDED, deadline, 0, innerTransactions, cosignatures);
        }

        private SignedTransaction SignWithAggregateCosigners(SecretKeyPair initiatorAccount, List<Account> cosignatories, string networkGenHash)
        {
            if (initiatorAccount == null) throw new ArgumentNullException(nameof(initiatorAccount));
            if (cosignatories == null) throw new ArgumentNullException(nameof(cosignatories));

            var signedTransaction = SignWith(initiatorAccount, networkGenHash.FromHex());

            var payload = signedTransaction.Payload.FromHex();

            foreach (var cosignatory in cosignatories)
            {
                var bytes = signedTransaction.Hash.FromHex();

                var signatureBytes = cosignatory.KeyPair.Sign(bytes); 

                payload = payload.Concat(cosignatory.KeyPair.PublicKey.Concat(signatureBytes)).ToArray();

                Cosignatures.Add(new AggregateTransactionCosignature(signatureBytes.ToHex(), new PublicAccount(cosignatory.KeyPair.PublicKey.ToHex(), src.Model2.NetworkType.Types.MIJIN_TEST)));  
            }

            payload = BitConverter.GetBytes(payload.Length).Concat(payload.SubArray(4, payload.Length - 4).ToArray()).ToArray();

            return SignedTransaction.Create(payload, new byte[] { }, signedTransaction.Hash.FromHex(), initiatorAccount.PublicKey, new byte[] { }, TransactionType);
        }

        private bool SignedByAccount(PublicAccount publicAccount)
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

using System.ComponentModel;
using CopperCurve;
using io.nem2.sdk.src.Model2;
using io.nem2.sdk.src.Model2.Accounts;

namespace io.nem2.sdk.Model.Transactions
{
    public class ModifyMultisigAccountTransaction : Transaction
    {
        public int MinRemovalDelta { get; }

        public int MinApprovalDelta { get; }

        public MultisigCosignatoryModification[] Modifications { get; }


        public ModifyMultisigAccountTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, int minApprovalDelta, int minRemovalDelta, List<MultisigCosignatoryModification> modifications)
            : this (networkType, version, deadline, fee, minApprovalDelta, minRemovalDelta, modifications, null, null){}

        public ModifyMultisigAccountTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, int minApprovalDelta, int minRemovalDelta, List<MultisigCosignatoryModification> modifications, string signature, PublicAccount signer)
        {
            if (modifications == null) throw new ArgumentNullException(nameof(modifications));
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType,
                    typeof(NetworkType.Types));
            
            Deadline = deadline;
            NetworkType = networkType;
            Version = version;
            Fee = fee;
            MinRemovalDelta = minRemovalDelta;
            MinApprovalDelta = minApprovalDelta;
            Modifications = modifications.ToArray();
            TransactionType = TransactionTypes.Types.MULTISIG_ACCOUNT_MODIFICATION;
            Signer = signer;
            Signature = signature;
            //TransactionInfo = transactionInfo;
        }

        public static ModifyMultisigAccountTransaction Create(NetworkType.Types networkType, Deadline deadline, int minApprovalDelta, int minRemovalDelta, List<MultisigCosignatoryModification> modifications)
        {
            return new ModifyMultisigAccountTransaction(networkType, 3, deadline, 0, minApprovalDelta, minRemovalDelta, modifications);
        }

        internal override byte[] GenerateBytes()
        {
            ushort size = (ushort)(123 + 33 * Modifications.Length);

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
            serializer.WriteByte((byte)MinRemovalDelta);
            serializer.WriteByte((byte)MinApprovalDelta);
            serializer.WriteByte((byte)Modifications.Length);

            for (var index = 0; index < Modifications.Length; index++)
            {
                serializer.WriteByte(Modifications[index].Type.GetValue());
                serializer.WriteBytes(Modifications[index].PublicAccount.PublicKey.FromHex());
            }

            return serializer.Bytes;
        }
    }
}


*/