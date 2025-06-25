using System.ComponentModel;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Model.Network;

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
