// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="ITransactionRepository.cs" company="Nem.io">   
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

using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.Responses;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    interface ITransactionRepository
    {
        // Get     
        IObservable<List<TransactionData>> SearchConfirmedTransactions(QueryModel queryModel);
        IObservable<List<TransactionData>> SearchUnconfirmedTransactions(QueryModel queryModel);
        IObservable<List<TransactionData>> SearchPartialTransactions(QueryModel queryModel);
        IObservable<TransactionData> GetConfirmedTransaction(string hash);
        IObservable<TransactionData> GetUnconfirmedTransaction(string hash);
        IObservable<TransactionData> GetPartialTransaction(string hash);

        // Post
        IObservable<List<TransactionData>> GetConfirmedTransactions(string[] transactionIds);
        IObservable<List<TransactionData>> GetUnconfirmedTransactions(string[] transactionIds);
        IObservable<List<TransactionData>> GetPartialTransactions(string[] transactionIds);

        // Put
        IObservable<TransactionAnnounceResponse> Announce(SignedTransaction payload);
        IObservable<TransactionAnnounceResponse> AnnounceAggregateTransaction(SignedTransaction payload);
        IObservable<TransactionAnnounceResponse> AnnounceCosignatureTransaction(CosignatureSignedTransaction payload);  
    }

    public class CosignatureSignedTransaction
    {
        public string ParentHash { get; set; }

        public string Signature { get; set; }

        public string Signer { get; set; }

        public int Version { get; set; }
    }
}
