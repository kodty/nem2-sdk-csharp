using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface INamespaceRepository
    {
        // Get
        IObservable<ExtendedHttpResponseMessege<List<NamespaceDatum>>> SearchNamespaces(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<NamespaceDatum>> GetNamespace(string namespaceId);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetNamespaceMerkle(string namespaceId);

        // Post
        IObservable<ExtendedHttpResponseMessege<List<NamespaceName>>> GetNamespacesNames(List<string> namespaceIds);
        IObservable<ExtendedHttpResponseMessege<List<AccountName>>> GetAccountNames(List<string> addresses);
        IObservable<ExtendedHttpResponseMessege<List<MosaicName>>> GetMosaicNames(List<string> mosaicIds);
    }
}
