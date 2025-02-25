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

using System.Diagnostics;
using System.Reactive.Linq;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using Newtonsoft.Json;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        public NamespaceHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<Namespaces> SearchNamespaces(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["namespaces"], queryModel)))
                .Select(JsonConvert.DeserializeObject<Namespaces>);
        }

        public IObservable<NamespaceDatum> GetNamespace(string namespaceId)
        {
            //if (string.IsNullOrEmpty(namespaceId.Name)) throw new ArgumentException("Value cannot be null or empty.", nameof(namespaceId));
            //if (namespaceId.HexId.Length != 16 || !Regex.IsMatch(namespaceId.HexId, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("Invalid namespace");

            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["namespaces", namespaceId])))
                .Select(JsonConvert.DeserializeObject<NamespaceDatum>);
        }

        public IObservable<MerkleRoot> GetNamespaceMerkle(string namespaceId)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["namespaces", namespaceId, "merkle"])))
                .Select(JsonConvert.DeserializeObject<MerkleRoot>);
        }

        public IObservable<List<NamespaceName>> GetNamespacesNames(List<string> namespaceIds)
        {
            var postBody = "{ namespaceids: " + JsonConvert.SerializeObject(namespaceIds) + "}";

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "names"]), new StringContent(postBody)))
                .Select(i => JsonConvert.DeserializeObject<List<NamespaceName>>(i.Content.ToString()));
        }

        public IObservable<List<AccountName>> GetAccountNames(List<string> addresses)
        {
            var postBody = "{ addresses: " + JsonConvert.SerializeObject(addresses) + "}";

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "account", "names"]), new StringContent(postBody)))
                .Select(i => JsonConvert.DeserializeObject<List<AccountName>>(i.Content.ToString()));

        }
        public IObservable<List<MosaicName>> GetMosaicNames(List<string> mosaicIds)
        {
            var postBody = "{ mosaicIds: " + JsonConvert.SerializeObject(mosaicIds) + "}";

            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["namespaces", "mosaic", "names"]), new StringContent(postBody)))
                .Select(i => JsonConvert.DeserializeObject<List<MosaicName>>(i.Content.ToString()));
        }
    }
}
