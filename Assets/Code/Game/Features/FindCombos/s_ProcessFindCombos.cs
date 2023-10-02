using System;
using System.Collections.Generic;
using System.Linq;
using Code.Game.Hero;
using Code.Game.Items;
using Code.MySubmodule.ECS.Features.RequestTrain;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEditor.Graphs;

namespace Code.Game.Features.FindCombos
{
    public sealed class s_ProcessFindCombos : IEcsRunSystem
    {
        // private readonly EcsFilterInject<Inc<Step<s_ProcessFindCombos>, c_FindCombos>> _featureFilter = default;
        //
        // private readonly EcsWorldInject _world = default;
        //
        // public void Run(IEcsSystems systems)
        // {
        //     foreach (var featureEntity in _featureFilter.Value)
        //     {
        //         var allCombos = GetAllCombos(c_board.Slots);
        //
        //         if (allCombos.Count > 0)
        //         {
        //             var bigBonusComboSlots = allCombos
        //                 .FirstOrDefault(x => x.type == ItemType.BigBonusItem)
        //                 .slots;
        //             
        //             var largestCombo = allCombos
        //                 .OrderByDescending(x => x.slots.Count)
        //                 .First();
        //
        //             (ItemType type, List<Slot> slots) comboToRemove = (bigBonusComboSlots != null)
        //                 ? (ItemType.BigBonusItem, bigBonusComboSlots)
        //                 : largestCombo;
        //             
        //             ref var c_removeCombo = ref _removeComboPool.Value.Add(entity);
        //             c_removeCombo.SlotsToRemove = comboToRemove;
        //         }
        //         
        //         else if (!_noCombosLeft.Value.Has(entity))
        //         {
        //             _noCombosLeft.Value.Add(entity);
        //             _searchForDicesPool.Value.Add(entity);
        //         }
        //         
        //         _featureFilter.Pools.Inc1.Del(featureEntity);
        //     }
        // }
        //
        // private List<(ItemType type, List<Slot> slots)> GetAllCombos(Slot[,] slots)
        // {
        //     var result = new List<(ItemType type, List<Slot> slots)>();
        //
        //     // (!!!) algorithm only for 1-row-and-1-column combos
        //     var rowCombos = GetCombosInDimension(slots, BoardDimension.Row);
        //     var columnCombos = GetCombosInDimension(slots, BoardDimension.Column);
        //
        //     result.AddRange(rowCombos);
        //     result.AddRange(columnCombos);
        //
        //     return result;
        // }
        //
        // private IEnumerable<(ItemType type, List<Slot> slots)> GetCombosInDimension(Slot[,] slots, BoardDimension dimension)
        // {
        //     var result = new List<(ItemType type, List<Slot> slots)>();
        //     var dimensionSize = slots.GetLength((int)dimension);
        //     for (var dimensionArrayIndex = 0; dimensionArrayIndex < dimensionSize; dimensionArrayIndex++)
        //     {
        //         var allCombosInArray = GetCombosInArray(slots, dimensionArrayIndex, dimension);
        //         result.AddRange(allCombosInArray);
        //     }
        //     
        //     return result;
        // }
        //
        // private IEnumerable<(ItemType type, List<Slot> slots)> GetCombosInArray(Slot[,] slots, int dimensionArrayIndex, BoardDimension dimension)
        // {
        //     var result = new List<(ItemType type, List<Slot> slots)>();
        //
        //     // TODO: refactor '1 - (int)dimension' to reduce confusion
        //     var dimensionLength = slots.GetLength(1 - (int)dimension);
        //     for (var comboFirstIndex = 0; comboFirstIndex < dimensionLength - 1; comboFirstIndex++)
        //     {
        //         var firstSlotInCombo = dimension switch
        //         {
        //             BoardDimension.Row => slots[dimensionArrayIndex, comboFirstIndex],
        //             BoardDimension.Column => slots[comboFirstIndex, dimensionArrayIndex],
        //             _ => throw new ArgumentOutOfRangeException()
        //         };
        //         
        //         var currentCombo = new List<Slot>();
        //         var currentType = firstSlotInCombo.AttachedItemType;
        //         var wildsTailLength = currentType == ItemType.Wild ? 1 : 0;
        //         currentCombo.Add(firstSlotInCombo);
        //         
        //         for (var comboNextIndex = comboFirstIndex + 1; comboNextIndex < dimensionLength; comboNextIndex++)
        //         {
        //             var nextSlotInCombo = dimension switch
        //             {
        //                 BoardDimension.Row => slots[dimensionArrayIndex, comboNextIndex],
        //                 BoardDimension.Column => slots[comboNextIndex, dimensionArrayIndex],
        //                 _ => throw new ArgumentOutOfRangeException()
        //             };
        //
        //             var isCurrentTypeWild = (currentType == ItemType.Wild); // <=> "is combo type determined yet?"
        //             var isNextOfCurrentType = (nextSlotInCombo.AttachedItemType == currentType);
        //             var isNextWild = (nextSlotInCombo.AttachedItemType == ItemType.Wild); 
        //
        //             if (!isCurrentTypeWild && !isNextOfCurrentType && !isNextWild)
        //             {
        //                 comboFirstIndex = (comboNextIndex - wildsTailLength) - 1;
        //                 break;
        //             }
        //             wildsTailLength = isNextWild ? (wildsTailLength + 1) : 0;
        //             if (isCurrentTypeWild) { currentType = nextSlotInCombo.AttachedItemType; }
        //             currentCombo.Add(nextSlotInCombo);
        //         }
        //
        //         // checking if combo is big enough and award for it exists
        //         // TODO: refactor: no need to check if bigger than min combo size, must only ensure that award in ItemDataSet exists
        //         var comboItemData = _itemDataSet.Value.Set.FirstOrDefault(itemData =>
        //         {
        //             var isOfNeededType = itemData.Type == currentType;
        //             var isComboSizePossible = itemData.Award.Any(comboAward => comboAward.ComboSize == currentCombo.Count);
        //             
        //             return isOfNeededType && isComboSizePossible;
        //         });
        //         if (comboItemData != null && currentCombo.Count >= comboItemData.MinComboSize)
        //         {
        //             result.Add((currentType, currentCombo));
        //         }
        //     }
        //
        //     return result;
        // }
        public void Run(IEcsSystems systems)
        {
            throw new NotImplementedException();
        }
    }
}