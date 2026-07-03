using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class NetworkHttp : HttpRouter, INetworkRepository
    {
        public NetworkHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<NetworkInfo>> GetNetwork()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network"])))
              .Select(e => FormResponse(ExtendResponse<NetworkInfo>(e)));
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkRentalFees>> GetRentalFees()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "fees", "rental"])))
              .Select(e => FormResponse(ExtendResponse<NetworkRentalFees>(e)));
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkTransactionFees>> GetTransactionFees()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "fees", "transaction"])))
              .Select(e => FormResponse(ExtendResponse<NetworkTransactionFees>(e)));
        }

        public IObservable<ExtendedHttpResponseMessege<NetworkProperties>> GetNetworkProperties()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["network", "properties"])))
             .Select(e => FormResponse(ExtendResponse<NetworkProperties>(e)));
        }
    }
}
