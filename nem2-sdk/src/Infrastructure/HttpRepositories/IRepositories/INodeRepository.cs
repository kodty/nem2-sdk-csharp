using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpExtension;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    public interface INodeRepository
    {
        IObservable<ExtendedHttpResponseMessege<NodeHealth>> GetNodeHealth();
        IObservable<ExtendedHttpResponseMessege<NodeInfo>> GetNodeInformation();
        IObservable<ExtendedHttpResponseMessege<List<NodePeer>>> GetNodePeers();
        IObservable<ExtendedHttpResponseMessege<NodeStorage>> GetNodeStorageInfo();
        IObservable<ExtendedHttpResponseMessege<NodeTime>> GetNodeTime();
        IObservable<ExtendedHttpResponseMessege<NodeRESTVersion>> GetNodeRESTVersion();
        IObservable<ExtendedHttpResponseMessege<NodeUnlockedAccounts>> GetNodeHArvestingAccountInfo();
    }
}
