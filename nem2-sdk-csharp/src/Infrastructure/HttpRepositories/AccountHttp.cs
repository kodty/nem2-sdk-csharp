// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 30/12/2024
// ***********************************************************************
// <copyright file="AccountHttp.cs" company="Nem.io">
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************

using io.nem2.sdk.Model.Accounts;
using System.Reactive.Linq;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Text;
using System.Text.Json;


namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class AccountHttp : HttpRouter, IAccountRepository
    {
        public AccountHttp(string host, int port) 
            : base(host, port){ }

        public IObservable<List<AccountData>> SearchAccounts(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["accounts"], queryModel)))
                 .Select(a => ResponseFilters<AccountData>.FilterEvents(a, "data"));
        }

        public IObservable<string> SearchAccountsString(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["accounts"], queryModel)));
        }

        public IObservable<AccountData> GetAccount(PublicAccount accountId)
        {
            return GetAccount(accountId.Address);
        }

        public IObservable<AccountData> GetAccount(Address accountId)
        {
            return Observable.FromAsync(
                 async ar => await Client.GetStringAsync(GetUri(["accounts",accountId.Plain])))
                 .Select(ObjectComposer.GenerateObject<AccountData>);
        }

        public IObservable<List<AccountData>> GetAccounts(List<string> accounts) // flag
        {
            var data = new Public_Keys()
            {
                publicKeys = accounts
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["accounts"]), new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")))
                 .Select(i => {
                     return ResponseFilters<AccountData>.FilterEvents(i.Content.ReadAsStringAsync().Result);                 
                 });
        }

        public IObservable<MerkleRoot> GetAccountMerkle(PublicAccount account)
        {
            return GetAccountMerkle(account.Address);
        }

        public IObservable<MerkleRoot> GetAccountMerkle(Address account)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["accounts", account.Plain, "merkle"])))
                 .Select(ObjectComposer.GenerateObject<MerkleRoot>);
        }

        public IObservable<List<RestrictionData>> SearchAccountRestrictions(QueryModel queryModel) // flag
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "account"], queryModel)))
               .Select(a => ResponseFilters<RestrictionData>.FilterEvents(a, "data"));
        }

        public IObservable<RestrictionData> GetAccountRestriction(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "account", address])))
                .Select(ObjectComposer.GenerateObject<RestrictionData>);
        }

        public IObservable<MerkleRoot> GetAccountRestrictionsMerkle(string compositeHash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "account", compositeHash, "merkle" ])))
                .Select(ObjectComposer.GenerateObject<MerkleRoot>);
        }
    }
}
