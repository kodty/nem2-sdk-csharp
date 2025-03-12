using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class NodeHttp : HttpRouter, INodeRepository
    {
        public NodeHttp(string host, int port) : base(host, port) { }

        public IObservable<NodeHealth> GetNodeHealth()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "health"])))
              .Select(ObjectComposer.GenerateObject<NodeHealth>);
        }

        public IObservable<NodeInfo> GetNodeInformation()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "info"])))
              .Select(ObjectComposer.GenerateObject<NodeInfo>);
        }

        public IObservable<List<NodePeer>> GetNodePeers()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "peers"])))
              .Select(n => ResponseFilters<NodePeer>.FilterEvents(n));
        }

        public IObservable<NodeStorage> GetNodeStorageInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "storage"])))
              .Select(ObjectComposer.GenerateObject<NodeStorage>);
        }

        public IObservable<NodeTime> GetNodeTime()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "time"])))
              .Select(ObjectComposer.GenerateObject<NodeTime>);
        }

        public IObservable<NodeRESTVersion> GetNodeRESTVersion()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "server"])))
              .Select(ObjectComposer.GenerateObject<NodeRESTVersion>);
        }

        public IObservable<NodeUnlockedAccounts> GetNodeHArvestingAccountInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "unlocked"])))
              .Select(ObjectComposer.GenerateObject<NodeUnlockedAccounts>);
        }
    }
}
