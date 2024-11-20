using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Wet Mix 1", "Audio")]
    [Serializable]
    public class TweenAudioChorusFilterWetMix1 : TweenValueFloat<AudioChorusFilter>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public override float Value
        {
            get => Target.wetMix1;
            set => Target.wetMix1 = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioChorusFilterWetMix1 WetMix1(AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterWetMix1, AudioChorusFilter, float>(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterWetMix1 WetMix1(AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterWetMix1, AudioChorusFilter, float>(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioChorusFilterExtension
    {
        public static TweenAudioChorusFilterWetMix1 TweenWetMix1(this AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = UTween.WetMix1(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterWetMix1 TweenWetMix1(this AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = UTween.WetMix1(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}