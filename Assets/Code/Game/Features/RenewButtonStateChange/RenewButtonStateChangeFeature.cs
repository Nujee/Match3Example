using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.RenewButtonStateChange
{
    public sealed class RenewButtonStateChangeFeature : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_RenewButtonStateChange());
        }
    }
}