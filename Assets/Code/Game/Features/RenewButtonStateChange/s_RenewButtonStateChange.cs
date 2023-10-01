using Code.Game.Interactables;
using Code.Game.Interactables.RenewButton;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.RenewButtonStateChange
{
    public sealed class s_RenewButtonStateChange : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_ActivateRenewButton>> _activateButton = default;
        private readonly EcsFilterInject<Inc<r_DeactivateRenewButton>> _deactivateButton = default;
            
        private readonly EcsCustomInject<RenewButtonView> _cleanButton = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var activateRequestEntity in _activateButton.Value)
            {
                _cleanButton.Value.Button.interactable = true;
                _world.Value.DelEntity(activateRequestEntity);
            }
                
            foreach (var deactivateRequestEntity in _deactivateButton.Value)
            {
                _cleanButton.Value.Button.interactable = false;
                _world.Value.DelEntity(deactivateRequestEntity);
            }
        }
    }
}