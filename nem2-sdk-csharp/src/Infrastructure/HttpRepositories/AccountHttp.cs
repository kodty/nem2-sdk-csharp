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
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts"], queryModel)))
                 .Select(r => { return ResponseFilters<AccountData>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<AccountData> GetAccount(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts",pubkOrAddress])))
                 .Select(r => { return ObjectComposer.GenerateObject<AccountData>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<AccountData>> GetAccounts(List<string> accounts) // flag
        {
            var data = new Public_Keys()
            {
                publicKeys = accounts
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["accounts"]), new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")))
                 .Select(r => {  return ResponseFilters<AccountData>.FilterEvents(OverrideEnsureSuccessStatusCode(r)); });
        }


        public IObservable<MerkleRoot> GetAccountMerkle(string pubkOrAddress)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["accounts", pubkOrAddress, "merkle"])))
                 .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<RestrictionData>> SearchAccountRestrictions(QueryModel queryModel) // flag
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account"], queryModel)))
               .Select(r => { return ResponseFilters<RestrictionData>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<RestrictionData> GetAccountRestriction(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account", address])))
                .Select(r => { return ObjectComposer.GenerateObject<RestrictionData>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetAccountRestrictionsMerkle(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "account", address, "merkle" ])))
                .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
