using DG.Tweening;
using UnityEngine;

namespace Code.Game.Utils
{
    public static class DOTweenExtensions
    {
        public static Tweener DOPunchPosition(this Transform transform, Vector3 punchForce, TweenSettings settings)
        {
            return transform.DOPunchPosition(punchForce, settings.Duration, settings.Vibrato, settings.Elasticity);
        }
    }
}