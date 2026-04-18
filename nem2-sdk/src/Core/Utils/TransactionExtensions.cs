using Coppery;
using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Transactions;
using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using TweetNaclSharp;
using TweetNaclSharp.Core.Extensions;

public static class TransactionExtensions
{
    private static byte[] Serialize(Type type, object obj, bool embedded, uint size)
    {
        DataSerializer serializer = new DataSerializer(size);

        serializer.Serialize(type, obj, embedded);

        return serializer.GetBytes();
    }

    internal static UnsignedTransaction PrepareEmbedded(this Transaction transaction, string publicKey)
    {
        transaction.EntityBody.Signer = publicKey.FromHex();

        byte[] body = Serialize(transaction.GetType(), transaction, true, transaction.Size - 8);

        byte[] reserved = new byte[4];

        return new UnsignedTransaction()
        {
            Payload = DataConverter.ConvertFrom(transaction.Size).Concat(reserved).Concat(body).ToArray()
        };
    }

    internal static SignedTransaction PrepareVerified(this Transaction transaction, SecretKeyPair keyPair, string genHash)
    {
        transaction.EntityBody.Signer = keyPair.PublicKey;
        
        var body = Serialize(transaction.GetType(), transaction, false, transaction.Size);

        var unverifiedTransactionData = body.SubArray(32 + 4, body.Length - (32 + 4));

        var genHashBytes = genHash.FromHex();

        var signingBytes = genHashBytes
                                    .Concat(
                                       unverifiedTransactionData
                                     ).ToArray();
       
        var verifiableEntity = new VerifiableEntity()
        {
            Size = transaction.Size + 72,
            VerifiableEntityHeaderReserved = 0,
            Signature = new byte[64]
        };

        Ed25519.crypto_sign2(verifiableEntity.Signature, signingBytes, keyPair.PrivateKey.Concat(keyPair.PublicKey).ToArray(), 32);
        
        var header = Serialize(typeof(VerifiableEntity), verifiableEntity, false, 72);

        return new SignedTransaction()
        {
            Payload = header.Concat(body).ToArray(),
            SignedBytes = signingBytes,
            Signer = keyPair.PublicKeyString,
            Signature = verifiableEntity.Signature.ToHex(),
            Hash = HashTransaction(verifiableEntity.Signature, keyPair.PublicKey, genHashBytes, unverifiedTransactionData)
        };
    }

    public static string HashTransaction(byte[] signature, byte[] signer, byte[] genHash, byte[] unverifiedTransactionData)
    {
        var hash = new byte[32];

        var sha3Hasher = new Sha3Digest(256);

        sha3Hasher.BlockUpdate(signature, 0, signature.Length);
        sha3Hasher.BlockUpdate(signer, 0, signer.Length);
        sha3Hasher.BlockUpdate(genHash, 0, genHash.Length);
        sha3Hasher.BlockUpdate(unverifiedTransactionData, 0, unverifiedTransactionData.Length);
        sha3Hasher.DoFinal(hash, 0);

        return hash.ToHex();
    }
}