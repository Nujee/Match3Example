using Code.Game.Items;
using Code.Game.Main;
using Code.Game.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Hero
{
    public sealed class i_Board : IEcsInitSystem
    {
        private readonly EcsPoolInject<c_Board> _boardPool = default;
        private readonly EcsPoolInject<c_Cell> _cellPool = default;

        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;
        private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;
        
        private readonly EcsWorldInject _world = default;

        public void Init(IEcsSystems systems)
        {
            var ls = _levelSettings.Value;

            var boardEntity = _world.Value.NewEntity();
            ref var c_board = ref _boardPool.Value.Add(boardEntity);

            //c_board.Type = BoardType.Main;
            
            c_board.Rows = ls.BoardRows;
            c_board.Columns = ls.BoardColumns;
            c_board.IsRenewable = true;
            c_board.CellsPacked = new EcsPackedEntity[c_board.Rows, c_board.Columns];
            
            for (var i = 0; i < c_board.Rows; i++)
            for (var j = 0; j < c_board.Columns; j++)
            {
                // Calculate world position centered from (0, 0, 0).
                // Rows go top-down.
                // Columns go left-right.
                var worldX = ls.BoardCellsWidth * (j - (ls.BoardColumns - 1) / 2f);
                var worldY = ls.BoardSlotsHeight * ((ls.BoardRows - 1) / 2f - i);

                var cellEntity = _world.Value.NewEntity();
                ref var c_cell = ref _cellPool.Value.Add(cellEntity);
                c_cell.BoardPosition = new Vector2Int(i, j);
                c_cell.WorldPosition = new Vector3(worldX, worldY);
                c_board.CellsPacked[i, j] = _world.Value.PackEntity(cellEntity);

                c_cell.SetRandomItem(_world.Value, _itemDataSet.Value, c_cell.WorldPosition);
            }
        }
    }
}