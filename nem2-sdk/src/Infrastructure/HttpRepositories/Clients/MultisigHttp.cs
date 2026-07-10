using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.Clients
{
    public class MultisigHttp : HttpRouter, IMultisigRepository
    {
        public MultisigHttp(string host, int port) : base(host, port) { }

        public IObservable<ExtendedHttpResponseMessege<MerkleRoot>> GetMultisigMerkleInfo(string pubkOrAddress)
            => HttpGetAsync<MerkleRoot>(["accounts", pubkOrAddress, "multisig", "merkle"]);
    }
}
