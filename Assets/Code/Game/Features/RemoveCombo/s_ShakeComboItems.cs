using Code.Game.Hero;
using Code.Game.Utils;
using Code.MySubmodule.DebugTools.MyLogger;
using Code.MySubmodule.ECS.Components.UnityComponents;
using Code.MySubmodule.ECS.Features.RequestTrain;
using Code.MySubmodule.Math;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.RemoveCombo
{
    public sealed class s_ShakeComboItems : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Step<s_ShakeComboItems>, c_RemoveCombo>> _featureFilter = default;
        
        private readonly EcsPoolInject<c_Cell> _cellPool = default;
        private readonly EcsPoolInject<c_Transform> _transformPool = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                ref var c_feature = ref _featureFilter.Pools.Inc2.Get(featureEntity);
                foreach (var comboCellPacked in c_feature.ComboTypeToCellsPacked.cells)
                {
                    if (!comboCellPacked.Unpack(_world.Value, out var comboCellEntity)) { continue; }
                    ref var c_cell = ref _cellPool.Value.Get(comboCellEntity);
                    
                    ref var c_item = ref comboCellPacked.GetAttachedItem(_world.Value, out var itemEntity);
                    ref var c_itemTransform = ref _transformPool.Value.Get(itemEntity);
                    
                    if (c_feature.ShakeDurationElapsed < c_feature.ShakeDurationTotal)
                    {
                        c_feature.ShakeDurationElapsed += Time.deltaTime;
                        
                        var currentProgress = c_feature.ShakeDurationElapsed / c_feature.ShakeDurationTotal;
                        var shift = c_feature.ShakeMagnitude * currentProgress * (Vector3)Random.insideUnitCircle;
                        c_itemTransform.Value.position = c_cell.WorldPosition + shift;
                    }
                    else
                    {
                        c_itemTransform.Value.position = c_cell.WorldPosition;
                        c_item.Data.Pool.Return(itemEntity);
                        _featureFilter.Pools.Inc1.Del(featureEntity);
                    }
                }
            }
        }
    }
}