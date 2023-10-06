using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.Features.CleanBoard
{
    public struct DropData
    {
        public EcsPackedEntity ItemPacked;
        public float Delay;
        public float Speed;
        public Vector3 TargetPosition;
        public bool IsDisposable;
    }
}