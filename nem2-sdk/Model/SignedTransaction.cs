using io.nem2.sdk.Infrastructure;
using TweetNaclSharp;

namespace io.nem2.sdk.Model
{
    public class SignedTransaction
    {
        public string Hash { get; set; }

        public string Signature { get; set; }

        public byte[] Payload { get; set; }

        public byte[] PayloadSigned { get; set; }

        public string Signer { get; set; }

        public bool IsAggregate { get; set; }

        public bool VerifySignature(byte[] networkGenHash)
        {
            return NaclFast.SignDetachedVerify(networkGenHash.Concat(PayloadSigned).ToArray(), Signature.FromHex(), Signer.FromHex());
        }

        public static bool VerifySignature(byte[] signedBytes, string signature, string signer)
        {
            return NaclFast.SignDetachedVerify(signedBytes, signature.FromHex(), signer.FromHex());
        }
    }
}
