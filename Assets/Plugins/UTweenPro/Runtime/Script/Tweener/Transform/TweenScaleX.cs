using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Scale X", nameof(Transform), -700)]
    [Serializable]
    public class TweenScaleX : TweenValueFloat<Transform>
    {
        public override float Value
        {
            get => Target.localScale.x;
            set
            {
                var localScale = Target.localScale;
                localScale.x = value;
                Target.localScale = localScale;
            }
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenScaleX ScaleX(Transform transform, float to, float duration)
        {
            var tweener = Play<TweenScaleX, Transform, float>(transform, to, duration)
                .SetCurrent2From() as TweenScaleX;
            return tweener;
        }

        public static TweenScaleX ScaleX(Transform transform, float from, float to, float duration)
        {
            var tweener = Play<TweenScaleX, Transform, float>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenScaleX TweenScaleX(this Transform transform, float to, float duration)
        {
            var tweener = UTween.ScaleX(transform, to, duration);
            return tweener;
        }

        public static TweenScaleX TweenScaleX(this Transform transform, float from, float to, float duration)
        {
            var tweener = UTween.ScaleX(transform, from, to, duration);
            return tweener;
        }
    }

    #endregion
}