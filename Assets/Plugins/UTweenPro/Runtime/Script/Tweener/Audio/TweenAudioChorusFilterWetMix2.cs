using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Wet Mix 2", "Audio")]
    [Serializable]
    public class TweenAudioChorusFilterWetMix2 : TweenValueFloat<AudioChorusFilter>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public override float Value
        {
            get => Target.wetMix2;
            set => Target.wetMix2 = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioChorusFilterWetMix2 WetMix2(AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterWetMix2, AudioChorusFilter, float>(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterWetMix2 WetMix2(AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterWetMix2, AudioChorusFilter, float>(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioChorusFilterExtension
    {
        public static TweenAudioChorusFilterWetMix2 TweenWetMix2(this AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = UTween.WetMix2(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterWetMix2 TweenWetMix2(this AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = UTween.WetMix2(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}