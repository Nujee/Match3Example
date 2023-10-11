using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.TopUp
{
    public sealed class f_TopUp : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_SetUpTopUp());
        }
    }
}