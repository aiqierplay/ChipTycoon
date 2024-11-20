using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Position Z", nameof(Transform), -900)]
    [Serializable]
    public class TweenPositionZ : TweenValueFloat<Transform>
    {
        public override bool SupportSpace => true;
        public override bool SupportSpeedBased => true;

        public override float Value
        {
            get => Space == SpaceMode.World ? Target.position.z : Target.localPosition.z;
            set
            {
                if (Space == SpaceMode.World)
                {
                    var position = Target.position;
                    position.z = value;
                    Target.position = position;
                }
                else
                {
                    var position = Target.localPosition;
                    position.z = value;
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
        public static TweenPositionZ PositionZ(Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPositionZ, Transform, float>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetCurrent2From() as TweenPositionZ;
            return tweener;
        }

        public static TweenPositionZ PositionZ(Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenPositionZ, Transform, float>(transform, from, to, duration)
                .SetSpace(spaceMode);
            return tweener;
        }

        public static TweenPositionZ LocalPositionZ(Transform transform, float to, float duration)
        {
            var tweener = Play<TweenPositionZ, Transform, float>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetCurrent2From() as TweenPositionZ;
            return tweener;
        }

        public static TweenPositionZ LocalPositionZ(Transform transform, float from, float to, float duration)
        {
            var tweener = Play<TweenPositionZ, Transform, float>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenPositionZ TweenPositionZ(this Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.PositionZ(transform, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPositionZ TweenPositionZ(this Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.PositionZ(transform, from, to, duration, spaceMode);
            return tweener;
        }

        public static TweenPositionZ TweenLocalPositionZ(this Transform transform, float to, float duration)
        {
            var tweener = UTween.LocalPositionZ(transform, to, duration);
            return tweener;
        }

        public static TweenPositionZ TweenLocalPositionZ(this Transform transform, float from, float to, float duration)
        {
            var tweener = UTween.LocalPositionZ(transform, from, to, duration);
            return tweener;
        }
    } 

    #endregion
}