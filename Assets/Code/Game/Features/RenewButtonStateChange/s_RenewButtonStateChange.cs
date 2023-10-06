using Code.Game.Interactables.RenewButton;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.RenewButtonStateChange
{
    public sealed class s_RenewButtonStateChange : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_ChangeRenewButtonState>> featureRequestFilter = default;

        private readonly EcsCustomInject<RenewButtonView> _cleanButton = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in featureRequestFilter.Value)
            {
                ref var r_feature = ref featureRequestFilter.Pools.Inc1.Get(featureRequest);
                
                _cleanButton.Value.Button.interactable = r_feature.IsActive;
                _world.Value.DelEntity(featureRequest);
            }
        }
    }
}