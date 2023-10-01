using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.DropItems
{
    public sealed class ItemsDropFeature : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_SetUpItemsDrop());
            systems.Add(new s_ProcessEachItemDropping());
        }
    }
}