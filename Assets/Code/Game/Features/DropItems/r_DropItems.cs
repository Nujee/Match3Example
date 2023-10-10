﻿using System.Collections.Generic;
using Code.Game.Features.CleanBoard;
using Leopotam.EcsLite;

namespace Code.Game.Features.DropItems
{
    public struct r_DropItems
    {
        public EcsPackedEntity BoardPacked;
        public List<DropData> DropDataList;

        public r_DropItems(EcsPackedEntity boardPacked, List<DropData> dropDataList)
        {
            BoardPacked = boardPacked;
            DropDataList = dropDataList;
        }
    }
}