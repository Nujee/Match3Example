using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.Hero
{
    public struct c_Cell
    {
        public Vector2Int BoardPosition;
        public Vector3 WorldPosition;
        public EcsPackedEntity AttachedItemPacked;// -1 if slot is empty
    }
}