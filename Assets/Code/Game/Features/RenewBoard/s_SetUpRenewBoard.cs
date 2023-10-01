using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.RenewBoard
{
    public sealed class s_SetUpRenewBoard : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_RenewBoard>> _featureRequestsFilter = default;

        private readonly EcsPoolInject<c_RenewBoard> _featurePool = default;
        
        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestsFilter.Value)
            {
                ref var r_featureRequest = ref _featureRequestsFilter.Pools.Inc1.Get(featureRequest);

                var featureEntity = _world.Value.NewEntity();
                ref var c_feature = ref _featurePool.Value.Add(featureEntity);

                if (!r_featureRequest.BoardToRenewPacked.Unpack(_world.Value, out _)) { continue; }
                c_feature.BoardPacked = r_featureRequest.BoardToRenewPacked;

                // for each slot in board
                // take slot's item and  untie it from slot
                // set position where old item will fall to
                // set random new item for slot
                // set position where new item will fall from
                // add n * _delay to each item, starting from 0-delay to left bottom corner,
                // and +1 * _delay with each new row and column.
                // Each item will signal when it fell into its place
                // When all items fell into their places, signal that board is renewed
                
                _world.Value.DelEntity(featureRequest);
            }
        }
    }
}