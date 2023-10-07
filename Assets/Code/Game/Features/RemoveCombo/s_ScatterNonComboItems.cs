using Code.MySubmodule.ECS.Features.RequestTrain;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.RemoveCombo
{
    public sealed class s_ScatterNonComboItems : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Step<s_ScatterNonComboItems>>> _featureFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureEntity in _featureFilter.Value)
            {
                
                
                _featureFilter.Pools.Inc1.Del(featureEntity);
            }
        }
    }
}