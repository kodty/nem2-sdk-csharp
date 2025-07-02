// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MosaicId.cs" company="Nem.io">   
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

using io.nem2.sdk.Core.Crypto.Chaos.NaCl;
using io.nem2.sdk.src.Export;

namespace io.nem2.sdk.Model.Mosaics
{
    public class MosaicId
    {
        public ulong Id { get; }


        public string MosaicName { get; }


        public string FullName { get; }


        public string HexId { get; }


        public bool IsNamePresent => MosaicName != null;


        public bool IsFullNamePresent => FullName != null;

        
        public MosaicId(ulong id)
        {
            Id = id;

            HexId = DataConverter.ConvertFromUInt64(id).ToHexLower();
        }

        public MosaicId(string hexId)
        {
            Id = DataConverter.ConvertToUInt64(hexId.FromHex());

            HexId = hexId;
        }

        public MosaicId(string[] identifierParts)
        {
            if (identifierParts.Count() > 3)
                throw new Exception("too many parts");

            var namespaceName = identifierParts[0];
            MosaicName = identifierParts[1];
            FullName = String.Join(':', identifierParts);

            Id = 0;     
            HexId = "";
        }

        
        public static MosaicId CreateFromHexMosaicIdentifier(string identifier)
        {
            return new MosaicId(identifier);
        } 
        
        public override bool Equals(object obj)
        {
            return Id == ((MosaicId) obj)?.Id;
        }

        public override int GetHashCode()
        {
            var hashCode = 1792400168;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MosaicName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FullName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HexId);
            return hashCode;
        }
    }
}
