using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.DropItems
{
    public sealed class s_SetUpDropItems : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_DropItems>> _featureRequestFilter = default;
        
        private readonly EcsPoolInject<c_DropItems> _featurePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestFilter.Value)
            {
                var world = systems.GetWorld();
                
                ref var r_feature = ref _featureRequestFilter.Pools.Inc1.Get(featureRequest);
                
                var featureEntity = world.NewEntity();
                ref var c_feature = ref _featurePool.Value.Add(featureEntity);
                
                c_feature.BoardPacked = r_feature.BoardPacked;
                c_feature.DropDataList = r_feature.DropDataList;
                
                world.DelEntity(featureRequest);
            }
        }
    }
}