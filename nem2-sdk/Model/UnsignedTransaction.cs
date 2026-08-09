namespace io.nem2.sdk.Model
{
    public class UnsignedTransaction
    {
        public byte[] Payload { get; set; }

        public byte[] VerifiablePayload { get; set; }

        public string Signer { get; set; }

        public bool IsAggregate { get; set; }
    }
}
