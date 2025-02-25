using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using Newtonsoft.Json;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class NetworkHttp : HttpRouter, INetworkRepository
    {
        public NetworkHttp(string host, int port) : base(host, port) { }

        public IObservable<NetworkInfo> GetNetwork()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["network"])))
              .Select(JsonConvert.DeserializeObject<NetworkInfo>);
        }

        public IObservable<NetworkRentalFees> GetRentalFees()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["network", "fees", "rental"])))
              .Select(JsonConvert.DeserializeObject<NetworkRentalFees>);
        }

        public IObservable<NetworkTransactionFees> GetTransactionFees()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["network", "fees", "transaction"])))
              .Select(JsonConvert.DeserializeObject<NetworkTransactionFees>);
        }

        public IObservable<NetworkProperties> GetNetworkProperties()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["network", "properties"])))
             .Select(ObjectComposer.GenerateObject<NetworkProperties>);
        }
    }
}
