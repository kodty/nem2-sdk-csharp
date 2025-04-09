// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 02-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="NamespaceId.cs" company="Nem.io">
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

using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;

namespace io.nem2.sdk.Model.Namespace
{
    public class NamespaceId
    {
        public ulong Id { get; }

        public string Name { get; }

        public string HexId { get; }

        public NamespaceId(string id)
        {     
            if (id == null) throw new ArgumentNullException(nameof(id) + " cannot be null");

            Id = IdGenerator.GenerateId(0, id);
            Name = id;

            HexId = BitConverter.GetBytes(Id).ToHexUpper();
        }



        public NamespaceId(ulong id)
        {
            Id = id;
            HexId = BitConverter.GetBytes(id).ToHexUpper();
        }
        
        public static NamespaceId Create(string id)
        {
            return new NamespaceId(id);
        }
    }
}
