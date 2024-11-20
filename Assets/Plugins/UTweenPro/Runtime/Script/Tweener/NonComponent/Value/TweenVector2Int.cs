using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenVector2Int : TweenValueVector2Int<UnityEngine.Object>
    {
        public override Vector2Int Value { get; set; }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenVector2Int Value(Vector2Int from, Vector2Int to, float duration, Action<Vector2Int> onValue)
        {
            var tweener = Create<TweenVector2Int>()
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenVector2Int;
            return tweener;
        }

        public static TweenVector2Int Value(Func<Vector2Int> fromGetter, Func<Vector2Int> toGetter, float duration, Action<Vector2Int> onValue)
        {
            var tweener = Create<TweenVector2Int>()
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenVector2Int;
            return tweener;
        }
    } 

    #endregion
}