// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MosaicInfo.cs" company="Nem.io">   
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

using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Namespace;

namespace io.nem2.sdk.Model.Mosaics
{
    public class MosaicInfo
    {
        public bool IsActive { get; }

        public int Index { get; }

        public string MetaId { get; }

        public NamespaceId NamespaceId { get; }

        public MosaicId MosaicId { get; }

        public ulong Supply { get; }

        public ulong Height { get; }
       
        public PublicAccount Owner { get; }

        public MosaicProperties Properties { get; }

        public bool IsExpired => !IsActive;

        public bool IsSupplyMutable => Properties.IsSupplyMutable;

        public bool IsTransferable => Properties.IsTransferable;

        public bool IsLevyMutable => Properties.IsLevyMutable;

        public ulong Duration => Properties.Duration;

        public int Divisibility => Properties.Divisibility;

        public MosaicInfo(bool active, int index, string metaId, NamespaceId namespaceId, MosaicId mosaicId, ulong supply, ulong height, PublicAccount owner, MosaicProperties properties)
        {   
            IsActive = active;
            Index = index;
            MetaId = metaId;
            NamespaceId = namespaceId;
            MosaicId = mosaicId;
            Supply = supply;
            Height = height;
            Owner = owner;
            Properties = properties;
        }
    }
}
