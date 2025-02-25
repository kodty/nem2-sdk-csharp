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
using Newtonsoft.Json;
using System.Reactive.Linq;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System;
using System.Formats.Tar;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.ComponentModel;
using System.Security.Principal;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Collections.Generic;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class AccountHttp : HttpRouter, IAccountRepository
    {
        public AccountHttp(string host, int port) 
            : base(host, port){ }

        public IObservable<List<AccountInfo>> SearchAccounts(QueryModel queryModel)
        {

            return Observable.FromAsync(
                 async ar => await Client.GetStringAsync(GetUri(["accounts"], queryModel)))
                 .Select(ResponseFilters<AccountInfo>.FilterEvents);
        }

        public IObservable<AccountInfo> GetAccount(PublicAccount accountId)
        {
            return GetAccount(accountId.Address);
        }

        public IObservable<AccountInfo> GetAccount(Address accountId)
        {
            return Observable.FromAsync(
                 async ar => await Client.GetStringAsync(GetUri(["accounts",accountId.Plain])))
                 .Select(ObjectComposer.GenerateObject<AccountInfo>);
        }

        [Description("accounts must be exclusively public keys or base32 addresses")]
        public IObservable<List<AccountInfo>> GetAccounts(List<string> accounts)
        {
            var data = new PublicKeys()
            {
                Public_Keys = accounts
            };

            return Observable.FromAsync(
                 async ar => await Client.PostAsync(GetUri(["accounts"]), new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")))
                 .Select(i => {
                
                     return ResponseFilters<AccountInfo>.FilterEvents(i.Content.ReadAsStringAsync().Result);  //  JsonConvert.DeserializeObject<List<AccountInfo>>(i.Content.ReadAsStringAsync().Result);
                 
                 });
        }

        public IObservable<MerkleRoot> GetAccountMerkle(PublicAccount account)
        {
            return GetAccountMerkle(account.Address);
        }

        public IObservable<MerkleRoot> GetAccountMerkle(Address account)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["accounts", account.Plain, "merkle"])))
                 .Select(JsonConvert.DeserializeObject<MerkleRoot>);
        }

        public IObservable<AccountsRestrictions> SearchAccountRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "account"], queryModel)))
               .Select(JsonConvert.DeserializeObject<AccountsRestrictions>);
        }

        public IObservable<ARestrictionData> GetAccountRestriction(string address)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "account", address])))
                .Select(JsonConvert.DeserializeObject<ARestrictionData>);
        }

        public IObservable<MerkleRoot> GetAccountRestrictionsMerkle(string compositeHash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "account", compositeHash, "merkle" ])))
                .Select(JsonConvert.DeserializeObject<MerkleRoot>);
        }
    }
}
