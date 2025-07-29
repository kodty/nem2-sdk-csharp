using System.Text.RegularExpressions;
using io.nem2.sdk.src.Model.Accounts;

namespace io.nem2.sdk.src.Model.Transactions
{
    public class MultisigCosignatoryModification
    {
        internal MultisigCosignatoryModificationType.Type Type { get; }

        public PublicAccount PublicAccount { get; }

        public bool IsAddition => Type == MultisigCosignatoryModificationType.Type.Add;

        public bool IsRemoval => Type == MultisigCosignatoryModificationType.Type.Remove;

        public MultisigCosignatoryModification(MultisigCosignatoryModificationType.Type type, PublicAccount publicAccount)
        {
            if (type != MultisigCosignatoryModificationType.Type.Add && type != MultisigCosignatoryModificationType.Type.Remove) throw new ArgumentOutOfRangeException(nameof(type));
            if (publicAccount == null) throw new ArgumentNullException(nameof(publicAccount));
            if (!Regex.IsMatch(publicAccount.PublicKeyString, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("invalid public key length");
            if (publicAccount.PublicKey.Length != 64) throw new ArgumentException("invalid public key not hex");           

            Type = type;
            PublicAccount = publicAccount;
        }

        public static MultisigCosignatoryModification Create(MultisigCosignatoryModificationType.Type type, PublicAccount publicKey)
        {
            return new MultisigCosignatoryModification(type, publicKey);
        }
    }
}
