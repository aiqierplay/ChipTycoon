using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("EulerAngles", nameof(Transform), -999)]
    [Serializable]
    public partial class TweenEulerAngles : TweenValueVector3<Transform>
    {
        public override bool SupportSpace => true;
        public override bool SampleAngle => true;

        public override Vector3 Value
        {
            get => Space == SpaceMode.World ? Target.eulerAngles : Target.localEulerAngles;
            set
            {
                if (Space == SpaceMode.World)
                {
                    Target.eulerAngles = value;
                }
                else
                {
                    Target.localEulerAngles = value;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            AngleMode = AngleMode.Clamp360;
            From.Reset(Vector3.zero);
            To.Reset(new Vector3(0f, 360f, 0f));
        }
    }

#if UNITY_EDITOR

    public partial class TweenEulerAngles : TweenValueVector3<Transform>
    {
        [TweenerProperty, NonSerialized] public SerializedProperty AngleModeProperty;

        public override void DrawAppend()
        {
            base.DrawAppend();
            GUIUtil.DrawToolbarEnum(AngleModeProperty, "Angle", typeof(AngleMode));
        }
    }
#endif

    #region Extension

    public partial class TweenEulerAngles : TweenValueVector3<Transform>
    {
        public TweenEulerAngles SetAngleMode(AngleMode angleMode)
        {
            AngleMode = angleMode;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenEulerAngles EulerAngles(Transform transform, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = Play<TweenEulerAngles, Transform, Vector3>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetAngleMode(angleMode)
                .SetCurrent2From() as TweenEulerAngles;
            return tweener;
        }

        public static TweenEulerAngles EulerAngles(Transform transform, Vector3 from, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = Play<TweenEulerAngles, Transform, Vector3>(transform, from, to, duration)
                .SetSpace(spaceMode)
                .SetAngleMode(angleMode);
            return tweener;
        }

        public static TweenEulerAngles LocalEulerAngles(Transform transform, Vector3 to, float duration, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = Play<TweenEulerAngles, Transform, Vector3>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetAngleMode(angleMode)
                .SetCurrent2From() as TweenEulerAngles;
            return tweener;
        }

        public static TweenEulerAngles LocalEulerAngles(Transform transform, Vector3 from, Vector3 to, float duration, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = Play<TweenEulerAngles, Transform, Vector3>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetAngleMode(angleMode);
            return tweener;
        }

        public static TweenEulerAngles EulerAngles(Transform transform, Transform to, float duration, SpaceMode spaceMode = SpaceMode.World, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = Play<TweenEulerAngles, Transform, Vector3>(transform,
                    (spaceMode == SpaceMode.World ? to.eulerAngles : to.localEulerAngles),
                    duration)
                .SetSpace(spaceMode)
                .SetAngleMode(angleMode)
                .SetCurrent2From() as TweenEulerAngles;
            return tweener;
        }

        public static TweenEulerAngles EulerAngles(Transform transform, Transform from, Transform to, float duration, SpaceMode spaceMode = SpaceMode.World, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = Play<TweenEulerAngles, Transform, Vector3>(transform,
                    (spaceMode == SpaceMode.World ? from.eulerAngles : from.localEulerAngles),
                    (spaceMode == SpaceMode.World ? to.eulerAngles : to.localEulerAngles),
                    duration)
                .SetSpace(spaceMode)
                .SetAngleMode(angleMode);
            return tweener;
        }

        public static TweenEulerAngles LocalEulerAngles(Transform transform, Transform to, float duration, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = Play<TweenEulerAngles, Transform, Vector3>(transform, to.localEulerAngles, duration)
                .SetSpace(SpaceMode.Local)
                .SetAngleMode(angleMode)
                .SetCurrent2From() as TweenEulerAngles;
            return tweener;
        }

        public static TweenEulerAngles LocalEulerAngles(Transform transform, Transform from, Transform to, float duration, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = Play<TweenEulerAngles, Transform, Vector3>(transform, from.localEulerAngles, to.localEulerAngles, duration)
                .SetSpace(SpaceMode.Local)
                .SetAngleMode(angleMode);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenEulerAngles TweenEulerAngles(this Transform transform, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = UTween.EulerAngles(transform, to, duration, spaceMode, angleMode);
            return tweener;
        }

        public static TweenEulerAngles TweenEulerAngles(this Transform transform, Vector3 from, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = UTween.EulerAngles(transform, from, to, duration, spaceMode, angleMode);
            return tweener;
        }

        public static TweenEulerAngles TweenLocalEulerAngles(this Transform transform, Vector3 to, float duration, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = UTween.LocalEulerAngles(transform, to, duration, angleMode);
            return tweener;
        }

        public static TweenEulerAngles TweenLocalEulerAngles(this Transform transform, Vector3 from, Vector3 to, float duration, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = UTween.LocalEulerAngles(transform, from, to, duration, angleMode);
            return tweener;
        }

        public static TweenEulerAngles TweenEulerAngles(this Transform transform, Transform to, float duration, SpaceMode spaceMode = SpaceMode.World, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = UTween.EulerAngles(transform, to, duration, spaceMode, angleMode);
            return tweener;
        }

        public static TweenEulerAngles TweenEulerAngles(this Transform transform, Transform from, Transform to, float duration, SpaceMode spaceMode = SpaceMode.World, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = UTween.EulerAngles(transform, from, to, duration, spaceMode, angleMode);
            return tweener;
        }

        public static TweenEulerAngles TweenLocalEulerAngles(this Transform transform, Transform to, float duration, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = UTween.LocalEulerAngles(transform, to, duration, angleMode);
            return tweener;
        }

        public static TweenEulerAngles TweenLocalEulerAngles(this Transform transform, Transform from, Transform to, float duration, AngleMode angleMode = AngleMode.Clamp360)
        {
            var tweener = UTween.LocalEulerAngles(transform, from, to, duration, angleMode);
            return tweener;
        }
    }

    #endregion
}