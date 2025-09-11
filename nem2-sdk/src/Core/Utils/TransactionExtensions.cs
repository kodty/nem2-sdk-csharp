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

        var signingBytes = new byte[32 + body.Length - 32 - 4];

        var genHashBytes = genHash.FromHex();

        for (int x = 0; x < 32; x++)
            signingBytes[x] = genHashBytes[x];

        Array.Copy(body, 32 + 4, signingBytes, 32, body.Length - 32 - 4);

        var VerifiableEntity = new VerifiableEntity()
        {
            Size = transaction.Size + 72,
            VerifiableEntityHeaderReserved = 0,
            Signature = keyPair.Sign(signingBytes)
        };

        var header = Serialize(typeof(VerifiableEntity), VerifiableEntity, false, 72);

        var pl = header.Concat(body).ToArray();

        return new SignedTransaction()
        {
            Payload = pl,
            SignedBytes = signingBytes,
            Signer = keyPair.PublicKeyString,
            Signature = VerifiableEntity.Signature.ToHex(),
            Hash = HashTransaction(pl)
        };
    }

    internal static string HashTransaction(byte[] payload)
    {
        var signature = payload.SubArray(4 + 4, 64);

        var signer = payload.SubArray(4 + 4 + 64, 32);

        var genHash = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4".FromHex();

        var transactionData = payload.SubArray(4 + 4 + 64 + 32, payload.Length - (4 + 4 + 64 + 32));

        var final = signature.Concat(signer).Concat(genHash).Concat(transactionData).ToArray();

        var hash = new byte[32];

        var sha3Hasher = new Sha3Digest(256);

        sha3Hasher.BlockUpdate(final, 0, final.Length);

        sha3Hasher.DoFinal(hash, 0);

        return hash.ToHex();
    }
}