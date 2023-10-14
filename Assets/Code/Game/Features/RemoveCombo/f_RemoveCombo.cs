using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.RemoveCombo
{
    public sealed class f_RemoveCombo : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_SetUpRemoveCombo());
            
            systems.Add(new s_ShakeComboItems());
            systems.Add(new s_ExplodeComboItems());
            systems.Add(new s_ScatterNonComboItems());
            systems.Add(new s_TopUpCells());
        }
    }
}