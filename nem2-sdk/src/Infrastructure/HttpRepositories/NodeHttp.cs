using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class NodeHttp : HttpRouter, INodeRepository
    {
        public NodeHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<NodeHealth>> GetNodeHealth()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "health"])))
              .Select(FormResponse<NodeHealth>);
        }

        public IObservable<ExtendedHttpResponseMessege<NodeInfo>> GetNodeInformation()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "info"])))
              .Select(FormResponse<NodeInfo>);
        }

        public IObservable<ExtendedHttpResponseMessege<List<NodePeer>>> GetNodePeers()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "peers"])))
              .Select(r => { return FormListResponse<NodePeer>(r); });
        }

        public IObservable<ExtendedHttpResponseMessege<NodeStorage>> GetNodeStorageInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "storage"])))
              .Select(FormResponse<NodeStorage>);
        }

        public IObservable<ExtendedHttpResponseMessege<NodeTime>> GetNodeTime()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "time"])))
              .Select(FormResponse<NodeTime>);
        }

        public IObservable<ExtendedHttpResponseMessege<NodeRESTVersion>> GetNodeRESTVersion()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "server"])))
              .Select(FormResponse<NodeRESTVersion>);
        }

        public IObservable<ExtendedHttpResponseMessege<NodeUnlockedAccounts>> GetNodeHArvestingAccountInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "unlocked"])))
              .Select(FormResponse<NodeUnlockedAccounts>);
        }
    }
}
