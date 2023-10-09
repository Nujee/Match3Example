using System.Collections.Generic;
using Code.Game.Hero;
using Code.Game.Items;
using Code.Game.Main;
using Code.MySubmodule.ECS.Components.UnityComponents;
using Code.MySubmodule.ECS.Features.RequestTrain;
using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.RemoveCombo
{
    public sealed class s_ScatterNonComboItems : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Step<s_ScatterNonComboItems>, c_RemoveCombo>> _featureFilter = default;
        
        private readonly EcsPoolInject<c_Board> _boardPool = default;
        private readonly EcsPoolInject<c_Cell> _cellPool = default;
        private readonly EcsPoolInject<c_Item> _itemPool = default;
        private readonly EcsPoolInject<c_Transform> _transformPool = default;
        
        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;

        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                ref var c_feature = ref _featureFilter.Pools.Inc2.Get(featureEntity);
                
                if (!c_feature.BoardPacked.Unpack(_world.Value, out var boardEntity)) { continue; }
                ref var c_board = ref _boardPool.Value.Get(boardEntity);

                var comboEntitiesToPositions = new Dictionary<int, Vector3>();
                c_feature.ComboTypeToCellsPacked.cells.ForEach(x =>
                {
                    if (x.Unpack(_world.Value, out var comboEntity))
                    {
                        ref var c_comboCell = ref _cellPool.Value.Get(comboEntity);
                        comboEntitiesToPositions.Add(comboEntity, c_comboCell.WorldPosition);
                    }
                });
                
                foreach (var cellPacked in c_board.CellsPacked)
                {
                    var isCellEntityAlive = cellPacked.Unpack(_world.Value, out var cellEntity);
                    var isCellInCombo = comboEntitiesToPositions.ContainsKey(cellEntity);
                    
                    if (!isCellEntityAlive || isCellInCombo) { continue; }
                    ref var c_cell = ref _cellPool.Value.Get(cellEntity);
                        
                    if (!c_cell.AttachedItemPacked.Unpack(_world.Value, out var itemEntity)) { continue; }
                    ref var c_item = ref _itemPool.Value.Get(itemEntity);
                    ref var c_itemTransform = ref _transformPool.Value.Get(itemEntity);

                    var netForce = Vector3.zero;
                    foreach (var comboPosition in comboEntitiesToPositions.Values)
                    {
                        var partialForceVector = comboPosition - c_cell.WorldPosition;
                        var partialForceUnscaled = partialForceVector / partialForceVector.sqrMagnitude;
                        netForce += partialForceUnscaled * _levelSettings.Value.ScatterForce;
                    }
                    
                    c_itemTransform.Value.DOPunchPosition(netForce, _levelSettings.Value.ScatterDuration);
                }
                
                _featureFilter.Pools.Inc1.Del(featureEntity);
            }
        }
    }
}