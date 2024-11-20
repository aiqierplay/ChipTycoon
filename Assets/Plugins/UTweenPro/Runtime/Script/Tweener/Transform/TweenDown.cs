using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Down", nameof(Transform))]
    [Serializable]
    public class TweenDown : TweenValueVector3<Transform>
    {
        public override bool SupportSpace => false;
        public override bool SupportSpeedBased => false;

        public override Vector3 Value
        {
            get => -Target.up;
            set => Target.up = -value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenDown Down(Transform transform, Vector3 to, float duration)
        {
            var tweener = Play<TweenDown, Transform, Vector3>(transform, to, duration)
                .SetCurrent2From() as TweenDown;
            return tweener;
        }

        public static TweenDown Down(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = Play<TweenDown, Transform, Vector3>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenDown TweenDown(this Transform transform, Vector3 to, float duration)
        {
            var tweener = UTween.Down(transform, to, duration);
            return tweener;
        }

        public static TweenDown TweenDown(this Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = UTween.Down(transform, from, to, duration);
            return tweener;
        }
    }

    #endregion
}