using UnityEngine;

namespace Code.Game.Main
{
    [DisallowMultipleComponent]
    public sealed class LevelSettings : MonoBehaviour
    {
        [field: SerializeField] public int BoardRows { get; private set; }
        [field: SerializeField] public int BoardColumns { get; private set; }
        [field: SerializeField] public float BoardCellsWidth { get; private set; }
        [field: SerializeField] public float BoardSlotsHeight { get; private set; }
    }
}