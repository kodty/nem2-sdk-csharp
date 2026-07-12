using io.nem2.sdk.Infrastructure.Responses;

namespace io.nem2.sdk.Infrastructure.Interfaces
{
    public interface INetworkNodeRepository
    {
        IObservable<ExtendedHttpResponseMessege<NetworkInfo>> GetNetwork();
        IObservable<ExtendedHttpResponseMessege<NetworkRentalFees>> GetRentalFees();
        IObservable<ExtendedHttpResponseMessege<NetworkTransactionFees>> GetTransactionFees();
        IObservable<ExtendedHttpResponseMessege<NetworkProperties>> GetNetworkProperties();
        IObservable<ExtendedHttpResponseMessege<NodeHealth>> GetNodeHealth();
        IObservable<ExtendedHttpResponseMessege<NodeInfo>> GetNodeInformation();
        IObservable<ExtendedHttpResponseMessege<NodePeer[]>> GetNodePeers();
        IObservable<ExtendedHttpResponseMessege<NodeStorage>> GetNodeStorageInfo();
        IObservable<ExtendedHttpResponseMessege<NodeTime>> GetNodeTime();
        IObservable<ExtendedHttpResponseMessege<NodeRESTVersion>> GetNodeRESTVersion();
        IObservable<ExtendedHttpResponseMessege<NodeUnlockedAccounts>> GetNodeHarvestingAccountInfo();
    }
}
