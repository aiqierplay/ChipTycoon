using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Position", nameof(Transform), -1000)]
    // [TweenerDocumentation("Tween Position", "Moves the target's position to the given value.")]
    [Serializable]
    public class TweenPosition : TweenValueVector3<Transform>
    {
        public override bool SupportSpace => true;
        public override bool SupportSpeedBased => true;

        public override Vector3 Value
        {
            get => Space == SpaceMode.World ? Target.position : Target.localPosition;
            set
            {
                if (Space == SpaceMode.World)
                {
                    Target.position = value;
                }
                else
                {
                    Target.localPosition = value;
                }
            }
        }

        public override float GetSpeedBasedDuration()
        {
            var distance = Vector3.Distance(From, To);
            var duration = distance / Duration;
            return duration;
        }

        public override void Reset()
        {
            base.Reset();
            To.Reset(Vector3.forward);
        }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenPosition Position(Transform transform, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPosition, Transform, Vector3>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetCurrent2From() as TweenPosition;
            return tweener;
        }

        public static TweenPosition Position(Transform transform, Vector3 from, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPosition, Transform, Vector3>(transform, from, to, duration)
                .SetSpace(spaceMode);
            return tweener;
        }

        public static TweenPosition LocalPosition(Transform transform, Vector3 to, float duration)
        {
            var tweener = Play<TweenPosition, Transform, Vector3>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetCurrent2From() as TweenPosition;
            return tweener;
        }

        public static TweenPosition LocalPosition(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = Play<TweenPosition, Transform, Vector3>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local);
            return tweener;
        }

        public static TweenPosition Position(Transform transform, Transform to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPosition, Transform, Vector3>(transform, 
                    (spaceMode == SpaceMode.World ? to.position : to.localPosition), 
                    duration)
                .SetSpace(spaceMode)
                .SetCurrent2From() as TweenPosition;
            return tweener;
        }

        public static TweenPosition Position(Transform transform, Transform from, Transform to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPosition, Transform, Vector3>(transform, 
                    (spaceMode == SpaceMode.World ? from.position : from.localPosition),
                    (spaceMode == SpaceMode.World ? to.position : to.localPosition),
                    duration)
                .SetSpace(spaceMode);
            return tweener;
        }

        public static TweenPosition LocalPosition(Transform transform, Transform to, float duration)
        {
            var tweener = Play<TweenPosition, Transform, Vector3>(transform, to.localPosition, duration)
                .SetSpace(SpaceMode.Local)
                .SetCurrent2From() as TweenPosition;
            return tweener;
        }

        public static TweenPosition LocalPosition(Transform transform, Transform from, Transform to, float duration)
        {
            var tweener = Play<TweenPosition, Transform, Vector3>(transform, from.localPosition, to.localPosition, duration)
                .SetSpace(SpaceMode.Local);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenPosition TweenPosition(this Transform transform, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.Position(transform, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPosition TweenPosition(this Transform transform, Vector3 from, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.Position(transform, from, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPosition TweenLocalPosition(this Transform transform, Vector3 to, float duration)
        {
            var tweener = UTween.LocalPosition(transform, to, duration);
            return tweener;
        }

        public static TweenPosition TweenLocalPosition(this Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var tweener = UTween.LocalPosition(transform, from, to, duration);
            return tweener;
        }

        public static TweenPosition TweenPosition(this Transform transform, Transform to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.Position(transform, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPosition TweenPosition(this Transform transform, Transform from, Transform to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.Position(transform, from, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPosition TweenLocalPosition(this Transform transform, Transform to, float duration)
        {
            var tweener = UTween.LocalPosition(transform, to, duration);
            return tweener;
        }

        public static TweenPosition TweenLocalPosition(this Transform transform, Transform from, Transform to, float duration)
        {
            var tweener = UTween.LocalPosition(transform, from, to, duration);
            return tweener;
        }
    } 

    #endregion
}