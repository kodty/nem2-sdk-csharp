using CopperCurve;
using io.nem2.sdk.src.Model2;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace io.nem2.sdk.src.Model2.Accounts
{

    public class PublicAccount
    {

        public Address Address { get; }

        public byte[] PublicKey { get; }

        public string PublicKeyString => PublicKey.ToHex();

        internal NetworkType.Types NetworkType { get; }

        public PublicAccount(string publicKey, NetworkType.Types networkType)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            if (!Regex.IsMatch(publicKey, @"\A\b[0-9a-fA-F]+\b\Z")) throw  new ArgumentException("invalid public key length");
            if (publicKey.Length != 64) throw new ArgumentException("invalid public key not hex");           
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int) networkType,
                    typeof(NetworkType.Types));

            Address = Address.CreateFromPublicKey(publicKey, networkType);
            PublicKey = publicKey.FromHex();
            NetworkType = networkType;
        }

        public static PublicAccount CreateFromPublicKey(string publicKey, NetworkType.Types networkType)
        {
            return new PublicAccount(publicKey, networkType);
        }
    }
}
