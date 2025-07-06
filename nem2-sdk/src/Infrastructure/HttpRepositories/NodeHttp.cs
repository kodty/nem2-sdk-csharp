using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Model2;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class NodeHttp : HttpRouter, INodeRepository
    {
        public NodeHttp(string host, int port) : base(host, port) { }

        public IObservable<NodeHealth> GetNodeHealth()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "health"])))
              .Select(r => { return Composer.GenerateObject<NodeHealth>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<NodeInfo> GetNodeInformation()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "info"])))
              .Select(r => { return Composer.GenerateObject<NodeInfo>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<NodePeer>> GetNodePeers()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "peers"])))
              .Select(r => { return Composer.FilterEvents<NodePeer>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<NodeStorage> GetNodeStorageInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "storage"])))
              .Select(r => { return Composer.GenerateObject<NodeStorage>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<NodeTime> GetNodeTime()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "time"])))
              .Select(r => { return Composer.GenerateObject<NodeTime>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<NodeRESTVersion> GetNodeRESTVersion()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "server"])))
              .Select(r => { return Composer.GenerateObject<NodeRESTVersion>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<NodeUnlockedAccounts> GetNodeHArvestingAccountInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["node", "unlocked"])))
              .Select(r => { return Composer.GenerateObject<NodeUnlockedAccounts>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
