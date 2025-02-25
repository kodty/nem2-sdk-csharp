// ***********************************************************************
// Assembly         : nis2
// Author           : kaili
// Created          : 05-24-2018
//
// Last Modified By : kaili
// Last Modified On : 05-24-2018
// ***********************************************************************
// <copyright file="NamespaceInfo.cs" company="Nem.io">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Namespace
{
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
    namespace io.nem2.sdk.Model.Mosaics
    {
        public class NamespaceInfo
        {
            public bool IsActive { get; }

            public int Index { get; }

            public string MetaId { get; }

            public NamespaceId ParentId { get; }

            public NamespaceTypes.Types NamespaceType { get; }

            public int Depth { get; }

            public List<NamespaceId> Levels { get; }
  
            public ulong StartHeight { get; }

            public ulong EndHeight { get; }

            public PublicAccount Owner { get; }

            public bool IsExpired => !IsActive;

            public NamespaceInfo(bool active, int index, string metaId, NamespaceTypes.Types namespaceId, int depth, List<NamespaceId> levels, NamespaceId parentId, ulong startHeight, ulong endHeight, PublicAccount owner)
            {
                IsActive = active;
                Owner = owner;
                Index = index;
                MetaId = metaId;
                NamespaceType = namespaceId;
                ParentId = parentId;
                StartHeight = startHeight;
                EndHeight = endHeight;
                Depth = depth;
                Levels = levels;
            }
        }
    }

}
