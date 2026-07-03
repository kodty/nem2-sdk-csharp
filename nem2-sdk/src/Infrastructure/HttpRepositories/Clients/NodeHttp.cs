using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using System.Reactive.Linq;


namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class NodeHttp : HttpRouter, INodeRepository
    {
        public NodeHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<NodeHealth>> GetNodeHealth()
        {
            return HttpGetAsync<NodeHealth>(["node", "health"]); 
        }

        public IObservable<ExtendedHttpResponseMessege<NodeInfo>> GetNodeInformation()
        {
            return HttpGetAsync<NodeInfo>(["node", "info"]);
        }

        public IObservable<ExtendedHttpResponseMessege<NodePeer[]>> GetNodePeers()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "peers"])))
                 .Select(e => FormResponse(ExtendResponse<NodePeer[]>(e)));
        }

        public IObservable<ExtendedHttpResponseMessege<NodeStorage>> GetNodeStorageInfo()
        {
            return HttpGetAsync<NodeStorage>(["node", "storage"]);
        }

        public IObservable<ExtendedHttpResponseMessege<NodeTime>> GetNodeTime()
        {
            return HttpGetAsync<NodeTime>(["node", "time"]);
        }

        public IObservable<ExtendedHttpResponseMessege<NodeRESTVersion>> GetNodeRESTVersion()
        {
            return HttpGetAsync<NodeRESTVersion>(["node", "server"]);
        }

        public IObservable<ExtendedHttpResponseMessege<NodeUnlockedAccounts>> GetNodeHArvestingAccountInfo()
        {
            return HttpGetAsync<NodeUnlockedAccounts>(["node", "unlocked"]);
        }
    }
}
