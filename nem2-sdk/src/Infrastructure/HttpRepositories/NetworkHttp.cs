using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Export;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Model2;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class NetworkHttp : HttpRouter, INetworkRepository
    {
        public NetworkHttp(string host, int port) : base(host, port) { }

        public IObservable<NetworkInfo> GetNetwork()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network"])))
              .Select(r => { return Composer.GenerateObject<NetworkInfo>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<NetworkRentalFees> GetRentalFees()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "fees", "rental"])))
              .Select(r => { return Composer.GenerateObject<NetworkRentalFees>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<NetworkTransactionFees> GetTransactionFees()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "fees", "transaction"])))
              .Select(r => { return Composer.GenerateObject<NetworkTransactionFees>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<NetworkProperties> GetNetworkProperties()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "properties"])))
             .Select(r => { return Composer.GenerateObject<NetworkProperties>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
