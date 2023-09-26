using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.BoardRenewal
{
    public class BoardRenewalFeature : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_RenewBoard());
        }
    }
}