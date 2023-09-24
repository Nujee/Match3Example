using Code.MySubmodule.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Game.Items
{
    [System.Serializable]
    public sealed class ItemData
    {
        [field: SerializeField] public ItemType Type { get; private set; }
        [field: SerializeField] public AssetReference Prefab { get; private set; }
        public Pool Pool { get; set; }
    }
}