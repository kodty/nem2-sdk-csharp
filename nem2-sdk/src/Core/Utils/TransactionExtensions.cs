using Coppery;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Transactions;
using Org.BouncyCastle.Crypto.Digests;
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
            Signature = keyPair.Sign(signingBytes)
        };

        var header = Serialize(typeof(VerifiableEntity), verifiableEntity, false, 72);

        var pl = header.Concat(body).ToArray();

        return new SignedTransaction()
        {
            Payload = pl,
            SignedBytes = signingBytes,
            Signer = keyPair.PublicKeyString,
            Signature = verifiableEntity.Signature.ToHex(),
            Hash = HashTransaction(verifiableEntity.Signature, keyPair.PublicKey, genHashBytes, unverifiedTransactionData)
        };
    }

    private static byte[] ConcatTransactionData(byte[] signature, byte[] signer, byte[] genHash, byte[] unverifiedTransactionData)
    {
        return signature.Concat(signer).Concat(genHash).Concat(unverifiedTransactionData).ToArray();
    }

    internal static string HashTransaction(byte[] signature, byte[] signer, byte[] genHash, byte[] unverifiedTransactionData)
    {
        var final = ConcatTransactionData(signature, signer, genHash, unverifiedTransactionData);

        var hash = new byte[32];

        var sha3Hasher = new Sha3Digest(256);

        sha3Hasher.BlockUpdate(final, 0, final.Length);

        sha3Hasher.DoFinal(hash, 0);

        return hash.ToHex();
    }
}