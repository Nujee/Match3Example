using Code.Game.Features.FindCombos;
using Code.Game.Items;
using Code.Game.Main;
using Code.MySubmodule.DebugTools.MyLogger;
using Code.MySubmodule.ECS.Components.UnityComponents;
using Code.MySubmodule.ECS.Features.RequestsToFeatures;
using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.Features.CleanBoard
{
    public sealed class s_ProcessCleanBoard : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_CleanBoard>> _dropItemsFilter = default;
        
        private readonly EcsPoolInject<c_Item> _itemPool = default;
        private readonly EcsPoolInject<c_Transform> _transformPool = default;

        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;

        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _dropItemsFilter.Value)
            {
                var ls = _levelSettings.Value;
                
                ref var c_feature = ref _dropItemsFilter.Pools.Inc1.Get(featureEntity);

                for (var i = 0; i < c_feature.DropDataList.Count; i++)
                {
                    var dropData = c_feature.DropDataList[i];
                    
                    if (!dropData.ItemPacked.Unpack(_world.Value, out var itemEntity)) { continue; }
                    ref var c_item = ref _itemPool.Value.Get(itemEntity);
                    ref var c_itemTransform = ref _transformPool.Value.Get(itemEntity);
                    
                    dropData.Delay -= Time.deltaTime;
                    if (dropData.Delay > 0f) { continue; }

                    var targetSpeed = dropData.Speed + _levelSettings.Value.DropItemsAcceleration * Time.deltaTime;
                    dropData.Speed = Mathf.Clamp(targetSpeed, 0f, _levelSettings.Value.DropItemsMaxSpeed);
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
                            c_itemTransform.Value.DOPunchPosition(Vector3.down * ls.LandDrawdown, 
                                ls.LandDuration, ls.LandVibrato, ls.LandElasticity);
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
                    "Do I End process clean board?".Log();
                    _world.Value.AddRequest(new r_FindCombos(c_feature.BoardPacked));
                    _world.Value.DelEntity(featureEntity);
                }
            }
        }
    }
}