using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Export;
using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using TweetNaclSharp.Core.Extensions;
using io.nem2.sdk.src.Model2;
using io.nem2.sdk.src.Model2.Transactions;
using System.Text.Json.Nodes;
using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Model2
{
    public static class TransactionExtensions2
    {
        public static Type GetTransactionType(string t, bool embedded = false)
        {
            var type = ((ushort)JsonObject.Parse(t)
                                      .AsObject()["transaction"]["type"]);

            if (type == 16718)
            {
                type += ((ushort)JsonObject.Parse(t)
                                      .AsObject()["transaction"]["registrationType"]);
            }

            return embedded ? type.GetEmbeddedTypeValue() : type.GetTypeValue();
        }

        public static byte[] Serialize<T>(object obj)
        {
            DataSerializer serializer = new DataSerializer();

            serializer.Serialize<T>(obj);

            return serializer.Bytes;
        }

        public static Payload PrepareEmbeddedTransaction<T>(Transaction1 transaction, PublicAccount account)
        {
            transaction.EntityBody.Signer = account.PublicKey;

            var body = Serialize<T>(transaction);

            byte[] size = ((uint)body.Length).ConvertFromUInt32();

            byte[] reserved = new byte[4];

            return new Payload()
            {
                payload = size.Concat(reserved).Concat(body).ToArray()
            };
        }

        public static Payload PrepareTransaction<T>(Transaction1 transaction, SecretKeyPair keyPair)
        {
            transaction.EntityBody.Signer = keyPair.PublicKeyString;
            Debug.WriteLine("key " + keyPair.PublicKeyString);
            var body = Serialize<T>(transaction);

            var genHashBytes = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4".FromHex();
            
            var signingBytes = new byte[body.Length - 32 - 4];

            Array.Copy(body, 32 + 4, signingBytes, 0, body.Length - 32 - 4);

            var VerifiableEntity = new VerifiableEntity()
            {
                Size = (uint)(4 + 4 + 64 + body.Length),
                VerifiableEntityHeaderReserved = 0,
                Signature = keyPair.Sign(genHashBytes.Concat(signingBytes).ToArray())
            };

            var header = Serialize<VerifiableEntity>(VerifiableEntity);

            return new Payload()
            {
                payload = header.Concat(body).ToArray()
            };
        }

        public static string HashTransaction(this Payload payload)
        {
            Debug.WriteLine("payload " + payload.payload.ToHex());
            var signature = payload.payload.SubArray(4 + 4, 64);

            Debug.WriteLine(signature.ToHex());
            var signer = payload.payload.SubArray(4 + 4 + 64, 32);

            Debug.WriteLine(signer.ToHex());
            var genHash = "49D6E1CE276A85B70EAFE52349AACCA389302E7A9754BCF1221E79494FC665A4".FromHex();
            var transactionData = payload.payload.SubArray(4 + 4 + 64 + 32, payload.payload.Length - (4 + 4 + 64 + 32));
            Debug.WriteLine(transactionData.ToHex());

            var final = signature.Concat(signer).Concat(genHash).Concat(transactionData).ToArray();
            Debug.WriteLine(final.ToHex());
            var hash = new byte[32];

            var sha3Hasher = new Sha3Digest(256);
            
            sha3Hasher.BlockUpdate(final, 0, final.Length);

            sha3Hasher.DoFinal(hash, 0);

            return hash.ToHex();
        }
    }
}
