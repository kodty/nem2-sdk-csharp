using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Reactive.Linq;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories
{
    public class MultisigHttp : HttpRouter, IMultisigRepository
    {
        public MultisigHttp(string host, int port) : base(host, port) { }

        public IObservable<MerkleRoot> GetMultisigMerkleInfo(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(Host + ":" + Port + "/accounts/" + pubkOrAddress + "/multisig/merkle"))
                 .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }

        /*
        public IObservable<MultisigAccountInfo> GetMultisigAccountInfo(PublicAccount account)
        {
            return GetMultisigAccountInfo(account.Address);
        }*/
        /*
        public IObservable<MultisigAccountInfo> GetMultisigAccountInfo(Address account)
        {
            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkType().Take(1);

            return Observable.FromAsync(async ar => await Client.GetStringAsync(Host + ":" + Port + "/account/" + account.Plain + "/multisig"))
                    .Select(JsonConvert.DeserializeObject<MultisigEntry>)
                    .Select(entry => new MultisigAccountInfo(
                            new PublicAccount(entry.Multisig.Account, networkTypeResolve.Wait()),
                            entry.Multisig.MinApproval,
                            entry.Multisig.MinRemoval,
                            entry.Multisig.Cosignatories
                                .Select(cosig => new PublicAccount(cosig, networkTypeResolve.Wait())).ToList(),
                            entry.Multisig.MultisigAccounts
                                .Select(musig => new PublicAccount(musig, networkTypeResolve.Wait())).ToList()
                    ));
        }*/
        /*
        public IObservable<MultisigAccountGraphInfo> GetMultisigAccountGraphInfo(PublicAccount account)
        {
            return GetMultisigAccountGraphInfo(account.Address);
        }
        */
  
        /*
        public IObservable<MultisigAccountGraphInfo> GetMultisigAccountGraphInfo(Address account)
        {

            return Observable.FromAsync(async ar => await Client.GetStringAsync(Host + ":" + Port + "/account/" + account.Plain + "/multisig/graph"))
                     .Select(JsonConvert.DeserializeObject<List<MultisigAccountGraphInfoDTO>>)
                     .Select(entry =>
                     {
                         Dictionary<int, List<MultisigAccountInfo>> graphInfoMap = new Dictionary<int, List<MultisigAccountInfo>>();

                         entry.ForEach(item => graphInfoMap.Add(
                               item.Level,
                               item.MultisigEntries
                                   .Select(entry => new MultisigAccountInfo(PublicAccount.CreateFromPublicKey(entry.Multisig.Account, GetNetworkType().Wait()),
                                       entry.Multisig.MinApproval,
                                       entry.Multisig.MinRemoval,
                                       entry.Multisig.Cosignatories
                                           .Select(cosig => PublicAccount.CreateFromPublicKey(cosig, GetNetworkType().Wait())).ToList(),
                                       entry.Multisig.MultisigAccounts
                                           .Select(musig => PublicAccount.CreateFromPublicKey(musig, GetNetworkType().Wait())).ToList())
                               ).ToList()));

                         return new MultisigAccountGraphInfo(graphInfoMap);
                     });
        }
        */
    }
}
