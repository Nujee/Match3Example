using Code.Game.Main;
using Code.MySubmodule.ECS.Features.RequestTrain;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.RemoveCombo
{
    public sealed class s_SetUpRemoveCombo : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_RemoveCombo>> _featureRequestFilter = default;
        
        private readonly EcsPoolInject<c_RemoveCombo> _removeComboPool = default;
        
        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;

        private readonly EcsWorldInject _world = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestFilter.Value)
            {
                var ls = _levelSettings.Value;
                
                ref var r_feature = ref _featureRequestFilter.Pools.Inc1.Get(featureRequest);
                
                var featureEntity = _world.Value.NewEntity();
                ref var c_feature  = ref _removeComboPool.Value.Add(featureEntity);
                
                c_feature.ComboTypeToCellsPacked = r_feature.ComboTypeToCellsPacked;
                c_feature.ShakeDurationTotal = ls.ShakeComboDuration;
                c_feature.ShakeMagnitude = ls.ShakeComboMagnitude;

                var comboRemoveTrain = systems.AddTrainTo(featureEntity);
                comboRemoveTrain.AddStep<s_ShakeComboItems>(ls.PreRemoveComboDelay);
                comboRemoveTrain.AddStep<s_ExplodeComboItems>(ls.ShakeComboDuration);
                comboRemoveTrain.AddStep<s_ScatterNonComboItems>();
                
                // if (r_feature.RemoveDelay > 0f)
                // {
                //     r_feature.RemoveDelay -= Time.deltaTime;
                // }
                // else
                // {
                //     foreach (var cellPacked in r_feature.ComboTypeToCellsPacked.cells)
                //     {
                //         ref var c_item = ref cellPacked.GetAttachedItem(_world.Value, out var itemEntity);
                //         c_item.Data.Pool.Return(itemEntity);
                //     }
                //     _world.Value.DelEntity(featureRequest);
                // }
            }
        }
    }
}