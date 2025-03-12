using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class NetworkHttp : HttpRouter, INetworkRepository
    {
        public NetworkHttp(string host, int port) : base(host, port) { }

        public IObservable<NetworkInfo> GetNetwork()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["network"])))
              .Select(ObjectComposer.GenerateObject<NetworkInfo>);
        }

        public IObservable<NetworkRentalFees> GetRentalFees()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["network", "fees", "rental"])))
              .Select(ObjectComposer.GenerateObject<NetworkRentalFees>);
        }

        public IObservable<NetworkTransactionFees> GetTransactionFees()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["network", "fees", "transaction"])))
              .Select(ObjectComposer.GenerateObject<NetworkTransactionFees>);
        }

        public IObservable<NetworkProperties> GetNetworkProperties()
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["network", "properties"])))
             .Select(ObjectComposer.GenerateObject<NetworkProperties>);
        }
    }
}
