using System.Collections.Generic;
using System.Linq;
using Code.Game.Features.DropItems;
using Code.Game.Hero;
using Code.Game.Items;
using Code.Game.Main;
using Code.Game.Utils;
using Code.MySubmodule.ECS.Features.RequestsToFeatures;
using Code.MySubmodule.ECS.Features.RequestTrain;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.RemoveCombo
{
    public sealed class s_TopUpCells : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Step<s_TopUpCells>, c_RemoveCombo>> _featureFilter = default;
        
        private readonly EcsPoolInject<c_Board> _boardPool = default;
        private readonly EcsPoolInject<c_Cell> _cellPool = default;

        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;
        private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                var world = systems.GetWorld();
                var ls = _levelSettings.Value;
                
                ref var c_feature = ref _featureFilter.Pools.Inc2.Get(featureEntity);
                if (!c_feature.BoardPacked.Unpack(systems.GetWorld(), out var boardEntity)) { continue; }
                ref var c_board = ref _boardPool.Value.Get(boardEntity);
                
                var dropDataList = new List<DropData>();
                
                var aggregatedList = ReassignItemsToLowerCells(ref c_board, world);
                for (var topUpColumn = 0; topUpColumn < aggregatedList.Count; topUpColumn++)
                {
                    var topUpColumnLength = aggregatedList[topUpColumn].Count;
                    for (var topUpRow = 0; topUpRow < topUpColumnLength; topUpRow++)
                    {
                        var topUpDropData = new DropData
                        {
                            ItemPacked = aggregatedList[topUpColumn][topUpRow].itemPacked,
                            Delay = ls.DropItemsInBetweenDelay * topUpRow,
                            TargetPosition = aggregatedList[topUpColumn][topUpRow].endpoint,
                            Speed = ls.DropItemsStartSpeed
                        };
                        dropDataList.Add(topUpDropData);
                    }
                }

                world.AddRequest(new r_DropItems(c_feature.BoardPacked, dropDataList));
                
                world.DelEntity(featureEntity);
            }
        }
        
        private List<List<(EcsPackedEntity itemPacked, Vector3 endpoint)>> ReassignItemsToLowerCells(ref c_Board c_board, EcsWorld world)
        {
            var result = new List<List<(EcsPackedEntity, Vector3)>>();
            
            var rows = c_board.CellsPacked.GetLength(0);
            var columns = c_board.CellsPacked.GetLength(1);

            // loop through columns, left to right
            for (var col = 0; col < columns; col++)
            {
                // make a counter for empty slots in this column
                var thisColumnEmptyCells = 0;
                var itemEntitiesToEndpoints = new List<(EcsPackedEntity, Vector3 endpoint)>();
                
                // loop through rows, bottom to top
                for (var row = rows - 1; row >= 0; row--)
                {
                    var thisCell = c_board.CellsPacked[row, col];
                    
                    if (!thisCell.Unpack(world, out var thisCellEntity)) { continue; }
                    ref var c_thisCell = ref _cellPool.Value.Get(thisCellEntity);

                    // if this slot is empty, increment empty slots counter...
                    if (!c_thisCell.AttachedItemPacked.Unpack(world, out _))
                    {
                        thisColumnEmptyCells++;
                    }
                    // ...or else if this slot isn't empty and there are empty slots below it,
                    // assign this slot's item to the lowest empty slot below it...
                    else if (thisColumnEmptyCells > 0)
                    {
                        var targetCellPacked = c_board.CellsPacked[row + thisColumnEmptyCells, col];
                        
                        if (!targetCellPacked.Unpack(world, out var cellToEntity)) { continue; }
                        ref var c_cellTo = ref _cellPool.Value.Get(cellToEntity);
                        
                        c_cellTo.AttachedItemPacked = c_thisCell.AttachedItemPacked;

                        // ... and add this item to the list of items to drop
                        itemEntitiesToEndpoints.Add((c_cellTo.AttachedItemPacked, c_cellTo.WorldPosition));
                    }
                }

                // loop through formally empty slots on top of this column
                // (in fact, they still point to their AttachedItemEntities), top to bottom,
                // and assign them new random items
                for (var row = 0; row < thisColumnEmptyCells; row++)
                {
                    var targetCellPacked = c_board.CellsPacked[row, col];
                    
                    if (!targetCellPacked.Unpack(world, out var cellToEntity)) { continue; }
                    ref var c_cellTo = ref _cellPool.Value.Get(cellToEntity);
                    
                    var newItemStartPosition = c_cellTo.WorldPosition 
                                               + (_levelSettings.Value.BoardSlotsHeight * thisColumnEmptyCells * Vector3.up);
                    c_cellTo.SetRandomItem(world, _itemDataSet.Value, newItemStartPosition);

                    // add this item to the list of items to drop
                    itemEntitiesToEndpoints.Add((c_cellTo.AttachedItemPacked, c_cellTo.WorldPosition));
                }

                // sort items to drop by their y coordinate (<=> from bottom to top)
                var itemEntitiesToEndpointsOrdered = itemEntitiesToEndpoints
                    .OrderBy(x => x.endpoint.y)
                    .ToList();
                
                result.Add(itemEntitiesToEndpointsOrdered);
            }

            return result;
        }
    }
}