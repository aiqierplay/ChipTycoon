using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenRectInt : TweenValueRectInt<UnityEngine.Object>
    {
        public override RectInt Value { get; set; }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenRectInt Value(RectInt from, RectInt to, float duration, Action<RectInt> onValue)
        {
            var tweener = Create<TweenRectInt>()
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenRectInt;
            return tweener;
        }

        public static TweenRectInt Value(Func<RectInt> fromGetter, Func<RectInt> toGetter, float duration, Action<RectInt> onValue)
        {
            var tweener = Create<TweenRectInt>()
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenRectInt;
            return tweener;
        }
    } 

    #endregion
}