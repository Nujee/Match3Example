using Leopotam.EcsLite;

namespace Code.Game.Features.CleanBoard
{
    public struct r_CleanBoard
    {
        public EcsPackedEntity BoardPacked;
        
        public r_CleanBoard(EcsPackedEntity boardPacked)
        {
            BoardPacked = boardPacked;
        }
    }
}