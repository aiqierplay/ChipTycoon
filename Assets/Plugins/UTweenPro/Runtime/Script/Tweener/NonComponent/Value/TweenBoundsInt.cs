using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenBoundsInt : TweenValueBoundsInt<UnityEngine.Object>
    {
        public override BoundsInt Value { get; set; }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenBoundsInt Value(BoundsInt from, BoundsInt to, float duration, Action<BoundsInt> onValue)
        {
            var tweener = Create<TweenBoundsInt>()
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenBoundsInt;
            return tweener;
        }

        public static TweenBoundsInt Value(Func<BoundsInt> fromGetter, Func<BoundsInt> toGetter, float duration, Action<BoundsInt> onValue)
        {
            var tweener = Create<TweenBoundsInt>()
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenBoundsInt;
            return tweener;
        }
    } 

    #endregion
}