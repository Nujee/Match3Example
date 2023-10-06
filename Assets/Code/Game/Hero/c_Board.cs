using Leopotam.EcsLite;

namespace Code.Game.Hero
{
    public struct c_Board
    {
        public int Rows;
        public int Columns;
        public EcsPackedEntity[,] CellsPacked;
        public bool IsRenewable;
    }
}