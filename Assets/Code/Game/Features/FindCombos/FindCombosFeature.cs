using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.FindCombos
{
    public sealed class FindCombosFeature : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_SetUpFindCombos());
            systems.Add(new s_ProcessFindCombos());  
        }
    }
}