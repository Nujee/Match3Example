﻿using Code.Game.Utils;
using TriInspector;
using UnityEngine;

namespace Code.Game.Main
{
    [DisallowMultipleComponent]
    public sealed class LevelSettings : MonoBehaviour
    {        
        [field: SerializeField] public float PreFindCombosDelay { get; private set; }
        [field: SerializeField] public int MinComboSize { get; private set; }
        [field: SerializeField] public float PreRemoveComboDelay { get; private set; }
        [field: Title("Board")]
        [field: SerializeField] public int BoardRows { get; private set; }
        [field: SerializeField] public int BoardColumns { get; private set; }
        [field: SerializeField] public float BoardCellsWidth { get; private set; }
        [field: SerializeField] public float BoardSlotsHeight { get; private set; }
        [field: Title("Dropping")]
        [field: SerializeField] public float DropItemsInBetweenDelay { get; private set; }
        [field: SerializeField] public float DropItemsStartSpeed { get; private set; }
        [field: SerializeField] public float DropItemsMaxSpeed { get; private set; }
        [field: SerializeField] public float DropItemsAcceleration { get; private set; }
        [field: Title("Landing")]
        [field: SerializeField] public float LandDrawdown { get; private set; }
        [field: SerializeField] public TweenSettings LandTween { get; private set; }
        [field: Title("Shaking")]
        [field: SerializeField] public float ShakeComboDuration { get; private set; }
        [field: SerializeField] public float ShakeComboMagnitude { get; private set; }
        [field: Title("Scattering")]
        [field: SerializeField] public float ScatterForce { get; private set; }
        [field: SerializeField] public TweenSettings ScatterTween { get; private set; }
    }
}