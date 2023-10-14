using System.Collections.Generic;
using Code.Game.Hero;
using Code.Game.Main;
using Code.Game.Utils;
using Code.MySubmodule.DebugTools.MyLogger;
using Code.MySubmodule.ECS.Components.UnityComponents;
using Code.MySubmodule.ECS.Features.RequestTrain;
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
        private readonly EcsPoolInject<c_Transform> _transformPool = default;
        
        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                var world = systems.GetWorld();
                var ls = _levelSettings.Value;
                
                ref var c_feature = ref _featureFilter.Pools.Inc2.Get(featureEntity);
                
                if (!c_feature.BoardPacked.Unpack(world, out var boardEntity)) { continue; }
                ref var c_board = ref _boardPool.Value.Get(boardEntity);

                var comboEntitiesToPositions = new Dictionary<int, Vector3>();
                foreach (var cellPacked in c_feature.ComboTypeToCellsPacked.cells)
                {
                    if (!cellPacked.Unpack(world, out var comboEntity)) { continue; }
                    ref var c_comboCell = ref _cellPool.Value.Get(comboEntity);
                    comboEntitiesToPositions.Add(comboEntity, c_comboCell.WorldPosition);
                }

                foreach (var cellPacked in c_board.CellsPacked)
                {
                    var isCellEntityAlive = cellPacked.Unpack(world, out var cellEntity);
                    var isCellInCombo = comboEntitiesToPositions.ContainsKey(cellEntity);
                    
                    if (!isCellEntityAlive || isCellInCombo) { continue; }
                    ref var c_cell = ref _cellPool.Value.Get(cellEntity);
                        
                    if (!c_cell.AttachedItemPacked.Unpack(world, out var itemEntity)) { continue; }
                    ref var c_itemTransform = ref _transformPool.Value.Get(itemEntity);
                    
                    // calculate scatter net force
                    var netForce = Vector3.zero;
                    foreach (var comboPosition in comboEntitiesToPositions.Values)
                    {
                        var partialForceVector = c_cell.WorldPosition - comboPosition;
                        var partialForceUnit = partialForceVector / partialForceVector.sqrMagnitude;
                        netForce += partialForceUnit;
                    }
                    netForce *= ls.ScatterForce;
                    
                    c_itemTransform.Value.DOPunchPosition(netForce, ls.ScatterTween);
                }
                
                _featureFilter.Pools.Inc1.Del(featureEntity);
            }
        }
    }
}