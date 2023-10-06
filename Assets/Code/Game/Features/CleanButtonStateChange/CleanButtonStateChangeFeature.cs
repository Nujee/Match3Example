using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.CleanButtonStateChange
{
    public sealed class CleanButtonStateChangeFeature : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_CleanButtonStateChange());
        }
    }
}