using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenColor : TweenValueColor<UnityEngine.Object>
    {
        public override Color Value { get; set; }
    }

    #region Extension
    
    public static partial class UTween
    {
        #region Color
       
        public static TweenColor Value(Color from, Color to, float duration, Action<Color> onValue)
        {
            var tweener = Create<TweenColor>()
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenColor;
            return tweener;
        }

        public static TweenColor Value(Func<Color> fromGetter, Func<Color> toGetter, float duration, Action<Color> onValue)
        {
            var tweener = Create<TweenColor>()
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenColor;
            return tweener;
        } 

        #endregion

        #region Color32

        public static TweenColor Value(Color32 from, Color32 to, float duration, Action<Color32> onValue)
        {
            var tweener = Create<TweenColor>()
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(value => { onValue(value); })
                .SetDuration(duration)
                .Play() as TweenColor;
            return tweener;
        }

        public static TweenColor Value(Func<Color32> fromGetter, Func<Color32> toGetter, float duration, Action<Color32> onValue)
        {
            var tweener = Create<TweenColor>()
                .SetFromGetter(() => fromGetter())
                .SetToGetter(() => toGetter())
                .SetValueSetter(value => { onValue(value); })
                .SetDuration(duration)
                .Play() as TweenColor;
            return tweener;
        } 

        #endregion
    } 

    #endregion
}