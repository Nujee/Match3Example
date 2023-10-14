using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Code.Game.Features.DropItems
{
    public struct r_DropItems
    {
        public EcsPackedEntity BoardPacked;
        public readonly List<DropData> DropDataList;

        public r_DropItems(EcsPackedEntity boardPacked, List<DropData> dropDataList)
        {
            BoardPacked = boardPacked;
            DropDataList = dropDataList;
        }
    }
}