using System.Collections.Generic;
using Code.Game.Features.DropItems;
using Code.Game.Hero;
using Code.Game.Items;
using Code.Game.Main;
using Code.Game.Utils;
using Code.MySubmodule.ECS.Features.RequestsToFeatures;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.CleanBoard
{
    public sealed class s_SetUpCleanBoard : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_CleanBoard>> _featureRequestsFilter = default;
        
        private readonly EcsPoolInject<c_Board> _boardPool = default;
        private readonly EcsPoolInject<c_Cell> _cellPool = default;

        private readonly EcsCustomInject<LevelSettings> _ls = default;
        private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestsFilter.Value)
            {
                ref var r_featureRequest = ref _featureRequestsFilter.Pools.Inc1.Get(featureRequest);
                if (!r_featureRequest.BoardPacked.Unpack(_world.Value, out var boardEntity)) { continue; }
                ref var c_board = ref _boardPool.Value.Get(boardEntity);

                var dropDataList = new List<DropData>();
                
                for (var row = 0; row < c_board.Rows; row++)
                for (var col = 0; col < c_board.Columns; col++)
                {
                    var cellPacked = c_board.CellsPacked[row, col];
                    
                    if (!cellPacked.Unpack(_world.Value, out var cellEntity)) { continue; }
                    ref var c_cell = ref _cellPool.Value.Get(cellEntity);

                    var dropDisplacement = c_board.Rows * _ls.Value.BoardSlotsHeight * Vector3.down;
                    var slotPositionOffset = ((c_board.Rows - 1) - row) + col;
                    
                    var oldItemDropData = new DropData
                    {
                        ItemPacked = c_cell.AttachedItemPacked,
                        Delay = slotPositionOffset * _ls.Value.DropItemsInBetweenDelay,
                        TargetPosition = c_cell.WorldPosition + dropDisplacement,
                        Speed = _ls.Value.DropItemsStartSpeed,
                        IsDisposable = true
                    };
                    dropDataList.Add(oldItemDropData);
                    
                    var newItemDropData = new DropData
                    {
                        ItemPacked = c_cell.SetRandomItem(_world.Value, _itemDataSet.Value, c_cell.WorldPosition - dropDisplacement),
                        Delay = (slotPositionOffset + c_board.Rows) * _ls.Value.DropItemsInBetweenDelay,
                        TargetPosition = c_cell.WorldPosition,
                        Speed = _ls.Value.DropItemsStartSpeed,
                        IsDisposable = false
                    };
                    dropDataList.Add(newItemDropData);
                }
                
                _world.Value.AddRequest(new r_DropItems(r_featureRequest.BoardPacked, dropDataList));

                _world.Value.DelEntity(featureRequest);
            }
        }
    }
}