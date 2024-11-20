using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenBounds : TweenValueBounds<UnityEngine.Object>
    {
        public override Bounds Value { get; set; }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenBounds Value(Bounds from, Bounds to, float duration, Action<Bounds> onValue)
        {
            var tweener = Create<TweenBounds>()
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenBounds;
            return tweener;
        }

        public static TweenBounds Value(Func<Bounds> fromGetter, Func<Bounds> toGetter, float duration, Action<Bounds> onValue)
        {
            var tweener = Create<TweenBounds>()
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenBounds;
            return tweener;
        }
    } 

    #endregion
}