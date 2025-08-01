using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using TweetNaclSharp.Core.Extensions;
using io.nem2.sdk.src.Model;
using io.nem2.sdk.src.Model.Transactions;
using System.Text.Json.Nodes;
using CopperCurve;
using io.nem2.sdk.src.Model.Accounts;

namespace io.nem2.sdk.Model
{
    public static class TransactionExtensions
    {
        public static Type GetTransactionType(string t, bool embedded = false)
        {
            var type = (ushort)JsonObject.Parse(t)
                                      .AsObject()["transaction"]["type"];

            if (type == 16718)
            {
                type += (ushort)JsonObject.Parse(t)
                                      .AsObject()["transaction"]["registrationType"];
            }

            return embedded ? type.GetEmbeddedTypeValue() : type.GetTypeValue();
        }

        public static byte[] Serialize<T>(object obj, bool embedded)
        {
            DataSerializer serializer = new DataSerializer();

            serializer.Serialize(typeof(T), obj, embedded);

            return serializer.Bytes;
        }

        public static UnsignedTransaction PrepareEmbeddedTransaction<T>(Transaction transaction, PublicAccount account)
        {
            transaction.EntityBody.Signer = account.PublicKey;

            var body = Serialize<T>(transaction, true);

            byte[] size = (4 + 4 + (uint)body.Length).ConvertFromUInt32();

            byte[] reserved = new byte[4];

            return new UnsignedTransaction()
            {
                Payload = size.Concat(reserved).Concat(body).ToArray()
            };
        }

        public static SignedTransaction PrepareTransaction<T>(Transaction transaction, SecretKeyPair keyPair)
        {
            transaction.EntityBody.Signer = keyPair.PublicKey;

            var body = Serialize<T>(transaction, false);

            var genHashBytes = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4".FromHex();
            
            var signingBytes = new byte[32 + body.Length - 32 - 4];

            Array.Copy(body, 32 + 4, signingBytes, 32, body.Length - 32 - 4);

            for (int x = 0; x < 32; x++)
                signingBytes[x] = genHashBytes[x];

            var sig = keyPair.Sign(signingBytes);

            var VerifiableEntity = new VerifiableEntity()
            {
                Size = (uint)(4 + 4 + 64 + body.Length),
                VerifiableEntityHeaderReserved = 0,
                Signature = sig               
            };

            var header = Serialize<VerifiableEntity>(VerifiableEntity, false);

            var pl = header.Concat(body).ToArray();

            return new SignedTransaction()
            {
                Payload = pl,
                SignedBytes = signingBytes,
                Signer = keyPair.PublicKeyString,
                Signature = sig.ToHex(),
                Hash = HashTransaction(pl)
            };
        }

        public static string HashTransaction(byte[] payload)
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
}
