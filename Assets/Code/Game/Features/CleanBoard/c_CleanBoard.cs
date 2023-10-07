using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Code.Game.Features.CleanBoard
{
    public struct c_CleanBoard : IEcsAutoReset<c_CleanBoard>
    {
        public EcsPackedEntity BoardPacked;
        public List<DropData> DropDataList;

        public void AutoReset(ref c_CleanBoard c)
        {
            c.DropDataList = new List<DropData>();
        }
    }
}