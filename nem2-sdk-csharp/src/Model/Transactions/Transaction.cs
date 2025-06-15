using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
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

        private byte[] SignedBytes { get; set; }

        internal byte[] GetSigner()
        {
            return Signer == null ? new byte[32] : Signer.PublicKey.DecodeHexString();
        }

        public byte[] PrepareSignature(SecretKeyPair keyPair, byte[] networkGenHash)
        {
            Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType);
           
            Bytes = GenerateBytes();

            byte[] signingBytes = Bytes.SubArray(8 + 64 + 32, Bytes.Length - 104);

            ///         Additionally, when signing and verifying transaction data, the network GenerationHashSeed is
            ///         prepended to the data so that transaction signatures will only verify on networks with a matching
            ///         GenerationHashSeed.
            
            SignedBytes = networkGenHash.Concat(signingBytes).ToArray();

            Signature = keyPair.Sign(SignedBytes).ToHexLower();

            return Signature.FromHex();
        }


        public byte[] HashTransaction(byte[] signature, byte[] signer, byte[] genHash, byte[] headlessTxData)
        {
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);

            Debug.WriteLine("signature " + signature.ToHexLower());
            Debug.WriteLine("signer " + signer.ToHexLower());
            Debug.WriteLine("genHash " + genHash.ToHexLower());
            Debug.WriteLine("headless tx data " + headlessTxData.ToHexLower());

            sha3Hasher.BlockUpdate(signature, 0, signature.Length);
            sha3Hasher.BlockUpdate(signer, 0, signer.Length);
            sha3Hasher.BlockUpdate(genHash, 0, genHash.Length);
            sha3Hasher.BlockUpdate(headlessTxData, 0, headlessTxData.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }

        public SignedTransaction SignWith(SecretKeyPair keyPair, byte[] networkGenHash)
        {
            ///       1. Transaction Signature - If this field was not included, an adversary could prevent
            ///       a transaction from being included in the network by frontloading it with a nearly
            ///       identical transaction that contained a malformed signature.
            ///       Page 32 of 116

            PrepareSignature(keyPair, networkGenHash);

            // insert signature into transaction bytes
            for (int x = 8; x < 64 + 8; x++) Bytes[x] = Signature.FromHex()[x - 8];
            Debug.WriteLine("bytes " + Bytes.ToHexLower());
            ///       4. Transaction verifiable data, slice verifiable entity bytes for hashing, repeating SignedBytes 
            var headlessTx = Bytes.SubArray(8 + 64 + 32, Bytes.Length - (8 + 64 + 32));

            ///       2. Transaction SignerPublicKey - If this field was not included, an adversary could
            ///       prevent a transaction from being included in the network by frontloading it with a
            ///       nearly identical transaction that contained a malformed signer public key.
            ///
            ///       3. Network GenerationHashSeed - This field prevents cross network transaction replay
            ///       attacks 13                 
            var hash = HashTransaction(Signature.FromHex(), keyPair.PublicKey, networkGenHash, headlessTx);

            return SignedTransaction.Create(Bytes, SignedBytes, hash, keyPair.PublicKey, Signature.FromHex(), TransactionType);
        }

        internal byte[] ToAggregate()
        {
            var bytes = GenerateBytes();

            var aggregate = bytes.Take(4 + 64, 32 + 2 + 2)
                                 .Concat( 
                                        bytes.Take(4 + 64 + 32 + 2 + 2 + 8 + 8, bytes.Length - (4 + 64 + 32 + 2 + 2 + 8 + 8))
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
