using Code.Game.Hero;
using Code.Game.Items;
using Code.Game.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.RenewBoard
{
    public sealed class s_RenewBoard : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_RenewBoard>> _renewBoardRequests = default;
        private readonly EcsFilterInject<Inc<c_Board>> _boardsFilter = default;

        private readonly EcsPoolInject<r_RenewBoard> _renewBoardPool = default;
        private readonly EcsPoolInject<c_Cell> _cellPool = default;
        private readonly EcsPoolInject<c_Item> _itemPool = default;

        private readonly EcsCustomInject<ItemDataSet> _itemDataSet = default;

        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var renewBoardRequest = _world.Value.NewEntity();
                _renewBoardPool.Value.Add(renewBoardRequest);
            }
            
            foreach (var boardRenewRequest in _renewBoardRequests.Value)
            {
                foreach (var boardEntity in _boardsFilter.Value)
                {
                    ref var c_board = ref _boardsFilter.Pools.Inc1.Get(boardEntity);
                    foreach (var cellPacked in c_board.CellsPacked)
                    {
                        if (!cellPacked.Unpack(_world.Value, out var cellEntity)) { continue; }
                        ref var c_cell = ref _cellPool.Value.Get(cellEntity);
                        
                        if (!c_cell.AttachedItemPacked.Unpack(_world.Value, out var attachedItemEntity)) { continue; }
                        ref var c_item = ref _itemPool.Value.Get(attachedItemEntity);
                        
                        c_item.Data.Pool.Return(attachedItemEntity);
                        c_cell.SetRandomItem(_world.Value, _itemDataSet.Value, c_cell.WorldPosition);
                    }
                }
                
                _world.Value.DelEntity(boardRenewRequest);
            }

            
            // foreach (var entity in _filter.Value)
            // {
            //     ref var c_board = ref _filter.Pools.Inc1.Get(entity);
            //     ref var c_cleanBoard = ref _filter.Pools.Inc2.Get(entity);
            //     
            //     c_cleanBoard.Delay -= Time.deltaTime;
            //     if (c_cleanBoard.Delay > 0) continue;
            //     
            //     var rows = c_board.Slots.GetLength(0);
            //     var columns = c_board.Slots.GetLength(1);
            //     var ls = _levelSettings.Value;
            //
            //     switch (c_cleanBoard.State)
            //     {
            //         case DropState.Started:
            //         {
            //             var slotHeight = ls.BoardSlotsHeight;
            //             var extraRows = ls.ExtraRows;
            //             var samplableItemDatas = c_board.ItemDatasSample.Where(x => x.IsSamplable).ToList();
            //             var topGap = ls.GapForNewItemsOnBoardTop;
            //             var basicOffset = topGap + ls.BoardRows + extraRows;
            //             
            //             if (_boardPresetter.Value.IsActive)
            //             {
            //                 // Set old items to drop down from slot
            //                 for (var row = 0; row < rows; row++)
            //                 {
            //                     for (var col = 0; col < columns; col++)
            //                     {
            //                         var slot = c_board.Slots[row, col];
            //                 
            //                         ref var c_oldDrop = ref _dropPool.Value.Add(slot.AttachedItemEntity);
            //                         c_oldDrop.Acceleration = ls.DropAcceleration;
            //                         c_oldDrop.Speed = ls.DropSpeed;
            //                         var dropDistance = Vector3.down * (2f * slotHeight); // 2 to avoid premature disappearance
            //                         c_oldDrop.EndPoint = slot.WorldPosition + dropDistance * ls.BoardRows;
            //                         c_oldDrop.Delay = ls.RowInBetweenDropDelay * (ls.BoardRows - 1 - row) 
            //                                           + ls.ColInBetweenDropDelay * col;
            //                         c_oldDrop.IsToBeRemoved = true;
            //                     }
            //                 }
            //                 
            //                 //Set new items to drop from top to slot
            //                 for (var col = 0; col < columns; col++)
            //                 {
            //                     var columnsOffset = col * ls.ExtraRowsPerColumnIncrease;
            //                     for (var row = 0; row < rows; row++)
            //                     {
            //                         var slot = c_board.Slots[row, col];
            //                         var spawnPosition = slot.WorldPosition + Vector3.up * (slotHeight * (basicOffset + columnsOffset)); 
            //                         //slot.SetRandomItem(c_board.ItemDatasSample, spawnPosition, _itemDataSet.Value.Set, ls, systems.GetWorld());
            //                         
            //                         slot.SetPresetItem(
            //                             c_board.ItemDatasSample, 
            //                             spawnPosition, 
            //                             _itemDataSet.Value.Set,
            //                             ls, 
            //                             systems.GetWorld(), 
            //                             _boardPresetter.Value.PresetBoard[row, col]);
            //                         
            //                         ref var c_newDrop = ref _dropPool.Value.Add(slot.AttachedItemEntity);
            //                         c_newDrop.Acceleration = ls.DropAcceleration;
            //                         c_newDrop.Speed = ls.DropSpeed;
            //                         c_newDrop.EndPoint = slot.WorldPosition;
            //                         c_newDrop.Delay = ls.NewItemsDropDelay 
            //                                           + ls.RowInBetweenDropDelay * (ls.ExtraRows + ls.BoardRows + columnsOffset - 1 - row)
            //                                           + ls.ColInBetweenDropDelay * col;
            //                     }
            //                     
            //                     var upperSlot = c_board.Slots[0, col];
            //                     for (var extraRow = 0; extraRow < extraRows + columnsOffset; extraRow++)
            //                     {
            //                         var extraOffset = Vector3.up * (slotHeight * (extraRow + topGap));
            //                         var extraSpawnPosition = upperSlot.WorldPosition + extraOffset;
            //                             
            //                         var randomIndex = Random.Range(0, samplableItemDatas.Count);
            //                         var randomItemPool = samplableItemDatas[randomIndex].Pool;
            //                         var randomItemEntity = randomItemPool.Get(extraSpawnPosition, Quaternion.identity);
            //                             
            //                         ref var c_transform = ref systems.GetWorld().GetPool<c_Transform>().Get(randomItemEntity);
            //                         c_transform.Transform.localScale = Vector3.one * ls.SymbolSize;
            //                         
            //                         ref var c_extraDrop = ref _dropPool.Value.Add(randomItemEntity);
            //                         c_extraDrop.Acceleration = ls.DropAcceleration;
            //                         c_extraDrop.Speed = ls.DropSpeed;
            //                         c_extraDrop.EndPoint = extraSpawnPosition + Vector3.down * (slotHeight * (topGap + ls.BoardRows + extraRows + columnsOffset));
            //                         c_extraDrop.Delay = ls.NewItemsDropDelay
            //                                             + ls.RowInBetweenDropDelay * extraRow
            //                                             + ls.ColInBetweenDropDelay * col;
            //                         c_extraDrop.IsToBeRemoved = true;
            //                     }
            //                 }
            //             }
            //             else
            //             {
            //                 var isSpinSpecial = (++c_board.SpinCount % ls.SpecialSpinNumber == 0);
            //                 var royalsAmount = isSpinSpecial 
            //                     ? ls.RoyalsInSpecialSpin 
            //                     : ls.RoyalsInSpin;
            //                 var commonsAmount = isSpinSpecial 
            //                     ? ls.CommonsInSpecialSpin 
            //                     : ls.CommonsInSpin;
            //
            //                 if (ls.DoUseItemsInSpinChances)
            //                 {
            //                     var commonItemsInSpin = RandomMethods
            //                         .GetSelectionArray(ls.ItemsInSpinChances, x => x.Weight)
            //                         .Random()!
            //                         .ItemsInSpin;
            //         
            //                     c_board.ItemDatasSample = c_board.AllItemTypesToTheirDatas
            //                         .GetSample(ls.RoyalsInSpin, commonItemsInSpin);
            //                 }
            //                 else
            //                 {
            //                     c_board.ItemDatasSample = c_board.AllItemTypesToTheirDatas
            //                         .GetSample(royalsAmount, commonsAmount);
            //                 }
            //
            //                 for (var row = 0; row < rows; row++)
            //                 {
            //                     for (var col = 0; col < columns; col++)
            //                     {
            //                         var slot = c_board.Slots[row, col];
            //                 
            //                         ref var c_oldDrop = ref _dropPool.Value.Add(slot.AttachedItemEntity);
            //                         c_oldDrop.Acceleration = ls.DropAcceleration;
            //                         c_oldDrop.Speed = ls.DropSpeed;
            //                         var dropDistance = Vector3.down * (2f * slotHeight); // 2 to avoid premature disappearance
            //                         c_oldDrop.EndPoint = slot.WorldPosition + dropDistance * ls.BoardRows;
            //                         c_oldDrop.Delay = ls.RowInBetweenDropDelay * (ls.BoardRows - 1 - row) 
            //                                           + ls.ColInBetweenDropDelay * col;
            //                         c_oldDrop.IsToBeRemoved = true;
            //                     }
            //                 }
            //
            //                 c_board.Slots.SetEachItemTypeFromSampleOnce(c_board.ItemDatasSample, systems.GetWorld(), ls, basicOffset, ls.ExtraRowsPerColumnIncrease);
            //                 for (var col = 0; col < columns; col++)
            //                 {
            //                     var colOff = col * ls.ExtraRowsPerColumnIncrease;
            //                     for (var row = 0; row < rows; row++)
            //                     {
            //                         var slot = c_board.Slots[row, col];
            //                         var spawnPosition = slot.WorldPosition + Vector3.up * (slotHeight * (basicOffset + colOff));
            //                 
            //                         if (!slot.isFirstPlacedOfSample)
            //                         {
            //                             slot.SetRandomItem(c_board.Slots, c_board.ItemDatasSample, spawnPosition, _itemDataSet.Value.Set, ls, systems.GetWorld());
            //                         }
            //                         
            //                         ref var c_newDrop = ref _dropPool.Value.Add(slot.AttachedItemEntity);
            //                         c_newDrop.Acceleration = ls.DropAcceleration;
            //                         c_newDrop.Speed = ls.DropSpeed;
            //                         c_newDrop.EndPoint = slot.WorldPosition;
            //                         c_newDrop.Delay = ls.NewItemsDropDelay 
            //                                           + ls.RowInBetweenDropDelay * (ls.ExtraRows + ls.BoardRows + colOff - 1 - row)
            //                                           + ls.ColInBetweenDropDelay * col;
            //                     }
            //                     
            //                     var upperSlot = c_board.Slots[0, col];
            //                     for (var extraRow = 0; extraRow < extraRows + colOff; extraRow++)
            //                     {
            //                         var extraOffset = Vector3.up * (slotHeight * (extraRow + topGap));
            //                         var extraSpawnPosition = upperSlot.WorldPosition + extraOffset;
            //                             
            //                         var randomIndex = Random.Range(0, samplableItemDatas.Count);
            //                         var randomItemPool = samplableItemDatas[randomIndex].Pool;
            //                         var randomItemEntity = randomItemPool.Get(extraSpawnPosition, Quaternion.identity);
            //                             
            //                         ref var c_transform = ref systems.GetWorld().GetPool<c_Transform>().Get(randomItemEntity);
            //                         c_transform.Transform.localScale = Vector3.one * ls.SymbolSize;
            //                         
            //                         ref var c_extraDrop = ref _dropPool.Value.Add(randomItemEntity);
            //                         c_extraDrop.Acceleration = ls.DropAcceleration;
            //                         c_extraDrop.Speed = ls.DropSpeed;
            //                         c_extraDrop.EndPoint = extraSpawnPosition + Vector3.down * (slotHeight * (topGap + ls.BoardRows + extraRows + colOff));
            //                         c_extraDrop.Delay = ls.NewItemsDropDelay
            //                                             + ls.RowInBetweenDropDelay * extraRow
            //                                             + ls.ColInBetweenDropDelay * col;
            //                         c_extraDrop.IsToBeRemoved = true;
            //                     }
            //                 }
            //             }
            //             
            //             c_cleanBoard.State = DropState.InProgress;
            //             break;
            //         }
            //         
            //         case DropState.InProgress:
            //         {
            //             #region Check if all drops have ended
            //             
            //             var hasDropEnded = true;
            //             for (var col = 0; col < columns; col++)
            //             {
            //                 var topItemEntity = c_board.Slots[0, col].AttachedItemEntity;
            //                 if (!_dropPool.Value.Has(topItemEntity)) continue;
            //                 hasDropEnded = false;
            //                 break;
            //             }
            //             if (hasDropEnded) c_cleanBoard.State = DropState.Finished;
            //             
            //             #endregion
            //             break;
            //         }
            //         
            //         case DropState.Finished:
            //         {
            //             #region Finish cleaning and check for accumulated bonuses (in this case, it's a proxy to move to comboSearch)
            //             
            //             _filter.Pools.Inc2.Del(entity);
            //             _noBonusesPool.Value.Del(entity);
            //             _noCombosLeftPool.Value.Del(entity);
            //             
            //             //Clean up in case screen clean was requested from betBonusScreen.
            //             if (_t2RiskBonusPool.Value.Has(entity)) _t2RiskBonusPool.Value.Del(entity);
            //
            //             ref var c_searchCombos = ref _searchCombosPool.Value.Add(entity);
            //             c_searchCombos.Delay = ls.AfterCleanUpDelay;
            //
            //             #endregion
            //             break;
            //         }
            //         
            //         default:
            //             throw new InvalidEnumArgumentException("Unknown board state!"); 
            //     }
            // }
        }
    }
}