using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public partial class TweenVector3 : TweenValueVector3<UnityEngine.Object>
    {
        public bool ClampAngle;
        public override bool SampleAngle => ClampAngle;

        public override Vector3 Value { get; set; }

        public override void Reset()
        {
            base.Reset();
            ClampAngle = false;
        }
    }

    #region Extension

    public partial class TweenVector3 : TweenValueVector3<UnityEngine.Object>
    {
        public TweenVector3 SetClampAngle(bool clampAngle = true)
        {
            ClampAngle = clampAngle;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenVector3 Value(Vector3 from, Vector3 to, float duration, Action<Vector3> onValue)
        {
            var tweener = Value(from, to, duration, false, onValue);
            return tweener;
        }

        public static TweenVector3 Value(Vector3 from, Vector3 to, float duration, bool clampAngle, Action<Vector3> onValue)
        {
            var tweener = Create<TweenVector3>()
                .SetClampAngle(clampAngle)
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenVector3;
            return tweener;
        }

        public static TweenVector3 Value(Func<Vector3> from, Func<Vector3> to, float duration, Action<Vector3> onValue)
        {
            var tweener = Value(from, to, duration, false, onValue);
            return tweener;
        }

        public static TweenVector3 Value(Func<Vector3> fromGetter, Func<Vector3> toGetter, float duration, bool clampAngle, Action<Vector3> onValue)
        {
            var tweener = Create<TweenVector3>()
                .SetClampAngle(clampAngle)
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenVector3;
            return tweener;
        }
    } 

    #endregion
}