using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("EulerAngles Y", nameof(Transform), -800)]
    [Serializable]
    public partial class TweenEulerAnglesY : TweenValueFloat<Transform>
    {
        public bool ClampAngle;

        public override bool SupportSpace => true;
        public override bool SampleAngle => ClampAngle;

        public override float Value
        {
            get => Space == SpaceMode.World ? Target.eulerAngles.y : Target.localEulerAngles.y;
            set
            {
                if (Space == SpaceMode.World)
                {
                    var eulerAngles = Target.eulerAngles;
                    eulerAngles.y = value;
                    Target.eulerAngles = eulerAngles;
                }
                else
                {
                    var eulerAngles = Target.localEulerAngles;
                    eulerAngles.y = value;
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

    public partial class TweenEulerAnglesY : TweenValueFloat<Transform>
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

    public partial class TweenEulerAnglesY : TweenValueFloat<Transform>
    {
        public TweenEulerAnglesY SetClampAngle(bool clampAngle = true)
        {
            ClampAngle = clampAngle;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenEulerAnglesY EulerAnglesY(Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesY, Transform, float>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetClampAngle(clampAngle)
                .SetCurrent2From() as TweenEulerAnglesY;
            return tweener;
        }

        public static TweenEulerAnglesY EulerAnglesY(Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesY, Transform, float>(transform, from, to, duration)
                .SetSpace(spaceMode)
                .SetClampAngle(clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesY LocalEulerAnglesY(Transform transform, float to, float duration, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesY, Transform, float>(transform, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetClampAngle(clampAngle)
                .SetCurrent2From() as TweenEulerAnglesY;
            return tweener;
        }

        public static TweenEulerAnglesY LocalEulerAnglesY(Transform transform, float from, float to, float duration, bool clampAngle = true)
        {
            var tweener = Play<TweenEulerAnglesY, Transform, float>(transform, from, to, duration)
                .SetSpace(SpaceMode.Local)
                .SetClampAngle(clampAngle);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenEulerAnglesY TweenEulerAnglesY(this Transform transform, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = UTween.EulerAnglesY(transform, to, duration, spaceMode, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesY TweenEulerAnglesY(this Transform transform, float from, float to, float duration, SpaceMode spaceMode = SpaceMode.World, bool clampAngle = true)
        {
            var tweener = UTween.EulerAnglesY(transform, from, to, duration, spaceMode, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesY TweenLocalEulerAnglesY(this Transform transform, float to, float duration, bool clampAngle = true)
        {
            var tweener = UTween.LocalEulerAnglesY(transform, to, duration, clampAngle);
            return tweener;
        }

        public static TweenEulerAnglesY TweenLocalEulerAnglesY(this Transform transform, float from, float to, float duration, bool clampAngle = true)
        {
            var tweener = UTween.LocalEulerAnglesY(transform, from, to, duration, clampAngle);
            return tweener;
        }
    }

    #endregion
}