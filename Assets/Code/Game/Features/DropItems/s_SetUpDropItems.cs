using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.DropItems
{
    public sealed class s_SetUpDropItems : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_DropItems>> _featureRequestFilter = default;
        
        private readonly EcsPoolInject<c_DropItems> _featurePool = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestFilter.Value)
            {
                ref var r_feature = ref _featureRequestFilter.Pools.Inc1.Get(featureRequest);
                
                var featureEntity = _world.Value.NewEntity();
                ref var c_feature = ref _featurePool.Value.Add(featureEntity);
                
                c_feature.BoardPacked = r_feature.BoardPacked;
                c_feature.DropDataList = r_feature.DropDataList;
                
                _world.Value.DelEntity(featureRequest);
            }
        }
    }
}