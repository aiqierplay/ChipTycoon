using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Height With Current Anchors", nameof(RectTransform), -97)]
    [Serializable]
    public class TweenHeightWithCurrentAnchors : TweenValueFloat<RectTransform>
    {
        public override float Value
        {
            get => Target.rect.size.y;
            set => Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenHeightWithCurrentAnchors HeightWithCurrentAnchors(RectTransform rectTransform, float to, float duration)
        {
            var tweener = Play<TweenHeightWithCurrentAnchors, RectTransform, float>(rectTransform, to, duration);
            return tweener;
        }

        public static TweenHeightWithCurrentAnchors HeightWithCurrentAnchors(RectTransform rectTransform, float from, float to, float duration)
        {
            var tweener = Play<TweenHeightWithCurrentAnchors, RectTransform, float>(rectTransform, from, to, duration);
            return tweener;
        }
    }

    public static partial class RectTransformExtension
    {
        public static TweenHeightWithCurrentAnchors TweenHeightWithCurrentAnchors(this RectTransform rectTransform, float to, float duration)
        {
            var tweener = UTween.HeightWithCurrentAnchors(rectTransform, to, duration);
            return tweener;
        }

        public static TweenHeightWithCurrentAnchors TweenHeightWithCurrentAnchors(this RectTransform rectTransform, float from, float to, float duration)
        {
            var tweener = UTween.HeightWithCurrentAnchors(rectTransform, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
