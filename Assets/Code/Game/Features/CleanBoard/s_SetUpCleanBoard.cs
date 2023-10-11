using System.Collections.Generic;
using Code.Game.Features.DropItems;
using Code.Game.Features.FindCombos;
using Code.Game.Hero;
using Code.Game.Items;
using Code.Game.Main;
using Code.Game.Utils;
using Code.MySubmodule.DebugTools.MyLogger;
using Code.MySubmodule.ECS.EndGame;
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

        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;
        private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestsFilter.Value)
            {
                var world = systems.GetWorld();
                var ls = _levelSettings.Value;

                ref var r_featureRequest = ref _featureRequestsFilter.Pools.Inc1.Get(featureRequest);
                if (!r_featureRequest.BoardPacked.Unpack(world, out var boardEntity)) { continue; }
                ref var c_board = ref _boardPool.Value.Get(boardEntity);

                var dropDataList = new List<DropData>();
                
                for (var row = 0; row < c_board.Rows; row++)
                for (var col = 0; col < c_board.Columns; col++)
                {
                    var cellPacked = c_board.CellsPacked[row, col];
                    
                    if (!cellPacked.Unpack(world, out var cellEntity)) { continue; }
                    ref var c_cell = ref _cellPool.Value.Get(cellEntity);

                    var dropDisplacement = c_board.Rows * ls.BoardSlotsHeight * Vector3.down;
                    var slotPositionOffset = ((c_board.Rows - 1) - row) + col;
                    
                    var oldItemDropData = new DropData
                    {
                        ItemPacked = c_cell.AttachedItemPacked,
                        Delay = slotPositionOffset * ls.DropItemsInBetweenDelay,
                        TargetPosition = c_cell.WorldPosition + dropDisplacement,
                        Speed = ls.DropItemsStartSpeed,
                        IsDisposable = true
                    };
                    dropDataList.Add(oldItemDropData);
                    
                    var newItemDropData = new DropData
                    {
                        ItemPacked = c_cell.SetRandomItem(world, _itemDataSet.Value, c_cell.WorldPosition - dropDisplacement),
                        Delay = (slotPositionOffset + c_board.Rows) * ls.DropItemsInBetweenDelay,
                        TargetPosition = c_cell.WorldPosition,
                        Speed = ls.DropItemsStartSpeed,
                        IsDisposable = false
                    };
                    dropDataList.Add(newItemDropData);
                }
                
                world.AddRequest(new r_DropItems(r_featureRequest.BoardPacked, dropDataList));

                world.DelEntity(featureRequest);
            }
        }
    }
}