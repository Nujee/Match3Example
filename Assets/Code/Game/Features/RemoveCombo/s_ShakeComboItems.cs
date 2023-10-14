using Code.Game.Hero;
using Code.Game.Items;
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
        private readonly EcsPoolInject<c_Item> _itemPool = default;
        private readonly EcsPoolInject<c_Transform> _transformPool = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                ref var c_feature = ref _featureFilter.Pools.Inc2.Get(featureEntity);

                var shakeProgress = c_feature.ShakeDurationElapsed / c_feature.ShakeDurationTotal;
                c_feature.ShakeDurationElapsed += Time.deltaTime;
                
                foreach (var comboCellPacked in c_feature.ComboTypeToCellsPacked.cells)
                {
                    if (!comboCellPacked.Unpack(_world.Value, out var comboCellEntity)) { continue; }
                    ref var c_cell = ref _cellPool.Value.Get(comboCellEntity);
                    
                    if (!c_cell.AttachedItemPacked.Unpack(_world.Value, out var attachedItemEntity)) { continue; }
                    ref var c_item = ref _itemPool.Value.Get(attachedItemEntity);
                    ref var c_itemTransform = ref _transformPool.Value.Get(attachedItemEntity);

                    if (c_feature.ShakeDurationElapsed < c_feature.ShakeDurationTotal)
                    {
                        //TODO: do something with this square() - thing maybe?
                        var shift = c_feature.ShakeMagnitude * shakeProgress.Square() * (Vector3)Random.insideUnitCircle;
                        c_itemTransform.Value.position = c_cell.WorldPosition + shift;
                        //TODO: remove hardcoded values
                        var targetScale = Mathf.Lerp(1f, 0.9f, shakeProgress);
                        c_itemTransform.Value.localScale = targetScale * Vector3.one;
                    }
                    else
                    {
                        c_itemTransform.Value.position = c_cell.WorldPosition;
                        c_item.Data.Pool.Return(attachedItemEntity);
                        _featureFilter.Pools.Inc1.Del(featureEntity);
                    }
                }
            }
        }
    }
}