// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MosaicHttp.cs" company="Nem.io">   
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
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using io.nem2.sdk.src.Infrastructure.Mapping;
using System.Text.Json;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class MosaicHttp : HttpRouter, IMosaicRepository
    {
        public MosaicHttp(string host, int port) : base(host, port) { }

        public IObservable<List<MosaicEvent>> SearchMosaics(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["mosaics"], queryModel)))
                .Select(m => ResponseFilters<MosaicEvent>.FilterEvents(m, "data"));
        }

        public IObservable<MosaicEvent> GetMosaic(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics", mosaicId])))
                .Select(r => { return ObjectComposer.GenerateObject<MosaicEvent>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<List<MosaicEvent>> GetMosaics(List<string> mosaicIds)
        {     
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["mosaics"]), new StringContent(JsonSerializer.Serialize(new MosaicIds() { mosaicIds = mosaicIds }), Encoding.UTF8, "application/json")))
                .Select(r => { return ResponseFilters<MosaicEvent>.FilterEvents(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetMosaicMerkle(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["mosaics", mosaicId, "merkle"])))
                .Select(r => { return ObjectComposer.GenerateObject<MerkleRoot>(OverrideEnsureSuccessStatusCode(r)); });

        }

        public IObservable<List<MosaicRestrictionData>> SearchMosaicRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic"], queryModel)))
               .Select(r => { return ResponseFilters<MosaicRestrictionData>.FilterEvents(OverrideEnsureSuccessStatusCode(r), "data"); });
        }

        public IObservable<MosaicRestrictionData> GetMosaicRestriction(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetAsync(GetUri(["restrictions", "mosaic", compositeHash])))
                .Select(r => { return ObjectComposer.GenerateObject<MosaicRestrictionData>(OverrideEnsureSuccessStatusCode(r)); });
        }

        public IObservable<MerkleRoot> GetMosaicRestrictionMerkle(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "mosaic", compositeHash, "merkle"])))
                .Select(ObjectComposer.GenerateObject<MerkleRoot>);
        }
    }
}
