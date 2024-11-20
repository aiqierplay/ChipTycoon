using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Right", nameof(Transform))]
    [Serializable]
    public class TweenRight : TweenValueVector3<Transform>
    {
        public override bool SupportSpace => false;
        public override bool SupportSpeedBased => false;

        public override Vector3 Value
        {
            get => Target.right;
            set => Target.right = value;
        }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenRight Right(Transform transform, Vector3 to, float duration)
        {
            var tweener = Play<TweenRight, Transform, Vector3>(transform, to, duration)
                .SetCurrent2From() as TweenRight;
            return tweener;
        }

        public static TweenRight Right(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = Play<TweenRight, Transform, Vector3>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenRight TweenRight(this Transform transform, Vector3 to, float duration)
        {
            var tweener = UTween.Right(transform, to, duration);
            return tweener;
        }

        public static TweenRight TweenRight(this Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = UTween.Right(transform, from, to, duration);
            return tweener;
        }
    } 

    #endregion
}