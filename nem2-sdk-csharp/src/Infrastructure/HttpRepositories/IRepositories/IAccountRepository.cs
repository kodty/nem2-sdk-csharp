// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="IAccountRepository.cs" company="Nem.io">   
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
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories
{
    interface IAccountRepository
    {
        IObservable<List<AccountInfo>> SearchAccounts(QueryModel queryModel);
        IObservable<AccountInfo> GetAccount(PublicAccount account);
        IObservable<List<AccountInfo>> GetAccounts(List<string> publicKeys);
        IObservable<MerkleRoot> GetAccountMerkle(Address account);
        IObservable<MerkleRoot> GetAccountMerkle(PublicAccount publicAccount);

        // restrictions
        IObservable<AccountsRestrictions> SearchAccountRestrictions(QueryModel queryModel);
        IObservable<ARestrictionData> GetAccountRestriction(string compositeHash);
        IObservable<MerkleRoot> GetAccountRestrictionsMerkle(string compositeHash);
    }
}
