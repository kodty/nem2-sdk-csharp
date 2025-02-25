using io.nem2.sdk.src.Infrastructure.Buffers.Model;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface INodeRepository
    {
        IObservable<NodeHealth> GetNodeHealth();
        IObservable<NodeInfo> GetNodeInformation();
        IObservable<List<NodePeer>> GetNodePeers();
        IObservable<NodeStorage> GetNodeStorageInfo();
        IObservable<NodeTime> GetNodeTime();
        IObservable<NodeRESTVersion> GetNodeRESTVersion();
        IObservable<NodeUnlockedAccounts> GetNodeHArvestingAccountInfo();
    }
}
