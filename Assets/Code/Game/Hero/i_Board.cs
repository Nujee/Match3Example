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

       // private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;
        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;
        private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;

        private readonly EcsWorldInject _world = default;
        
        public void Init(IEcsSystems systems)
        {
            var ls = _levelSettings.Value;
            var world = _world.Value;
            
            var boardEntity = world.NewEntity();
            ref var c_board = ref _boardPool.Value.Add(boardEntity);

            var rows = ls.BoardRows;
            var columns = ls.BoardColumns;
            c_board.Cells = new EcsPackedEntity[rows, columns];
            
            for (var i = 0; i < rows; i++)
            for (var j = 0; j < columns; j++)
            {
                // calculate world position centered from (0, 0, 0)
                var worldX = ls.BoardCellsWidth * (j - (ls.BoardColumns - 1) / 2f);
                var worldY = ls.BoardSlotsHeight * ((ls.BoardRows - 1) / 2f - i);

                var cellEntity = world.NewEntity();
                ref var c_cell = ref _cellPool.Value.Add(cellEntity);
                c_cell.BoardPosition = new Vector2Int(i, j);
                c_cell.WorldPosition = new Vector3(worldX, worldY);
                c_board.Cells[i, j] = world.PackEntity(cellEntity);

                c_cell.SetRandomItem(_itemDataSet.Value, world, c_cell.WorldPosition);
            }
        }
    }
}