using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Up", nameof(Transform))]
    [Serializable]
    public class TweenUp : TweenValueVector3<Transform>
    {
        public override bool SupportSpace => false;
        public override bool SupportSpeedBased => false;

        public override Vector3 Value
        {
            get => Target.up;
            set => Target.up = value;
        }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenUp Up(Transform transform, Vector3 to, float duration)
        {
            var tweener = Play<TweenUp, Transform, Vector3>(transform, to, duration)
                .SetCurrent2From() as TweenUp;
            return tweener;
        }

        public static TweenUp Up(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = Play<TweenUp, Transform, Vector3>(transform, from, to, duration);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenUp TweenUp(this Transform transform, Vector3 to, float duration)
        {
            var tweener = UTween.Up(transform, to, duration);
            return tweener;
        }

        public static TweenUp TweenUp(this Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = UTween.Up(transform, from, to, duration);
            return tweener;
        }
    } 

    #endregion
}