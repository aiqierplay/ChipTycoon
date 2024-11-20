using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public partial class TweenVector2 : TweenValueVector2<UnityEngine.Object>
    {
        public bool ClampAngle;
        public override bool SampleAngle => ClampAngle;

        public override Vector2 Value { get; set; }

        public override void Reset()
        {
            base.Reset();
            ClampAngle = false;
        }
    }

    #region Extension

    public partial class TweenVector2 : TweenValueVector2<UnityEngine.Object>
    {
        public TweenVector2 SetClampAngle(bool clampAngle = true)
        {
            ClampAngle = clampAngle;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenVector2 Value(Vector2 from, Vector2 to, float duration, Action<Vector2> onValue)
        {
            var tweener = Value(from, to, duration, false, onValue);
            return tweener;
        }

        public static TweenVector2 Value(Vector2 from, Vector2 to, float duration, bool clampAngle, Action<Vector2> onValue)
        {
            var tweener = Create<TweenVector2>()
                .SetClampAngle(clampAngle)
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenVector2;
            return tweener;
        }

        public static TweenVector2 Value(Func<Vector2> from, Func<Vector2> to, float duration, Action<Vector2> onValue)
        {
            var tweener = Value(from, to, duration, false, onValue);
            return tweener;
        }

        public static TweenVector2 Value(Func<Vector2> fromGetter, Func<Vector2> toGetter, float duration, bool clampAngle, Action<Vector2> onValue)
        {
            var tweener = Create<TweenVector2>()
                .SetClampAngle(clampAngle)
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenVector2;
            return tweener;
        }
    } 

    #endregion
}