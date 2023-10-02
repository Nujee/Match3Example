using Code.Game.Main;
using Code.MySubmodule.ECS.Features.RequestTrain;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.FindCombos
{
    public sealed class s_SetUpFindCombos : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_FindCombos>> _findCombosFilter = default;
        
        private readonly EcsPoolInject<c_FindCombos> _findCombosPool = default;
        
        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;

        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _findCombosFilter.Value)
            {
                var featureEntity = _world.Value.NewEntity();
                _findCombosPool.Value.Add(featureEntity);

                var myTrain = systems.AddTrainTo(featureEntity);
                myTrain.AddStep<s_ProcessFindCombos>(_levelSettings.Value.PreFindCombosDelay);

                _world.Value.DelEntity(featureRequest);
            }
        }
    }
}