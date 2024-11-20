using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Rotation", nameof(Transform))]
    [Serializable]
    public class TweenRotation : TweenValueQuaternion<Transform>
    {
        public override bool SupportSpace => true;

        public override Quaternion Value
        {
            get => Space == SpaceMode.World ? Target.rotation : Target.localRotation;
            set
            {
                if (Space == SpaceMode.World)
                {
                    Target.rotation = value;
                }
                else
                {
                    Target.localRotation = value;
                }
            }
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenRotation Rotation(Transform transform, Quaternion to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenRotation, Transform, Quaternion>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetCurrent2From() as TweenRotation;
            return tweener;
        }

        public static TweenRotation Rotation(Transform transform, Quaternion from, Quaternion to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenRotation, Transform, Quaternion>(transform, from, to, duration)
                .SetSpace(spaceMode);
            return tweener;
        }

        public static TweenRotation LocalRotation(Transform transform, Quaternion to, float duration)
        {
            var tweener = Play<TweenRotation, Transform, Quaternion>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetCurrent2From() as TweenRotation;
            return tweener;
        }

        public static TweenRotation LocalRotation(Transform transform, Quaternion from, Quaternion to, float duration)
        {
            var tweener = Play<TweenRotation, Transform, Quaternion>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenRotation TweenRotation(this Transform transform, Quaternion to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.Rotation(transform, to, duration, spaceMode);
            return tweener;
        }

        public static TweenRotation TweenRotation(this Transform transform, Quaternion from, Quaternion to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.Rotation(transform, from, to, duration, spaceMode);
            return tweener;
        }

        public static TweenRotation TweenLocalRotation(this Transform transform, Quaternion to, float duration)
        {
            var tweener = UTween.LocalRotation(transform, to, duration);
            return tweener;
        }

        public static TweenRotation TweenLocalRotation(this Transform transform, Quaternion from, Quaternion to, float duration)
        {
            var tweener = UTween.LocalRotation(transform, from, to, duration);
            return tweener;
        }

        public static TweenRotation TweenLookAt(this Transform transform, Transform target, float duration)
        {
            var direction = target.position - transform.position;
            var rotation = Quaternion.LookRotation(direction);
            var tweener = UTween.Rotation(transform, rotation, duration);
            return tweener;
        }

        public static TweenRotation TweenLookAt(this Transform transform, Vector3 target, float duration)
        {
            var direction = target - transform.position;
            var rotation = Quaternion.LookRotation(direction);
            var tweener = UTween.Rotation(transform, rotation, duration);
            return tweener;
        }
    }

    #endregion
}