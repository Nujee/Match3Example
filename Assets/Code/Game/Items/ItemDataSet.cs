using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Items
{
    [CreateAssetMenu(fileName = "ItemDataSet", menuName = "ScriptableObjects/ItemDataSet", order = 0)]
    public sealed class ItemDataSet : ScriptableObject
    {
        [field: SerializeField] public List<ItemData> Set { get; private set; }
    }
}