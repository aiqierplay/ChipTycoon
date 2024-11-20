using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Scale Y", nameof(Transform), -700)]
    [Serializable]
    public class TweenScaleY : TweenValueFloat<Transform>
    {
        public override float Value
        {
            get => Target.localScale.y;
            set
            {
                var localScale = Target.localScale;
                localScale.y = value;
                Target.localScale = localScale;
            }
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenScaleY ScaleY(Transform transform, float to, float duration)
        {
            var tweener = Play<TweenScaleY, Transform, float>(transform, to, duration)
                .SetCurrent2From() as TweenScaleY;
            return tweener;
        }

        public static TweenScaleY ScaleY(Transform transform, float from, float to, float duration)
        {
            var tweener = Play<TweenScaleY, Transform, float>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenScaleY TweenScaleY(this Transform transform, float to, float duration)
        {
            var tweener = UTween.ScaleY(transform, to, duration);
            return tweener;
        }

        public static TweenScaleY TweenScaleY(this Transform transform, float from, float to, float duration)
        {
            var tweener = UTween.ScaleY(transform, from, to, duration);
            return tweener;
        }
    }

    #endregion
}