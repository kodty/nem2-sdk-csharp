using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class NodeHttp : HttpRouter, INodeRepository
    {
        public NodeHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<NodeHealth>> GetNodeHealth()
            => HttpGetAsync<NodeHealth>(["node", "health"]);    

        public IObservable<ExtendedHttpResponseMessege<NodeInfo>> GetNodeInformation()
            => HttpGetAsync<NodeInfo>(["node", "info"]);

        public IObservable<ExtendedHttpResponseMessege<NodePeer[]>> GetNodePeers()
            => Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "peers"])))
                    .Select(e => FormResponse(ExtendResponse<NodePeer[]>(e)));  

        public IObservable<ExtendedHttpResponseMessege<NodeStorage>> GetNodeStorageInfo()
            => HttpGetAsync<NodeStorage>(["node", "storage"]);

        public IObservable<ExtendedHttpResponseMessege<NodeTime>> GetNodeTime()
            => HttpGetAsync<NodeTime>(["node", "time"]);

        public IObservable<ExtendedHttpResponseMessege<NodeRESTVersion>> GetNodeRESTVersion()
            => HttpGetAsync<NodeRESTVersion>(["node", "server"]);

        public IObservable<ExtendedHttpResponseMessege<NodeUnlockedAccounts>> GetNodeHarvestingAccountInfo()
            => HttpGetAsync<NodeUnlockedAccounts>(["node", "unlocked"]);
    }
}
