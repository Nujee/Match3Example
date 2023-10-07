using Leopotam.EcsLite;

namespace Code.Game.Features.FindCombos
{
    public struct r_FindCombos
    {
        public EcsPackedEntity BoardPacked;
        
        public r_FindCombos(EcsPackedEntity boardPacked)
        {
            BoardPacked = boardPacked;
        }
    }
}