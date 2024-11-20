using System;

namespace Aya.TweenPro
{
    public partial class TweenFloat : TweenValueFloat<UnityEngine.Object>
    {
        public bool ClampAngle;
        public override bool SampleAngle => ClampAngle;

        public override float Value { get; set; }

        public override void Reset()
        {
            base.Reset();
            ClampAngle = false;
        }
    }

    #region Extension

    public partial class TweenFloat : TweenValueFloat<UnityEngine.Object>
    {
        public TweenFloat SetClampAngle(bool clampAngle = true)
        {
            ClampAngle = clampAngle;
            return this;
        }
    }

    public static partial class UTween
    {
        #region float

        public static TweenFloat Value10(float duration, Action<float> onValue)
        {
            var tweener = Value(1f, 0f, duration, false, onValue);
            return tweener;
        }

        public static TweenFloat Value01(float duration, Action<float> onValue)
        {
            var tweener = Value(0f, 1f, duration, false, onValue);
            return tweener;
        }

        public static TweenFloat Value(float from, float to, float duration, Action<float> onValue)
        {
            var tweener = Value(from, to, duration, false, onValue);
            return tweener;
        }

        public static TweenFloat Value(float from, float to, float duration, bool clampAngle, Action<float> onValue)
        {
            var tweener = Create<TweenFloat>()
                .SetClampAngle(clampAngle)
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenFloat;
            return tweener;
        }

        public static TweenFloat Value(Func<float> from, Func<float> to, float duration, Action<float> onValue)
        {
            var tweener = Value(from, to, duration, false, onValue);
            return tweener;
        }

        public static TweenFloat Value(Func<float> fromGetter, Func<float> toGetter, float duration, bool clampAngle, Action<float> onValue)
        {
            var tweener = Create<TweenFloat>()
                .SetClampAngle(clampAngle)
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenFloat;
            return tweener;
        }

        #endregion
    } 

    #endregion
}