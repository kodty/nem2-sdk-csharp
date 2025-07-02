using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model.Network;
using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Transactions;
using TweetNaclSharp.Core.Extensions;

namespace io.nem2.sdk.Model.Transactions
{
    public abstract class Transaction
    {
        public string Signature { get; internal set; }

        public PublicAccount Signer { get; internal set; }

        public int Version { get; set; }

        public NetworkType.Types NetworkType { get; internal set; }

        public TransactionTypes.Types TransactionType { get; internal set; }

        public ulong Fee { get; internal set; }

        public Deadline Deadline { get; internal set; }

        //public TransactionInfo TransactionInfo { get; internal set; }

        private byte[] Bytes { get; set; }

        private byte[] Headless { get; set; }

        private byte[] SignedBytes { get; set; }

        internal byte[] GetSigner()
        {
            return Signer == null ? new byte[32] : Signer.PublicKey.FromHex();
        }

        public SignedTransaction SignWith(SecretKeyPair keyPair, byte[] networkGenHash)
        {
            PrepareSignature(keyPair, networkGenHash);

            for (int x = 8; x < 64 + 8; x++) Bytes[x] = Signature.FromHex()[x - 8];

            var hash = HashTransaction(Signature.FromHex(), networkGenHash);

            return SignedTransaction.Create(Bytes, SignedBytes, hash, keyPair.PublicKey, Signature.FromHex(), TransactionType);
        }

        public byte[] PrepareSignature(SecretKeyPair keyPair, byte[] networkGenHash)
        {
            Bytes = GenerateBytes();

            Headless = Bytes.SubArray(8 + 64 + 32 + 4, Bytes.Length - 108); 

            SignedBytes = networkGenHash.Concat(Headless).ToArray();

            Signature = keyPair.Sign(SignedBytes).ToHexLower();
            
            return Signature.FromHex();
        }

        public byte[] HashTransaction(byte[] signature, byte[] genHash)
        {
            var hashBytes = signature.Concat(Signer.PublicKey.FromHex()).Concat(genHash).Concat(Headless.SubArray(32, Headless.Length - 32)).ToArray();

            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);
    
            sha3Hasher.BlockUpdate(hashBytes, 0, hashBytes.Length);

            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }

        internal byte[] ToAggregate()
        {
            var bytes = GenerateBytes();

            var aggregate = bytes.SubArray(4 + 64, 32 + 2 + 2)
                                 .Concat( 
                                        bytes.SubArray(4 + 64 + 32 + 2 + 2 + 8 + 8, bytes.Length - (4 + 64 + 32 + 2 + 2 + 8 + 8))
                                 ).ToArray();

            return BitConverter.GetBytes(aggregate.Length + 4).Concat(aggregate).ToArray();
        }

        public Transaction ToAggregate(PublicAccount signer)
        {
            Signer = PublicAccount.CreateFromPublicKey(signer.PublicKey, NetworkType);

            return this;
        }

        internal abstract byte[] GenerateBytes();
    }
}
