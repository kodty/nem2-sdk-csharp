// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="TransactionHttp.cs" company="Nem.io">
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

using System.Reactive.Linq;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.src.Infrastructure.Buffers.Model.Responses;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Diagnostics;
using System.Text;
using io.nem2.sdk.Infrastructure.Buffers.Model;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class TransactionHttp : HttpRouter, ITransactionRepository
    {
        public TransactionHttp(string host, int port) 
            : base(host, port) {}


        public IObservable<List<TransactionData>> SearchConfirmedTransactions(QueryModel queryModel)
        {          
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["transactions", "confirmed"], queryModel)))
               .Select(t => ResponseFilters<TransactionData>.FilterMany(t, "data"));
        }

        public IObservable<TransactionData> GetConfirmedTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["transactions", "confirmed", hash])))
               .Select(ResponseFilters<TransactionData>.FilterSingle);
        }
        public IObservable<TransactionData> GetUnconfirmedTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["transactions", "unconfirmed", hash])))
               .Select(ResponseFilters<TransactionData>.FilterSingle);
        }

        public IObservable<TransactionData> GetPartialTransaction(string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["transactions", "partial", hash])))
               .Select(ResponseFilters<TransactionData>.FilterSingle);
        }

        public IObservable<List<TransactionData>> SearchUnconfirmedTransactions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["transactions", "unconfirmed"], queryModel)))
               .Select(t => ResponseFilters<TransactionData>.FilterMany(t));
        }

        public IObservable<List<TransactionData>> SearchPartialTransactions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["transactions", "partial"], queryModel)))
               .Select(t => ResponseFilters<TransactionData>.FilterMany(t));
        }

        public IObservable<List<TransactionData>> GetConfirmedTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "confirmed"]), new StringContent(postBody, Encoding.UTF8, "application/json")))    
                .Select(i => {
                    return ResponseFilters<TransactionData>.FilterMany(i.Content.ReadAsStringAsync().Result.ToString()); 
                });
        }

        public IObservable<List<TransactionData>> GetUnconfirmedTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "unconfirmed"]), new StringContent(postBody, Encoding.UTF8, "application/json")))
                .Select(i => ResponseFilters<TransactionData>.FilterMany(i.Content.ToString()));
        }

        public IObservable<List<TransactionData>> GetPartialTransactions(string[] transactionIds)
        {
            var postBody = JsonSerializer.Serialize(new TransactionIdentifiers() { transactionIds = transactionIds });

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["transactions", "partial"]), new StringContent(postBody, Encoding.UTF8, "application/json")))
                .Select(i => ResponseFilters<TransactionData>.FilterMany(i.Content.ToString()));
        }

        public IObservable<TransactionAnnounceResponse> Announce(SignedTransaction signedTransaction)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions"]), new StringContent(signedTransaction.Payload, Encoding.UTF8, "application/json")))
                .Select(i => new TransactionAnnounceResponse() { Message = JObject.Parse(i.Content.ToString())["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> AnnounceAggregateTransaction(SignedTransaction signedTransaction)
        {
            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions", "partial"]), new StringContent(signedTransaction.Payload, Encoding.UTF8, "application/json")))
                .Select(i => new TransactionAnnounceResponse() { Message = JObject.Parse(i.Content.ToString())["message"].ToString() });
        }

        public IObservable<TransactionAnnounceResponse> AnnounceCosignatureTransaction(CosignatureSignedTransaction signedTransaction)
        {
            var postBody = JsonSerializer.Serialize(signedTransaction);

            return Observable.FromAsync(async ar => await Client.PutAsync(GetUri(["transactions", "cosignature"]), new StringContent(postBody)))
                .Select(i => new TransactionAnnounceResponse() { Message = JObject.Parse(i.Content.ToString())["message"].ToString() });
        }
    }
}
