using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("EulerAngles X", nameof(Transform), -800)]
    [Serializable]
    public partial class TweenEulerAnglesX : TweenValueFloat<Transform>
    {
        public bool ClampAngle;

        public override bool SupportSpace => true;
        public override bool SampleAngle => ClampAngle;

        public override float Value
        {
            get => Space == SpaceMode.World ? Target.eulerAngles.x : Target.localEulerAngles.x;
            set
            {
                if (Space == SpaceMode.World)
                {
                    var eulerAngles = Target.eulerAngles;
                    eulerAngles.x = value;
                    Target.eulerAngles = eulerAngles;
                }
                else
                {
                    var eulerAngles = Target.localEulerAngles;
                    eulerAngles.x = value;
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

    public partial class TweenEulerAnglesX : TweenValueFloat<Transform>
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

    public partial class TweenEulerAnglesX : TweenValueFloat<Transform>
    {
        public TweenEulerAnglesX SetClampAngle(bool clampAngle = true)
        {
            ClampAngle = clampAngle;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenEulerAnglesX EulerAnglesX(Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesX, Transform, float>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetClampAngle(clampAngle)
                .SetCurrent2From() as TweenEulerAnglesX;
            return tweener;
        }

        public static TweenEulerAnglesX EulerAnglesX(Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesX, Transform, float>(transform, from, to, duration)
                .SetSpace(spaceMode)
                .SetClampAngle(clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesX LocalEulerAnglesX(Transform transform, float to, float duration, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesX, Transform, float>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetClampAngle(clampAngle)
                .SetCurrent2From() as TweenEulerAnglesX;
            return tweener;
        }

        public static TweenEulerAnglesX LocalEulerAnglesX(Transform transform, float from, float to, float duration, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesX, Transform, float>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetClampAngle(clampAngle);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenEulerAnglesX TweenEulerAnglesX(this Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = UTween.EulerAnglesX(transform, to, duration, spaceMode, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesX TweenEulerAnglesX(this Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = UTween.EulerAnglesX(transform, from, to, duration, spaceMode, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesX TweenLocalEulerAnglesX(this Transform transform, float to, float duration, bool clampAngle = true)
        {
            var tweener = UTween.LocalEulerAnglesX(transform, to, duration, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesX TweenLocalEulerAnglesX(this Transform transform, float from, float to, float duration, bool clampAngle = true)
        {
            var tweener = UTween.LocalEulerAnglesX(transform, from, to, duration, clampAngle);
            return tweener;
        }
    }

    #endregion
}