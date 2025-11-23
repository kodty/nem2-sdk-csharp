using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Text.Json;
using io.nem2.sdk.src.Infrastructure.HttpExtension;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        public NamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<List<NamespaceDatum>>> SearchNamespaces(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces"], queryModel)))
                 .Select(r => { return FormListResponse<NamespaceDatum>(r, "data"); });
        }

        public IObservable<ExtendedHttpResponseMessege<NamespaceDatum>> GetNamespace(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId])))
                .Select(FormResponse<NamespaceDatum>);
        }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetNamespaceMerkle(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["namespaces", namespaceId, "merkle"])))
                .Select(FormResponse<MerkleRoot>);
        }

        public IObservable<ExtendedHttpResponseMessege<List<NamespaceName>>> GetNamespacesNames(List<string> namespaceIds)
        {
            var ids = new Namespace_Ids()
            {
                namespaceIds = namespaceIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                .Select(r => { return FormListResponse<NamespaceName>(r); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<AccountName>>> GetAccountNames(List<string> addresses)
        {
            var ids = new Account_Ids()
            {
                addresses = addresses
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "account", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
               .Select(r => { return FormListResponse<AccountName>(r, "accountNames"); });
        }

        public IObservable<ExtendedHttpResponseMessege<List<MosaicName>>> GetMosaicNames(List<string> mosaicIds)
        {
            var ids = new MosaicIds()
            {
                mosaicIds = mosaicIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "mosaic", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                  .Select(r => { return FormListResponse<MosaicName>(r, "accountNames"); });
        }
    }
}
