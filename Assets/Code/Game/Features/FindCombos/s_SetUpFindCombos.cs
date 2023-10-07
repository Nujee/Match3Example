using Code.Game.Main;
using Code.MySubmodule.DebugTools.MyLogger;
using Code.MySubmodule.ECS.Features.RequestTrain;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Game.Features.FindCombos
{
    public sealed class s_SetUpFindCombos : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<r_FindCombos>> _featureRequestFilter = default;

        private readonly EcsPoolInject<c_FindCombos> _findCombosPool = default;
        
        private readonly EcsCustomInject<LevelSettings> _levelSettings = default;

        private readonly EcsWorldInject _world = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var featureRequest in _featureRequestFilter.Value)
            {
                ref var r_feature = ref _featureRequestFilter.Pools.Inc1.Get(featureRequest);
                if (r_feature.BoardPacked.Unpack(_world.Value, out _))
                {
                    "Am I ehre?".Log();
                    var featureEntity = _world.Value.NewEntity();
                    ref var c_feature = ref _findCombosPool.Value.Add(featureEntity);
                
                    c_feature.BoardPacked = r_feature.BoardPacked;
                    
                    var myTrain = systems.AddTrainTo(featureEntity);
                    myTrain.AddStep<s_ProcessFindCombos>(_levelSettings.Value.PreFindCombosDelay);

                    _world.Value.DelEntity(featureRequest);
                }
            }
        }
    }
}