using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Code.Game.Features.DropItems
{
    public struct c_DropItems : IEcsAutoReset<c_DropItems>
    {
        public EcsPackedEntity BoardPacked;
        public List<DropData> DropDataList;

        public void AutoReset(ref c_DropItems c)
        {
            c.DropDataList = new List<DropData>();
        }
    }
}