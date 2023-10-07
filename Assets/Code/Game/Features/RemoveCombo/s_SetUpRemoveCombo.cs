using Code.Game.Utils;
using Code.MySubmodule.DebugTools.MyLogger;
using Code.MySubmodule.ECS.Components.UnityComponents;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.RemoveCombo
{
    public sealed class s_SetUpRemoveCombo : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_RemoveCombo>> _removeComboFilter = default;

        private readonly EcsWorldInject _world = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _removeComboFilter.Value)
            {
                ref var r_feature = ref _removeComboFilter.Pools.Inc1.Get(featureRequest);
                
                if (r_feature.RemoveDelay > 0f)
                {
                    r_feature.RemoveDelay -= Time.deltaTime;
                    r_feature.RemoveDelay.Log();
                }
                else
                {
                    foreach (var cellPacked in r_feature.ComboTypeToCellsPacked.cells)
                    {
                        ref var c_item = ref cellPacked.GetAttachedItem(_world.Value, out var itemEntity);
                        ref var c_itemTransform = ref _world.Value.GetPool<c_Transform>().Get(itemEntity);

                        c_itemTransform.Value.position += (Vector3.up + Vector3.right) * 5f;

                        // c_item.Data.Pool.Return(itemEntity);
                    }
                    
                    _world.Value.DelEntity(featureRequest);
                }
            }
        }
    }
}