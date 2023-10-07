using System.Collections.Generic;
using Code.Game.Items;
using Leopotam.EcsLite;

namespace Code.Game.Features.RemoveCombo
{
    public struct r_RemoveCombo
    {
        // such delay is temp
        public float RemoveDelay;
        public (ItemType type, List<EcsPackedEntity> cells) ComboTypeToCellsPacked;
        
        public r_RemoveCombo(float removeDelay, (ItemType type, List<EcsPackedEntity> cells) comboTypeToCellsPacked)
        {
            RemoveDelay = removeDelay;
            ComboTypeToCellsPacked = comboTypeToCellsPacked;
        }
    }
}