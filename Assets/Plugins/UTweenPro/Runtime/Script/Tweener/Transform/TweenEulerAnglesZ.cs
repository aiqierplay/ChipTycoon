using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("EulerAngles Z", nameof(Transform), -800)]
    [Serializable]
    public partial class TweenEulerAnglesZ : TweenValueFloat<Transform>
    {
        public bool ClampAngle;

        public override bool SupportSpace => true;
        public override bool SampleAngle => ClampAngle;

        public override float Value
        {
            get => Space == SpaceMode.World ? Target.eulerAngles.z : Target.localEulerAngles.z;
            set
            {
                if (Space == SpaceMode.World)
                {
                    var eulerAngles = Target.eulerAngles;
                    eulerAngles.z = value;
                    Target.eulerAngles = eulerAngles;
                }
                else
                {
                    var eulerAngles = Target.localEulerAngles;
                    eulerAngles.z = value;
                    Target.localEulerAngles = eulerAngles;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            ClampAngle = false;
            To.Reset(360f);
        }
    }

#if UNITY_EDITOR

    public partial class TweenEulerAnglesZ : TweenValueFloat<Transform>
    {
        [TweenerProperty, NonSerialized] public SerializedProperty ClampAngleProperty;

        public override void DrawAppend()
        {
            base.DrawAppend();
            GUIUtil.DrawToggleButton(ClampAngleProperty);
        }
    }
#endif

    #region Extension

    public partial class TweenEulerAnglesZ : TweenValueFloat<Transform>
    {
        public TweenEulerAnglesZ SetClampAngle(bool clampAngle = true)
        {
            ClampAngle = clampAngle;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenEulerAnglesZ EulerAnglesZ(Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesZ, Transform, float>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetClampAngle(clampAngle)
                .SetCurrent2From() as TweenEulerAnglesZ;
            return tweener;
        }

        public static TweenEulerAnglesZ EulerAnglesZ(Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesZ, Transform, float>(transform, from, to, duration)
                .SetSpace(spaceMode)
                .SetClampAngle(clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesZ LocalEulerAnglesZ(Transform transform, float to, float duration, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesZ, Transform, float>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetClampAngle(clampAngle)
                .SetCurrent2From() as TweenEulerAnglesZ;
            return tweener;
        }

        public static TweenEulerAnglesZ LocalEulerAnglesZ(Transform transform, float from, float to, float duration, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesZ, Transform, float>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetClampAngle(clampAngle);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenEulerAnglesZ TweenEulerAnglesZ(this Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = UTween.EulerAnglesZ(transform, to, duration, spaceMode, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesZ TweenEulerAnglesZ(this Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = UTween.EulerAnglesZ(transform, from, to, duration, spaceMode, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesZ TweenLocalEulerAnglesZ(this Transform transform, float to, float duration, bool clampAngle = true)
        {
            var tweener = UTween.LocalEulerAnglesZ(transform, to, duration, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesZ TweenLocalEulerAnglesZ(this Transform transform, float from, float to, float duration, bool clampAngle = true)
        {
            var tweener = UTween.LocalEulerAnglesZ(transform, from, to, duration, clampAngle);
            return tweener;
        }
    }

    #endregion
}