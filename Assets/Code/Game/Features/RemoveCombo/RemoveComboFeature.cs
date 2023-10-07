using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.RemoveCombo
{
    public sealed class RemoveComboFeature : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_SetUpRemoveCombo());
            systems.Add(new s_ShakeComboItems());
            systems.Add(new s_ExplodeComboItems());
            systems.Add(new s_ScatterNonComboItems());
            // start delay and do something like shaking during this delay
            // push other items on shake end
            // somehow "explode"(?) combo items and remove at the explosion end
            // after delay, drop new items on their places
        }
    }
}