using Code.Game.Features.CleanBoard;
using Code.Game.Features.RenewButtonStateChange;
using Code.Game.Hero;
using Code.MySubmodule.ECS.Features.RequestsToFeatures;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Interactables.RenewButton
{
    public sealed class i_RenewButton : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<c_Board>> _boardsFilter = default;

        private readonly EcsCustomInject<RenewButtonView> _renewButton = default;

        private readonly EcsWorldInject _world = default;

        public void Init(IEcsSystems systems)
        {
            _renewButton.Value.Init();
            _renewButton.Value.Button.onClick.AddListener(() =>
            {
                _world.Value.AddRequest(new r_ChangeCleanButtonState(false));
                
                foreach (var boardEntity in _boardsFilter.Value)
                {
                    ref var c_board = ref _boardsFilter.Pools.Inc1.Get(boardEntity);
                    if (c_board.IsRenewable)
                    {
                        var boardPacked = _world.Value.PackEntity(boardEntity);
                        _world.Value.AddRequest(new r_CleanBoard(boardPacked));
                    }
                }
            });
        }
    }
}