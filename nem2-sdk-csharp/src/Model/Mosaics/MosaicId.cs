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

using io.nem2.sdk.Core.Utils;

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
            HexId = Id.ToString("X");
        }
        
        
        public MosaicId(string identifier)
        {
            if (string.IsNullOrEmpty(identifier)) throw new ArgumentException(identifier + " is not valid");
            if (!identifier.Contains(":")) throw new ArgumentException(identifier + " is not valid");
            var parts = identifier.Split(':');
            if (parts.Length != 2) throw new ArgumentException(identifier + " is not valid");
            if (parts[0] == "") throw new ArgumentException(identifier + " is not valid");
            if (parts[1] == "") throw new ArgumentException(identifier + " is not valid");
            var namespaceName = parts[0];
            MosaicName = parts[1];
            FullName = identifier;
            Id = IdGenerator.GenerateId(IdGenerator.GenerateId(0, namespaceName), MosaicName);     
            HexId = Id.ToString("X");
        }

        
        public static MosaicId CreateFromMosaicIdentifier(string identifier)
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
