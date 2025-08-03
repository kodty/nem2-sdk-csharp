using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface INetworkRepository
    {
        IObservable<NetworkInfo> GetNetwork();
        IObservable<NetworkRentalFees> GetRentalFees();
        IObservable<NetworkTransactionFees> GetTransactionFees();
        IObservable<NetworkProperties> GetNetworkProperties();
    }
}
