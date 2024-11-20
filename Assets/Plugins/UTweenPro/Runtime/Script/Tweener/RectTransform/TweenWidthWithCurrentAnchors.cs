using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Width With Current Anchors", nameof(RectTransform), -98)]
    [Serializable]
    public class TweenWidthWithCurrentAnchors : TweenValueFloat<RectTransform>
    {
        public override float Value
        {
            get => Target.rect.size.x;
            set => Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenWidthWithCurrentAnchors WidthWithCurrentAnchors(RectTransform rectTransform, float to, float duration)
        {
            var tweener = Play<TweenWidthWithCurrentAnchors, RectTransform, float>(rectTransform, to, duration);
            return tweener;
        }

        public static TweenWidthWithCurrentAnchors WidthWithCurrentAnchors(RectTransform rectTransform, float from, float to, float duration)
        {
            var tweener = Play<TweenWidthWithCurrentAnchors, RectTransform, float>(rectTransform, from, to, duration);
            return tweener;
        }
    }

    public static partial class RectTransformExtension
    {
        public static TweenWidthWithCurrentAnchors TweenWidthWithCurrentAnchors(this RectTransform rectTransform, float to, float duration)
        {
            var tweener = UTween.WidthWithCurrentAnchors(rectTransform, to, duration);
            return tweener;
        }

        public static TweenWidthWithCurrentAnchors TweenWidthWithCurrentAnchors(this RectTransform rectTransform, float from, float to, float duration)
        {
            var tweener = UTween.WidthWithCurrentAnchors(rectTransform, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
