using Code.Game.Hero;
using Code.Game.Items;
using Code.Game.Main;
using Code.Game.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.DropItems
{
    public sealed class s_SetUpItemsDrop : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_DropItems>> _featureRequestsFilter = default;

        private readonly EcsPoolInject<c_DropItems> _featurePool = default;
        private readonly EcsPoolInject<c_Board> _boardPool = default;
        private readonly EcsPoolInject<c_Cell> _cellPool = default;

        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;
        private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;

        private readonly EcsWorldInject _world = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestsFilter.Value)
            {
                ref var r_featureRequest = ref _featureRequestsFilter.Pools.Inc1.Get(featureRequest);

                var featureEntity = _world.Value.NewEntity();
                ref var c_feature  = ref _featurePool.Value.Add(featureEntity);
                
                if (!r_featureRequest.BoardPacked.Unpack(_world.Value, out var boardEntity)) { continue; }
                c_feature.BoardPacked = r_featureRequest.BoardPacked;
                
                ref var c_board = ref _boardPool.Value.Get(boardEntity);

                for (var row = 0; row < c_board.Rows; row++)
                for (var col = 0; col < c_board.Columns; col++)
                {
                    var cellPacked = c_board.CellsPacked[row, col];
                    if (!cellPacked.Unpack(_world.Value, out var cellEntity)) { continue; }
                    ref var c_cell = ref _cellPool.Value.Get(cellEntity);
                    
                    if (!c_cell.AttachedItemPacked.Unpack(_world.Value, out _)) { continue; }
                    
                    var dropDelta = c_board.Rows * _levelSettings.Value.BoardSlotsHeight * Vector3.up;
                    var dropDelayOffset = (c_board.Rows - 1 - row) + col;
                    
                    var oldItemDropData = new DropData
                    {
                        ItemPacked = c_cell.AttachedItemPacked,
                        Delay = dropDelayOffset * _levelSettings.Value.DropItemsInBetweenDelay,
                        TargetPosition = c_cell.WorldPosition - dropDelta,
                        IsDisposable = true
                    };
                    c_feature.DropDataList.Add(oldItemDropData);
                    
                    var newItemDropData = new DropData
                    {
                        ItemPacked = c_cell.SetRandomItem(_world.Value, _itemDataSet.Value, c_cell.WorldPosition + dropDelta),
                        Delay = (dropDelayOffset + c_board.Rows) * _levelSettings.Value.DropItemsInBetweenDelay,
                        TargetPosition = c_cell.WorldPosition,
                        IsDisposable = false
                    };
                    c_feature.DropDataList.Add(newItemDropData);
                }
                
                _world.Value.DelEntity(featureRequest);
            }
        }
    }
}