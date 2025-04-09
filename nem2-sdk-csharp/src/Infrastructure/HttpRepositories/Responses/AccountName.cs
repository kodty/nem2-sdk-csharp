
namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses
{
    public class Account_Ids
    {
        public List<string> addresses { get; set; }
    }

    public class AccountName
    {
        public string Address { get; set; }

        public List<string> Names { get; set; }
    }

    public class NamespaceName
    {
        public string ParentId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class MosaicName
    {
        public string MosaicId { get; set; }

        public List<string> Names { get; set; }
    }
}
