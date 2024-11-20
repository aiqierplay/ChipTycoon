using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Backward",  nameof(Transform))]
    [Serializable]
    public class TweenBackward : TweenValueVector3<Transform>
    {
        public override bool SupportSpace => false;
        public override bool SupportSpeedBased => false;

        public override Vector3 Value
        {
            get => -Target.forward;
            set => Target.forward = -value;
        }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenBackward Backward(Transform transform, Vector3 to, float duration)
        {
            var tweener = Play<TweenBackward, Transform, Vector3>(transform, to, duration)
                .SetCurrent2From() as TweenBackward;
            return tweener;
        }

        public static TweenBackward Backward(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = Play<TweenBackward, Transform, Vector3>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenBackward TweenBackward(this Transform transform, Vector3 to, float duration)
        {
            var tweener = UTween.Backward(transform, to, duration);
            return tweener;
        }

        public static TweenBackward TweenBackward(this Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = UTween.Backward(transform, from, to, duration);
            return tweener;
        }
    } 

    #endregion
}