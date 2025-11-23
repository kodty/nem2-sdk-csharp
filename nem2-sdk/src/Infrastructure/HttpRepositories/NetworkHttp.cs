using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpExtension;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class NetworkHttp : HttpRouter, INetworkRepository
    {
        public NetworkHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<NetworkInfo>> GetNetwork()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network"])))
              .Select(FormResponse<NetworkInfo>);
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkRentalFees>> GetRentalFees()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "fees", "rental"])))
              .Select(FormResponse<NetworkRentalFees>);
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkTransactionFees>> GetTransactionFees()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "fees", "transaction"])))
              .Select(FormResponse<NetworkTransactionFees>);
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkProperties>> GetNetworkProperties()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "properties"])))
             .Select(FormResponse<NetworkProperties>);
        }
    }
}
