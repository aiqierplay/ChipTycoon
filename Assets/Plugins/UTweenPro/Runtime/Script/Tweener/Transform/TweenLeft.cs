using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Left", nameof(Transform))]
    [Serializable]
    public class TweenLeft : TweenValueVector3<Transform>
    {
        public override bool SupportSpace => false;
        public override bool SupportSpeedBased => false;

        public override Vector3 Value
        {
            get => -Target.right;
            set => Target.right = -value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenLeft Left(Transform transform, Vector3 to, float duration)
        {
            var tweener = Play<TweenLeft, Transform, Vector3>(transform, to, duration)
                .SetCurrent2From() as TweenLeft;
            return tweener;
        }

        public static TweenLeft Left(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = Play<TweenLeft, Transform, Vector3>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenLeft TweenLeft(this Transform transform, Vector3 to, float duration)
        {
            var tweener = UTween.Left(transform, to, duration);
            return tweener;
        }

        public static TweenLeft TweenLeft(this Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = UTween.Left(transform, from, to, duration);
            return tweener;
        }
    }

    #endregion
}