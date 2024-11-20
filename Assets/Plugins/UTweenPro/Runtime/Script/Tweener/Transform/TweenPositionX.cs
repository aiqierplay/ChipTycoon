using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Position X", nameof(Transform), -900)]
    [Serializable]
    public class TweenPositionX : TweenValueFloat<Transform>
    {
        public override bool SupportSpace => true;
        public override bool SupportSpeedBased => true;

        public override float Value
        {
            get => Space == SpaceMode.World ? Target.position.x : Target.localPosition.x;
            set
            {
                if (Space == SpaceMode.World)
                {
                    var position = Target.position;
                    position.x = value;
                    Target.position = position;
                }
                else
                {
                    var position = Target.localPosition;
                    position.x = value;
                    Target.localPosition = position;
                }
            }
        }

        public override float GetSpeedBasedDuration()
        {
            var distance = Mathf.Abs(From - To);
            var duration = distance / Duration;
            return duration;
        }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenPositionX PositionX(Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPositionX, Transform, float>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetCurrent2From() as TweenPositionX;
            return tweener;
        }

        public static TweenPositionX PositionX(Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPositionX, Transform, float>(transform, from, to, duration)
                .SetSpace(spaceMode);
            return tweener;
        }

        public static TweenPositionX LocalPositionX(Transform transform, float to, float duration)
        {
            var tweener = Play<TweenPositionX, Transform, float>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetCurrent2From() as TweenPositionX;
            return tweener;
        }

        public static TweenPositionX LocalPositionX(Transform transform, float from, float to, float duration)
        {
            var tweener = Play<TweenPositionX, Transform, float>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenPositionX TweenPositionX(this Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.PositionX(transform, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPositionX TweenPositionX(this Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.PositionX(transform, from, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPositionX TweenLocalPositionX(this Transform transform, float to, float duration)
        {
            var tweener = UTween.LocalPositionX(transform, to, duration);
            return tweener;
        }

        public static TweenPositionX TweenLocalPositionX(this Transform transform, float from, float to, float duration)
        {
            var tweener = UTween.LocalPositionX(transform, from, to, duration);
            return tweener;
        }
    } 

    #endregion
}