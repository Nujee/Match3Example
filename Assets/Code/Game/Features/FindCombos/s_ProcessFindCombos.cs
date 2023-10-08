using System.Collections.Generic;
using System.Linq;
using Code.Game.Features.CleanButtonStateChange;
using Code.Game.Features.RemoveCombo;
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

                    _world.Value.AddRequest(new r_RemoveCombo(largestCombo));
                }
                else
                {
                    _world.Value.AddRequest(new r_ChangeCleanButtonState(true));
                }
                
                _world.Value.DelEntity(featureEntity);
            }
        }
        
        private List<(ItemType type, List<EcsPackedEntity> comboCells)> GetAllCombos(EcsPackedEntity[,] cells)
        {
            var allCombos = new List<(ItemType, List<EcsPackedEntity>)>();
            
            var rowCombos = GetCombosInDimension(true);
            var columnCombos = GetCombosInDimension(false);
            
            allCombos.AddRange(rowCombos);
            allCombos.AddRange(columnCombos);
        
            return allCombos;
            
            IEnumerable<(ItemType, List<EcsPackedEntity>)> GetCombosInDimension(bool isRowSearch)
            {
                var combosInDimension = new List<(ItemType , List<EcsPackedEntity>)>();
            
                var searchDimensionSize = cells.GetLength(isRowSearch ? 0 : 1);
                var otherDimensionSize = cells.GetLength(isRowSearch ? 1 : 0);
                
                for (var dimensionArrayIndex = 0; dimensionArrayIndex < searchDimensionSize; dimensionArrayIndex++)
                {
                    var allCombosInArray = GetCombosInArray(dimensionArrayIndex);
                    combosInDimension.AddRange(allCombosInArray);
                }
            
                return combosInDimension;

                IEnumerable<(ItemType, List<EcsPackedEntity>)> GetCombosInArray(int dimensionArrayIndex)
                {
                    var combosInArray = new List<(ItemType, List<EcsPackedEntity>)>();
                    
                    for (var comboBeginIndex = 0; comboBeginIndex < otherDimensionSize - 1; comboBeginIndex++)
                    {
                        var firstCellInCombo = isRowSearch
                            ? cells[dimensionArrayIndex, comboBeginIndex]
                            : cells[comboBeginIndex, dimensionArrayIndex];

                        var currentCombo = new List<EcsPackedEntity>();
                        var currentCellType = firstCellInCombo.GetAttachedItem(_world.Value).Data.Type;
                        var wildsTailLength = 0;// currentCellType == ItemType.None ? 1 : 0;
                        currentCombo.Add(firstCellInCombo);
                        
                        for (var comboNextIndex = comboBeginIndex + 1; comboNextIndex < otherDimensionSize; comboNextIndex++)
                        {
                            var nextCellInCombo = isRowSearch
                                ? cells[dimensionArrayIndex, comboNextIndex]
                                : cells[comboNextIndex, dimensionArrayIndex];

                            var nextCellType = nextCellInCombo.GetAttachedItem(_world.Value).Data.Type;
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