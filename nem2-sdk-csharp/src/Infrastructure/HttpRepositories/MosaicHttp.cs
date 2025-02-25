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

using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.src.Infrastructure.HttpRepositories;
using io.nem2.sdk.src.Infrastructure.HttpRepositories.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    public class MosaicHttp : HttpRouter, IMosaicRepository
    {
        public MosaicHttp(string host, int port) : base(host, port) { }


        public IObservable<Mosaics> SearchMosaics(QueryModel queryModel)
        {
            //if (mosaicIds == null) throw new ArgumentNullException(nameof(mosaicIds));
            //if (mosaicIds.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(mosaicIds));
            //if (mosaicIds.Any(e => e.Length != 16 || !Regex.IsMatch(e, @"\A\b[0-9a-fA-F]+\b\Z"))) throw new ArgumentException("Collection contains invalid id.");

            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["mosaics"], queryModel)))
                .Select(JsonConvert.DeserializeObject<Mosaics>);
        }

        public IObservable<MosaicEvent> GetMosaic(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["mosaics", mosaicId])))
                .Select(JsonConvert.DeserializeObject<MosaicEvent>);    
        }

        public IObservable<List<MosaicEvent>> GetMosaics(List<string> mosaicIds)
        {
            var data = new MosaicIds()
            {
                Mosaic_Ids = mosaicIds
            };
          
            return Observable.FromAsync(async ar => await Client.PostAsync(GetUri(["mosaics"]), new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")))
                .Select(i => {
                    return JsonConvert.DeserializeObject<List<MosaicEvent>>(i.Content.ReadAsStringAsync().Result);
                   
                });
        }

        public IObservable<MerkleRoot> GetMosaicMerkle(string mosaicId)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["mosaics", mosaicId, "merkle"])))
                .Select(JsonConvert.DeserializeObject<MerkleRoot>);

        }

        public IObservable<MosaicRestrictions> SearchMosaicRestrictions(QueryModel queryModel)
        {
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["mosaics"], queryModel)))
               .Select(JsonConvert.DeserializeObject<MosaicRestrictions>);
        }

        public IObservable<MosaicRestrictionEntry> GetMosaicRestriction(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "mosaics", compositeHash])))
                .Select(JsonConvert.DeserializeObject<MosaicRestrictionEntry>);
        }

        public IObservable<MerkleRoot> GetMosaicRestrictionMerkle(string compositeHash)
        { 
            return Observable.FromAsync(async ar => await Client.GetStringAsync(GetUri(["restrictions", "mosaics", compositeHash, "merkle"])))
                .Select(JsonConvert.DeserializeObject<MerkleRoot>);
        }

        /*
         * 
         * private MosaicProperties ExtractMosaicProperties(ulong[] properties)
        {
            var flags = "00" + Convert.ToString((long)properties[0], 2);
            var bitMapFlags = flags.Substring(flags.Length - 3, 3);

            return new MosaicProperties(bitMapFlags.ToCharArray()[2] == '1',
                bitMapFlags.ToCharArray()[1] == '1',
                bitMapFlags.ToCharArray()[0] == '1',
                (int)properties[1],
                properties.Count() == 3 ? properties[2] : 0);
        }

        public IObservable<List<MosaicInfo>> GetMosaicsFromNamespace(NamespaceId namespaceId)
        {
            if (namespaceId == null) throw new ArgumentNullException(nameof(namespaceId));
            if(namespaceId.HexId.Length != 16 || !Regex.IsMatch(namespaceId.HexId, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("invalid namespace id");

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkType().Take(1);

            return Observable.FromAsync(async ar => await MosaicRoutesApi.GetMosaicsFromNamespaceAsync(namespaceId.HexId))
                .Select(e => e.Select(mosaic => new MosaicInfo(
                    mosaic.Meta.Active,
                    mosaic.Meta.Index,
                    mosaic.Meta.Id,
                    new NamespaceId(BitConverter.ToUInt64(mosaic.Mosaic.NamespaceId.FromHex(), 0)),
                    new MosaicId(BitConverter.ToUInt64(mosaic.Mosaic.MosaicId.FromHex(), 0)),
                    mosaic.Mosaic.Supply,
                    mosaic.Mosaic.Height,
                    new PublicAccount(mosaic.Mosaic.Owner, networkTypeResolve.Wait()),
                    ExtractMosaicProperties(mosaic.Mosaic.Properties))).ToList());
        }*/
        /*
        public IObservable<List<MosaicName>> GetMosaicsName(List<string> mosaicIds)
        {
            if (mosaicIds == null) throw new ArgumentNullException(nameof(mosaicIds));
            if (mosaicIds.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(mosaicIds));
            if (mosaicIds.Any(e => e.Length != 16 || !Regex.IsMatch(e, @"\A\b[0-9a-fA-F]+\b\Z"))) throw new ArgumentException("Collection contains invalid id.");

            return Observable.FromAsync(async ar => await MosaicRoutesApi.GetMosaicsNameAsync(new MosaicIds() { mosaicIds = mosaicIds }))
                .Select(e => e.Select(mosaic => new MosaicName(new MosaicId(mosaic.MosaicId), mosaic.Name, new NamespaceId(mosaic.ParentId))).ToList());
        }  */
    }
}
