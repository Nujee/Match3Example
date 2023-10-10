using Leopotam.EcsLite;

namespace Code.Game.Features.TopUp
{
    public struct r_TopUp
    {
        public EcsPackedEntity BoardPacked;
        
        public r_TopUp(EcsPackedEntity boardPacked)
        {
            BoardPacked = boardPacked;
        }
    }
}