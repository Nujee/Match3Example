using Code.Game.Features.DropItems;
using Code.Game.Features.RenewButtonStateChange;
using Code.Game.Hero;
using Code.MySubmodule.ECS.Features.RequestsToFeatures;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Interactables.RenewButton
{
    public sealed class i_RenewButton : IEcsInitSystem
    {
        //TODO: rewrite cuz temp
        private readonly EcsFilterInject<Inc<c_Board>> _boardFilter = default;

        private readonly EcsPoolInject<r_DropItems> _renewBoardRequestPool = default;

        private readonly EcsCustomInject<RenewButtonView> _renewButton = default;

        private readonly EcsWorldInject _world = default;

        public void Init(IEcsSystems systems)
        {
            _renewButton.Value.Init();
            _renewButton.Value.Button.onClick.AddListener(() =>
            {
                _world.Value.AddRequest(new r_ChangeRenewButtonState(false));

                //_world.Value.AddRequest<r_DropItems());
                
                var renewBoardRequest = _world.Value.NewEntity();
                ref var r_renewBoard = ref _renewBoardRequestPool.Value.Add(renewBoardRequest);
                //TODO: rewrite cuz temp
                foreach (var boardEntity in _boardFilter.Value)
                {
                    r_renewBoard.BoardPacked = _world.Value.PackEntity(boardEntity);
                    break;
                }
            });
        }
    }
}