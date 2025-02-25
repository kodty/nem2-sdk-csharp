using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.Buffers.Model;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using Newtonsoft.Json;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class NodeHttp : HttpRouter, INodeRepository
    {
        public NodeHttp(string host, int port) : base(host, port) { }

        public IObservable<NodeHealth> GetNodeHealth()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "health"])))
              .Select(JsonConvert.DeserializeObject<NodeHealth>);
        }

        public IObservable<NodeInfo> GetNodeInformation()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "info"])))
              .Select(JsonConvert.DeserializeObject<NodeInfo>);
        }

        public IObservable<List<NodePeer>> GetNodePeers()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "peers"])))
              .Select(JsonConvert.DeserializeObject<List<NodePeer>>);
        }

        public IObservable<NodeStorage> GetNodeStorageInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "storage"])))
              .Select(JsonConvert.DeserializeObject<NodeStorage>);
        }

        public IObservable<NodeTime> GetNodeTime()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "time"])))
              .Select(JsonConvert.DeserializeObject<NodeTime>);
        }

        public IObservable<NodeRESTVersion> GetNodeRESTVersion()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "server"])))
              .Select(JsonConvert.DeserializeObject<NodeRESTVersion>);
        }

        public IObservable<NodeUnlockedAccounts> GetNodeHArvestingAccountInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["node", "unlocked"])))
              .Select(JsonConvert.DeserializeObject<NodeUnlockedAccounts>);
        }
    }
}
