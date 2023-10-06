using Code.Game.Features.CleanBoard;
using Code.Game.Hero;
using Code.Game.Items;
using Code.Game.Main;
using Code.Game.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.RenewBoard
{
    public sealed class s_UpdateSlotItems : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_RenewBoard>> _featureFilter = default;
        
        private readonly EcsPoolInject<c_Board> _boardPool = default;
        private readonly EcsPoolInject<c_Cell> _cellPool = default;
        private readonly EcsPoolInject<r_CleanBoard> _dropItemsPool = default;
        
        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;
        private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;

        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                ref var c_feature = ref _featureFilter.Pools.Inc1.Get(featureEntity);
                
                if (!c_feature.BoardPacked.Unpack(_world.Value, out var boardEntity)) { continue; }
                ref var c_board = ref _boardPool.Value.Get(boardEntity);
                
                var dropItemsRequest = _world.Value.NewEntity();
                ref var r_dropItems = ref _dropItemsPool.Value.Add(dropItemsRequest);
                r_dropItems.BoardPacked = c_feature.BoardPacked;
                
                for (var row = 0; row < c_board.Rows; row++)
                for (var col = 0; col < c_board.Columns; col++)
                {
                    var cellPacked = c_board.CellsPacked[row, col];
                    if (!cellPacked.Unpack(_world.Value, out var cellEntity)) { continue; }
                    ref var c_cell = ref _cellPool.Value.Get(cellEntity);
                    
                    if (!c_cell.AttachedItemPacked.Unpack(_world.Value, out _)) { continue; }
                    
                    var dropDelta = c_board.Rows * _levelSettings.Value.BoardSlotsHeight * Vector3.up;
                    var dropDelayOffset = (c_board.Rows - 1 - row) + col;
                    
                    // var oldItemDropData = new DropData
                    // {
                    //     ItemPacked = c_cell.AttachedItemPacked,
                    //     Delay = dropDelayOffset * _levelSettings.Value.DropItemsInBetweenDelay,
                    //     Speed = _levelSettings.Value.DropItemsStartSpeed,
                    //     TargetPosition = c_cell.WorldPosition - dropDelta,
                    //     IsDisposable = true
                    // };
                    // r_dropItems.DropDataList.Add(oldItemDropData);
                    //
                    // var newItemDropData = new DropData
                    // {
                    //     ItemPacked = c_cell.SetRandomItem(_world.Value, _itemDataSet.Value, c_cell.WorldPosition + dropDelta),
                    //     Delay = (dropDelayOffset + c_board.Rows) * _levelSettings.Value.DropItemsInBetweenDelay,
                    //     Speed = _levelSettings.Value.DropItemsStartSpeed,
                    //     TargetPosition = c_cell.WorldPosition,
                    //     IsDisposable = false
                    // };
                    // r_dropItems.DropDataList.Add(newItemDropData);
                }
                
                _world.Value.DelEntity(featureEntity);
            }
        }
    }
}