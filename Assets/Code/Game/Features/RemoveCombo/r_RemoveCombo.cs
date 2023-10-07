﻿using System.Collections.Generic;
using Code.Game.Items;
using Leopotam.EcsLite;

namespace Code.Game.Features.RemoveCombo
{
    public struct r_RemoveCombo
    {
        public (ItemType type, List<EcsPackedEntity> cells) ComboTypeToCellsPacked;
        
        public r_RemoveCombo((ItemType type, List<EcsPackedEntity> cells) comboTypeToCellsPacked)
        {
            ComboTypeToCellsPacked = comboTypeToCellsPacked;
        }
    }
}