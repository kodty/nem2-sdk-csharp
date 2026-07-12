using io.nem2.sdk.Infrastructure.Interfaces;
using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.HttpClients
{
    public class NetworkNodeHttp : HttpRouter, INetworkNodeRepository
    {
        public NetworkNodeHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<NetworkInfo>> GetNetwork()
            => HttpGetAsync<NetworkInfo>(["network"]);

        public IObservable<ExtendedHttpResponseMessege<NetworkRentalFees>> GetRentalFees()
            => HttpGetAsync<NetworkRentalFees>(["network", "fees", "rental"]);

        public IObservable<ExtendedHttpResponseMessege<NetworkTransactionFees>> GetTransactionFees()
            => HttpGetAsync<NetworkTransactionFees>(["network", "fees", "transaction"]);

        public IObservable<ExtendedHttpResponseMessege<NetworkProperties>> GetNetworkProperties()
            => HttpGetAsync<NetworkProperties>(["network", "properties"]);

        public IObservable<ExtendedHttpResponseMessege<NodeHealth>> GetNodeHealth()
            => HttpGetAsync<NodeHealth>(["node", "health"]);    

        public IObservable<ExtendedHttpResponseMessege<NodeInfo>> GetNodeInformation()
            => HttpGetAsync<NodeInfo>(["node", "info"]);

        public IObservable<ExtendedHttpResponseMessege<NodePeer[]>> GetNodePeers()
            => HttpGetAsyncArray<NodePeer>(["node", "peers"]);

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
