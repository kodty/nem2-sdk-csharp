namespace io.nem2.sdk.src.Infrastructure.Buffers.Model
{
    public class ReceiptDatum
    {
        public string Id { get; set; }

        public Meta Meta { get; set; }

        public TransactionStatement Statement { get; set; }
    }

    public class Meta
    {
        public ulong Timestamp { get; set; }
    }

    public class Receipt
    {
        public ushort Version { get; set; }

        public ushort Type { get; set; }

        public string MosaicId { get; set; }

        public ulong Amount { get; set; }

        public string SenderAddress { get; set; }

        public string RecipientAddress { get; set; }
    }

    public class Source
    {
        public ushort PrimaryId { get; set; }

        public ushort SecondaryId { get; set; }
    }

    public class TransactionStatement
    {
        public ulong Height { get; set; }

        public Source Source { get; set; }

        public List<Receipt> Receipts { get; set; }
    }
  
    public class AddressDatum
    {
        public string Id { get; set; }

        public Meta Meta { get; set; }

        public AddressStatement Statement { get; set; }
    }

    public class ResolutionEntry
    {
        public Source Source { get; set; }

        public string Resolved { get; set; }
    }

    public class AddressStatement
    {
        public ulong Height { get; set; }

        public string Unresolved { get; set; }

        public List<ResolutionEntry> ResolutionEntries { get; set; }
    }

    public class MosaicDatum
    {
        public string Id { get; set; }

        public Meta Meta { get; set; }

        public MosaicStatement Statement { get; set; }
    }

    public class MosaicStatement
    {
        public ulong Height { get; set; }

        public string Unresolved { get; set; }

        public List<ResolutionEntry> ResolutionEntries { get; set; }
    }
}
