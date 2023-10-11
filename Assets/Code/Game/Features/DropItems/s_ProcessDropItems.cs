using Code.Game.Features.FindCombos;
using Code.Game.Items;
using Code.Game.Main;
using Code.Game.Utils;
using Code.MySubmodule.ECS.Components.UnityComponents;
using Code.MySubmodule.ECS.Features.RequestsToFeatures;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.DropItems
{
    public sealed class s_ProcessDropItems : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_DropItems>> _featureFilter = default;
        
        private readonly EcsPoolInject<c_Item> _itemPool = default;
        private readonly EcsPoolInject<c_Transform> _transformPool = default;

        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                var world = systems.GetWorld();
                var ls = _levelSettings.Value;
                
                ref var c_feature = ref _featureFilter.Pools.Inc1.Get(featureEntity);

                for (var i = 0; i < c_feature.DropDataList.Count; i++)
                {
                    var dropData = c_feature.DropDataList[i];
                    
                    if (!dropData.ItemPacked.Unpack(world, out var itemEntity)) { continue; }
                    ref var c_item = ref _itemPool.Value.Get(itemEntity);
                    ref var c_itemTransform = ref _transformPool.Value.Get(itemEntity);
                    
                    dropData.Delay -= Time.deltaTime;
                    if (dropData.Delay > 0f) { continue; }

                    var targetSpeed = dropData.Speed + ls.DropItemsAcceleration * Time.deltaTime;
                    dropData.Speed = Mathf.Clamp(targetSpeed, 0f, ls.DropItemsMaxSpeed);
                    var frameShift = dropData.Speed * Time.deltaTime * Vector3.down;
                    var newPosition = c_itemTransform.Value.position + frameShift;
                    
                    if (newPosition.y < dropData.TargetPosition.y)
                    {
                        if (dropData.IsDisposable)
                        {
                            c_item.Data.Pool.Return(itemEntity);
                        }
                        else
                        {
                            c_itemTransform.Value.position = dropData.TargetPosition;
                            c_itemTransform.Value.DOPunchPosition(Vector3.down * ls.LandDrawdown, ls.LandTween);
                        }
                        c_feature.DropDataList.Remove(dropData);
                    }
                    else
                    {
                        c_itemTransform.Value.position = newPosition;
                    }
                }

                if (c_feature.DropDataList.Count == 0)
                {
                    world.AddRequest(new r_FindCombos(c_feature.BoardPacked));
                    world.DelEntity(featureEntity);
                }
            }
        }
    }
}