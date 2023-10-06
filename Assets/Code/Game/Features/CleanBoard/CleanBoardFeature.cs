using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.CleanBoard
{
    public sealed class CleanBoardFeature : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_SetUpCleanBoard());
            systems.Add(new s_ProcessCleanBoard());
        }
    }
}