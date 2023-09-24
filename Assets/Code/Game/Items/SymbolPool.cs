using Code.MySubmodule.Pools;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.Items
{
    public sealed class SymbolPool : Pool
    {
        private readonly ItemData _itemData;

        public SymbolPool(ItemData itemData)
        {
            _itemData = itemData;
            _itemData.Pool = this;
        }
        
        protected override void DoOnGet(EcsWorld world, int entity, GameObject pooledObject)
        {
            ref var c_item = ref world.GetPool<c_Item>().Add(entity);
            c_item.Data = _itemData;
        }
    }
}