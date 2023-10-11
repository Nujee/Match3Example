using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.DropItems
{
    public sealed class f_DropItems : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_SetUpDropItems());
            systems.Add(new s_ProcessDropItems());
        }
    }
}