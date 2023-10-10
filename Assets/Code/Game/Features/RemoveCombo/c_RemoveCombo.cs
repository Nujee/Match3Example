using System.Collections.Generic;
using Code.Game.Items;
using Leopotam.EcsLite;

namespace Code.Game.Features.RemoveCombo
{
    public struct c_RemoveCombo
    {
        public EcsPackedEntity BoardPacked;
        public (ItemType type, List<EcsPackedEntity> cells) ComboTypeToCellsPacked;
        public float ShakeDurationTotal;
        public float ShakeDurationElapsed;
        public float ShakeMagnitude;
    }
}