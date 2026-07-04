namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface INodeRepository
    {
        IObservable<ExtendedHttpResponseMessege<NodeHealth>> GetNodeHealth();
        IObservable<ExtendedHttpResponseMessege<NodeInfo>> GetNodeInformation();
        IObservable<ExtendedHttpResponseMessege<NodePeer[]>> GetNodePeers();
        IObservable<ExtendedHttpResponseMessege<NodeStorage>> GetNodeStorageInfo();
        IObservable<ExtendedHttpResponseMessege<NodeTime>> GetNodeTime();
        IObservable<ExtendedHttpResponseMessege<NodeRESTVersion>> GetNodeRESTVersion();
        IObservable<ExtendedHttpResponseMessege<NodeUnlockedAccounts>> GetNodeHarvestingAccountInfo();
    }
}
