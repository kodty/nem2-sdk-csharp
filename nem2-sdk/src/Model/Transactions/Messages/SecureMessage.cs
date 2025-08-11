using Coppery;
using io.nem2.sdk.Core.Crypto;

namespace io.nem2.sdk.src.Model.Transactions.Messages
{
    public class SecureMessage : IMessage
    {
        private byte Type { get; }

        private byte[] Payload { get; }

        public SecureMessage(byte[] payload)
        {
            Type = MessageType.Type.ENCRYPTED.GetValue();
            Payload = payload;
        }
        public static SecureMessage Create(string msg, string senderPrivateKey, string receiverPublicKey)
        {
            return new SecureMessage(CryptoUtils.Encode(msg, senderPrivateKey, receiverPublicKey).FromHex());
        }

        public string GetDecodedPayload(string privateKey, string publicKey)
        {
            return CryptoUtils.Decode(Payload, privateKey, publicKey);
        }

        internal override byte GetMessageType()
        {
            return Type;
        }

        public override byte[] GetPayload()
        {
            return Payload;
        }

        public override ushort GetLength()
        {
            return (ushort)Payload.Length;
        }     
    }
}
