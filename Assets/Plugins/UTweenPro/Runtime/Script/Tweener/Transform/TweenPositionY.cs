using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Position Y", nameof(Transform), -900)]
    [Serializable]
    public class TweenPositionY : TweenValueFloat<Transform>
    {
        public override bool SupportSpace => true;
        public override bool SupportSpeedBased => true;

        public override float Value
        {
            get => Space == SpaceMode.World ? Target.position.y : Target.localPosition.y;
            set
            {
                if (Space == SpaceMode.World)
                {
                    var position = Target.position;
                    position.y = value;
                    Target.position = position;
                }
                else
                {
                    var position = Target.localPosition;
                    position.y = value;
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
        public static TweenPositionY PositionY(Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPositionY, Transform, float>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetCurrent2From() as TweenPositionY;
            return tweener;
        }

        public static TweenPositionY PositionY(Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPositionY, Transform, float>(transform, from, to, duration)
                .SetSpace(spaceMode);
            return tweener;
        }

        public static TweenPositionY LocalPositionY(Transform transform, float to, float duration)
        {
            var tweener = Play<TweenPositionY, Transform, float>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetCurrent2From() as TweenPositionY;
            return tweener;
        }

        public static TweenPositionY LocalPositionY(Transform transform, float from, float to, float duration)
        {
            var tweener = Play<TweenPositionY, Transform, float>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenPositionY TweenPositionY(this Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.PositionY(transform, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPositionY TweenPositionY(this Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.PositionY(transform, from, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPositionY TweenLocalPositionY(this Transform transform, float to, float duration)
        {
            var tweener = UTween.LocalPositionY(transform, to, duration);
            return tweener;
        }

        public static TweenPositionY TweenLocalPositionY(this Transform transform, float from, float to, float duration)
        {
            var tweener = UTween.LocalPositionY(transform, from, to, duration);
            return tweener;
        }
    } 

    #endregion
}