using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.TopUp
{
    public sealed class s_SetUpTopUp : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_TopUp>> _featureRequestFilter = default;
        
        private readonly EcsPoolInject<c_TopUp> _topUpPool = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestFilter.Value)
            {
                var featureEntity = _world.Value.NewEntity();
                ref var c_feature  = ref _topUpPool.Value.Add(featureEntity);
                
                
                
                _world.Value.DelEntity(featureRequest);
            }
        }
    }
}