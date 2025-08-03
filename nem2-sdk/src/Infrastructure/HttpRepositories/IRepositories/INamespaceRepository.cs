using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface INamespaceRepository
    {
        // Get
        IObservable<List<NamespaceDatum>> SearchNamespaces(QueryModel queryModel);
        IObservable<NamespaceDatum> GetNamespace(string namespaceId);
        IObservable<MerkleRoot> GetNamespaceMerkle(string namespaceId);

        // Post
        IObservable<List<NamespaceName>> GetNamespacesNames(List<string> namespaceIds);
        IObservable<List<AccountName>> GetAccountNames(List<string> addresses);
        IObservable<List<MosaicName>> GetMosaicNames(List<string> mosaicIds);
    }
}
