using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Wet Mix 3", "Audio")]
    [Serializable]
    public class TweenAudioChorusFilterWetMix3 : TweenValueFloat<AudioChorusFilter>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public override float Value
        {
            get => Target.wetMix3;
            set => Target.wetMix3 = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioChorusFilterWetMix3 WetMix3(AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterWetMix3, AudioChorusFilter, float>(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterWetMix3 WetMix3(AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterWetMix3, AudioChorusFilter, float>(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioChorusFilterExtension
    {
        public static TweenAudioChorusFilterWetMix3 TweenWetMix3(this AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = UTween.WetMix3(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterWetMix3 TweenWetMix3(this AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = UTween.WetMix3(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}