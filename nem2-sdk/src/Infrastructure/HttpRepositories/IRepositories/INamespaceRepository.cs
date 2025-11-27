using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface INamespaceRepository
    {
        // Get
        IObservable<ExtendedHttpResponseMessege<Datum<NamespaceData>>> SearchNamespaces(QueryModel queryModel);
        IObservable<ExtendedHttpResponseMessege<NamespaceData>> GetNamespace(string namespaceId);
        IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetNamespaceMerkle(string namespaceId);

        // Post
        IObservable<ExtendedHttpResponseMessege<List<NamespaceName>>> GetNamespacesNames(List<string> namespaceIds);
        IObservable<ExtendedHttpResponseMessege<Account_Names>> GetAccountNames(List<string> addresses);
        IObservable<ExtendedHttpResponseMessege<Mosaic_Names>> GetMosaicNames(List<string> mosaicIds);
    }
}
