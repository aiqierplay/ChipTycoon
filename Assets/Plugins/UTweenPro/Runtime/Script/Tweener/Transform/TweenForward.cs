using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Forward", nameof(Transform))]
    [Serializable]
    public class TweenForward : TweenValueVector3<Transform>
    {
        public override bool SupportSpace => false;
        public override bool SupportSpeedBased => false;

        public override Vector3 Value
        {
            get => Target.forward;
            set => Target.forward = value;
        }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenForward Forward(Transform transform, Vector3 to, float duration)
        {
            var tweener = Play<TweenForward, Transform, Vector3>(transform, to, duration)
                .SetCurrent2From() as TweenForward;
            return tweener;
        }

        public static TweenForward Forward(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = Play<TweenForward, Transform, Vector3>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenForward TweenForward(this Transform transform, Vector3 to, float duration)
        {
            var tweener = UTween.Forward(transform, to, duration);
            return tweener;
        }

        public static TweenForward TweenForward(this Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = UTween.Forward(transform, from, to, duration);
            return tweener;
        }
    } 

    #endregion
}