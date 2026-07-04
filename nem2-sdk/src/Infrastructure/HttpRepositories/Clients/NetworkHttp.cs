using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class NetworkHttp : HttpRouter, INetworkRepository
    {
        public NetworkHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<NetworkInfo>> GetNetwork()
            => HttpGetAsync<NetworkInfo>(["network"]);

        public IObservable<ExtendedHttpResponseMessege<NetworkRentalFees>> GetRentalFees()
            => HttpGetAsync<NetworkRentalFees>(["network", "fees", "rental"]);          

        public IObservable<ExtendedHttpResponseMessege<NetworkTransactionFees>> GetTransactionFees()
            => HttpGetAsync<NetworkTransactionFees>(["network", "fees", "transaction"]);           

        public IObservable<ExtendedHttpResponseMessege<NetworkProperties>> GetNetworkProperties()
            => HttpGetAsync<NetworkProperties>(["network", "properties"]);
    }
}
