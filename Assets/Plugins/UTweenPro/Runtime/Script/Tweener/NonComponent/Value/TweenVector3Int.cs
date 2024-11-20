using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenVector3Int : TweenValueVector3Int<UnityEngine.Object>
    {
        public override Vector3Int Value { get; set; }
    }

    #region Extension
    
    public static partial class UTween
    {
        public static TweenVector3Int Value(Vector3Int from, Vector3Int to, float duration, Action<Vector3Int> onValue)
        {
            var tweener = Create<TweenVector3Int>()
                .SetFrom(from)
                .SetTo(to)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenVector3Int;
            return tweener;
        }

        public static TweenVector3Int Value(Func<Vector3Int> fromGetter, Func<Vector3Int> toGetter, float duration, Action<Vector3Int> onValue)
        {
            var tweener = Create<TweenVector3Int>()
                .SetFromGetter(fromGetter)
                .SetToGetter(toGetter)
                .SetValueSetter(onValue)
                .SetDuration(duration)
                .Play() as TweenVector3Int;
            return tweener;
        }
    } 

    #endregion
}