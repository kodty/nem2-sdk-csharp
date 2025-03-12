
namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{

    public class MosaicIds
    {
        public List<string> mosaicIds { get; set; }
    }
    public class MosaicTransfer
    {
        public string Id { get; set; }

        public ulong Amount { get; set; }
    }

    public class MosaicEvent
    {
        public Mosaic Mosaic { get; set; }

        public string Id { get; set; }
    }

    public class Mosaic
    {
        public int Version { get; set; }

        public string Id { get; set; }

        public ulong Supply { get; set; }

        public ulong StartHeight { get; set; }

        public string OwnerAddress { get; set; }

        public int Revision { get; set; }

        public int Flags { get; set; }

        public int Divisibility { get; set; }

        public int Duration { get; set; }
    }
}
