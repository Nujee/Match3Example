using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Features.RenewButtonStateChange;
using Code.Game.Hero;
using Code.Game.Items;
using Code.Game.Main;
using Code.Game.Utils;
using Code.MySubmodule.ECS.Features.RequestsToFeatures;
using Code.MySubmodule.ECS.Features.RequestTrain;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.FindCombos
{
    public sealed class s_ProcessFindCombos : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Step<s_ProcessFindCombos>, c_FindCombos>> _featureFilter = default;
        
        private readonly EcsPoolInject<c_Board> _boardPool = default;

        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;

        private readonly EcsWorldInject _world = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                ref var c_feature = ref _featureFilter.Pools.Inc2.Get(featureEntity);

                if (!c_feature.BoardPacked.Unpack(_world.Value, out var boardEntity)) continue;
                ref var c_board = ref _boardPool.Value.Get(boardEntity);

                var allCombos = GetAllCombos(c_board.CellsPacked);
        
                if (allCombos.Count > 0)
                {
                    var largestCombo = allCombos
                        .OrderByDescending(x => x.comboCells.Count)
                        .First();

                    // call some "remove combo" - feature request
                }
                else
                {
                    _world.Value.AddRequest(new r_ChangeRenewButtonState(true));
                }
                
                _featureFilter.Pools.Inc1.Del(featureEntity);
            }
        }
        
        private List<(ItemType type, List<EcsPackedEntity> comboCells)> GetAllCombos(EcsPackedEntity[,] cells)
        {
            var allCombos = new List<(ItemType, List<EcsPackedEntity>)>();
            
            var rowCombos = GetCombosInDimension(BoardDimension.Row);
            var columnCombos = GetCombosInDimension(BoardDimension.Column);
            
            allCombos.AddRange(rowCombos);
            allCombos.AddRange(columnCombos);
        
            return allCombos;
            
            IEnumerable<(ItemType, List<EcsPackedEntity>)> GetCombosInDimension(BoardDimension dimension)
            {
                var combosInDimension = new List<(ItemType , List<EcsPackedEntity>)>();
            
                var dimensionSize = cells.GetLength((int)dimension);
                for (var dimensionArrayIndex = 0; dimensionArrayIndex < dimensionSize; dimensionArrayIndex++)
                {
                    var allCombosInArray = GetCombosInArray(dimensionArrayIndex);
                    combosInDimension.AddRange(allCombosInArray);
                }
            
                return combosInDimension;

                IEnumerable<(ItemType, List<EcsPackedEntity>)> GetCombosInArray(int dimensionArrayIndex)
                {
                    var combosInArray = new List<(ItemType, List<EcsPackedEntity>)>();
            
                    var dimensionSize = cells.GetLength((int)dimension);
                    for (var comboBeginIndex = 0; comboBeginIndex < dimensionSize - 1; comboBeginIndex++)
                    {
                        var firstCellInCombo = dimension switch
                        {
                            BoardDimension.Row => cells[dimensionArrayIndex, comboBeginIndex],
                            BoardDimension.Column => cells[comboBeginIndex, dimensionArrayIndex],
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        
                        var currentCombo = new List<EcsPackedEntity>();

                        var currentCellType = _world.Value.GetAttachedItemType(firstCellInCombo);
                        var wildsTailLength = currentCellType == ItemType.None ? 1 : 0;
                        currentCombo.Add(firstCellInCombo);
                        
                        for (var comboNextIndex = comboBeginIndex + 1; comboNextIndex < dimensionSize; comboNextIndex++)
                        {
                            var nextCellInCombo = dimension switch
                            {
                                BoardDimension.Row => cells[dimensionArrayIndex, comboNextIndex],
                                BoardDimension.Column => cells[comboNextIndex, dimensionArrayIndex],
                                _ => throw new ArgumentOutOfRangeException()
                            };
                
                            var nextCellType = _world.Value.GetAttachedItemType(nextCellInCombo);
                            var isCurrentTypeWild = (currentCellType == ItemType.None);
                            var isNextOfCurrentType = (nextCellType == currentCellType);
                            var isNextWild = (nextCellType == ItemType.None); 
                
                            if (!isCurrentTypeWild && !isNextOfCurrentType && !isNextWild)
                            {
                                comboBeginIndex = (comboNextIndex - wildsTailLength) - 1;
                                break;
                            }
                            wildsTailLength = isNextWild ? (wildsTailLength + 1) : 0;
                            if (isCurrentTypeWild) { currentCellType = nextCellType; }
                            currentCombo.Add(nextCellInCombo);
                        }
                        
                        if (currentCombo.Count >= _levelSettings.Value.MinComboSize)
                        {
                            combosInArray.Add((currentCellType, currentCombo));
                        }
                    }
                
                    return combosInArray;
                }
            }
        }
    }
}