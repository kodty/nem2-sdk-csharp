// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="NamespaceHttp.cs" company="Nem.io">   
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
using System.Text;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Text.Json;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        public NamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<List<NamespaceDatum>> SearchNamespaces(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["namespaces"], queryModel)))
                .Select(n => ResponseFilters<NamespaceDatum>.FilterEvents(n, "data"));
        }

        public IObservable<NamespaceDatum> GetNamespace(string namespaceId) // flag
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["namespaces", namespaceId])))
                .Select(ObjectComposer.GenerateObject<NamespaceDatum>);
        }

        public IObservable<MerkleRoot> GetNamespaceMerkle(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["namespaces", namespaceId, "merkle"])))
                .Select(ObjectComposer.GenerateObject<MerkleRoot>);
        }

        public IObservable<List<NamespaceName>> GetNamespacesNames(List<string> namespaceIds)
        {
            var ids = new Namespace_Ids()
            {
                namespaceIds = namespaceIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                .Select(i => ResponseFilters<NamespaceName>.FilterEvents(i.Content.ReadAsStringAsync().Result));
        }

        public IObservable<List<AccountName>> GetAccountNames(List<string> addresses)
        {
            var ids = new Account_Ids()
            {
                addresses = addresses
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "account", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
               .Select(i => ResponseFilters<AccountName>.FilterEvents(i.Content.ReadAsStringAsync().Result, "accountNames"));

        }
        public IObservable<List<MosaicName>> GetMosaicNames(List<string> mosaicIds)
        {
            var ids = new MosaicIds()
            {
                mosaicIds = mosaicIds
            };

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "mosaic", "names"]), new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json")))
                 .Select(i => ResponseFilters<MosaicName>.FilterEvents(i.Content.ReadAsStringAsync().Result, "mosaicNames"));
        }
    }
}
