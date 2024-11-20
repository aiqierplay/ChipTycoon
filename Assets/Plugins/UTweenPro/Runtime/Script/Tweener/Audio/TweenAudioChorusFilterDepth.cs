using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Depth", "Audio")]
    [Serializable]
    public class TweenAudioChorusFilterDepth : TweenValueFloat<AudioChorusFilter>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public override float Value
        {
            get => Target.depth;
            set => Target.depth = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioChorusFilterDepth Depth(AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterDepth, AudioChorusFilter, float>(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterDepth Depth(AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterDepth, AudioChorusFilter, float>(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioChorusFilterExtension
    {
        public static TweenAudioChorusFilterDepth TweenDepth(this AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = UTween.Depth(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterDepth TweenDepth(this AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = UTween.Depth(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}