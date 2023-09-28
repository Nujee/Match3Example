using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.BoardRenewal
{
    public sealed class s_SetUpBoardRenewal : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_RenewBoard>> _featureRequests = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequests.Value)
            {
                var featureEntity = _world.Value.NewEntity();
                
                
                _world.Value.DelEntity(featureRequest);
            }
        }
    }
}