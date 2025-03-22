// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="BlockchainHttp.cs" company="Nem.io">   
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
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.IRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;


namespace io.nem2.sdk.Infrastructure.HttpRepositories
{

    public class BlockchainHttp : HttpRouter, IBlockchainRepository
    {
        public BlockchainHttp(string host, int port) 
            : base(host, port) { }

        public IObservable<List<BlockInfo>> SearchBlocks(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks"], queryModel)))
                 .Select(r => { return ResponseFilters<BlockInfo>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<BlockInfo> GetBlock(ulong height)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height])))
                .Select(r => { return ObjectComposer.GenerateObject<BlockInfo>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<MerklePath>> GetBlockTransactionMerkle(ulong height, string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height, "transactions", hash, "merkle"])))
                .Select(r => { return ResponseFilters<MerklePath>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "merklePath"); });
        }

        public IObservable<List<MerklePath>> GetBlockRecieptMerkle(ulong height, string hash)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["blocks", height, "reciepts", hash, "merkle"])))
              .Select(r => { return ResponseFilters<MerklePath>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "merklePath"); });
        }
 
        public IObservable<BlockchainInfo> GetBlockchainInfo()
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["chain", "info"])))
                .Select(r => { return ObjectComposer.GenerateObject<BlockchainInfo>(OverrideEnsureSuccessStatusCode(r)); });
        }
    }
}
