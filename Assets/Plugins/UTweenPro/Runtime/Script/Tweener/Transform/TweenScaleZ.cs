using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Scale Z", nameof(Transform), -700)]
    [Serializable]
    public class TweenScaleZ : TweenValueFloat<Transform>
    {
        public override float Value
        {
            get => Target.localScale.z;
            set
            {
                var localScale = Target.localScale;
                localScale.z = value;
                Target.localScale = localScale;
            }
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenScaleZ ScaleZ(Transform transform, float to, float duration)
        {
            var tweener = Play<TweenScaleZ, Transform, float>(transform, to, duration)
                .SetCurrent2From() as TweenScaleZ;
            return tweener;
        }

        public static TweenScaleZ ScaleZ(Transform transform, float from, float to, float duration)
        {
            var tweener = Play<TweenScaleZ, Transform, float>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenScaleZ TweenScaleZ(this Transform transform, float to, float duration)
        {
            var tweener = UTween.ScaleZ(transform, to, duration);
            return tweener;
        }

        public static TweenScaleZ TweenScaleZ(this Transform transform, float from, float to, float duration)
        {
            var tweener = UTween.ScaleZ(transform, from, to, duration);
            return tweener;
        }
    }

    #endregion
}