using Code.MySubmodule.ECS.Features;
using Leopotam.EcsLite;

namespace Code.Game.Features.RenewBoard
{
    public class RenewBoardFeature : IFeature
    {
        public void Init(EcsSystems systems)
        {
            systems.Add(new s_SetUpRenewBoard());
            systems.Add(new s_UpdateSlotItems());
            //systems.Add(new s_RenewBoard());
        }
    }
}