using Code.Game.Hero;
using Code.Game.Items;
using Code.MySubmodule.Math;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.Utils
{
    public static class Extensions
    {
        public static void SetRandomItem(ref this c_Cell c_cell, ItemDataSet itemDataSet, EcsWorld world, Vector3 whereTo)
        {
            var randomItemEntity = itemDataSet.Set.Random().Pool.Get(whereTo, Quaternion.identity);
            c_cell.AttachedItemPacked = world.PackEntity(randomItemEntity);
        }
    }
}