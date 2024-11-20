using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Size With Current Anchors", nameof(RectTransform), -99)]
    [Serializable]
    public class TweenSizeWithCurrentAnchors : TweenValueVector2<RectTransform>
    {
        public override Vector2 Value
        {
            get => Target.rect.size;
            set
            {
                Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value.x);
                Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value.y);
            }
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenSizeWithCurrentAnchors SizeWithCurrentAnchors(RectTransform rectTransform, Vector2 to, float duration)
        {
            var tweener = Play<TweenSizeWithCurrentAnchors, RectTransform, Vector2>(rectTransform, to, duration);
            return tweener;
        }

        public static TweenSizeWithCurrentAnchors SizeWithCurrentAnchors(RectTransform rectTransform, Vector2 from, Vector2 to, float duration)
        {
            var tweener = Play<TweenSizeWithCurrentAnchors, RectTransform, Vector2>(rectTransform, from, to, duration);
            return tweener;
        }
    }

    public static partial class RectTransformExtension
    {
        public static TweenSizeWithCurrentAnchors TweenSizeWithCurrentAnchors(this RectTransform rectTransform, Vector2 to, float duration)
        {
            var tweener = UTween.SizeWithCurrentAnchors(rectTransform, to, duration);
            return tweener;
        }

        public static TweenSizeWithCurrentAnchors TweenSizeWithCurrentAnchors(this RectTransform rectTransform, Vector2 from, Vector2 to, float duration)
        {
            var tweener = UTween.SizeWithCurrentAnchors(rectTransform, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
