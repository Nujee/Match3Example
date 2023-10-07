using System;
using Code.Game.Hero;
using Code.Game.Items;
using Code.MySubmodule.Math;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.Utils
{
    public static class Extensions
    {
        public static EcsPackedEntity SetRandomItem(ref this c_Cell c_cell, EcsWorld world, ItemDataSet itemDataSet, Vector3 whereTo)
        {
            var randomItemEntity = itemDataSet.Set.Random()!.Pool.Get(whereTo, Quaternion.identity);
            c_cell.AttachedItemPacked = world.PackEntity(randomItemEntity);

            return c_cell.AttachedItemPacked;
        }
        
        public static ref c_Item GetAttachedItem(this EcsPackedEntity cellPacked, EcsWorld world)
        {
            if (!cellPacked.Unpack(world, out var cellEntity))
                throw new Exception("Can't unpack cell entity");

            var attachedItemEntity = GetAttachedItemInternal(world, cellEntity);
    
            return ref world.GetPool<c_Item>().Get(attachedItemEntity);
        }
        
        public static ref c_Item GetAttachedItem(this EcsPackedEntity cellPacked, EcsWorld world, out int attachedItemEntity)
        {
            if (!cellPacked.Unpack(world, out var cellEntity))
                throw new Exception("Can't unpack cell entity");

            attachedItemEntity = GetAttachedItemInternal(world, cellEntity);
    
            return ref world.GetPool<c_Item>().Get(attachedItemEntity);
        }

        private static int GetAttachedItemInternal(EcsWorld world, int cellEntity)
        {
            ref var c_cell = ref world.GetPool<c_Cell>().Get(cellEntity);

            if (!c_cell.AttachedItemPacked.Unpack(world, out var attachedItemEntity))
                throw new Exception("Can't unpack attached item entity");

            return attachedItemEntity;
        }
    }
}