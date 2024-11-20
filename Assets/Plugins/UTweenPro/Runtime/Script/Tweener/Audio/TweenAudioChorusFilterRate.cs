using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Rate", "Audio")]
    [Serializable]
    public class TweenAudioChorusFilterRate : TweenValueFloat<AudioChorusFilter>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => 0f;
        public override bool RequireClampMax => true;
        public override float MaxValue => 20f;

        public override float Value
        {
            get => Target.rate;
            set => Target.rate = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioChorusFilterRate Rate(AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterRate, AudioChorusFilter, float>(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterRate Rate(AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterRate, AudioChorusFilter, float>(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioChorusFilterExtension
    {
        public static TweenAudioChorusFilterRate TweenRate(this AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = UTween.Rate(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterRate TweenRate(this AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = UTween.Rate(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}