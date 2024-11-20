using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Cutoff Frequency", "Audio")]
    [Serializable]
    public class TweenAudioLowPassFilterCutoffFrequency : TweenValueFloat<AudioLowPassFilter>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => 10f;
        public override bool RequireClampMax => true;
        public override float MaxValue => 22000f;

        public override float Value
        {
            get => Target.cutoffFrequency;
            set => Target.cutoffFrequency = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioLowPassFilterCutoffFrequency CutoffFrequency(AudioLowPassFilter audioLowPassFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioLowPassFilterCutoffFrequency, AudioLowPassFilter, float>(audioLowPassFilter, to, duration);
            return tweener;
        }

        public static TweenAudioLowPassFilterCutoffFrequency CutoffFrequency(AudioLowPassFilter audioLowPassFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioLowPassFilterCutoffFrequency, AudioLowPassFilter, float>(audioLowPassFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioLowPassFilterExtension
    {
        public static TweenAudioLowPassFilterCutoffFrequency TweenCutoffFrequency(this AudioLowPassFilter audioLowPassFilter, float to, float duration)
        {
            var tweener = UTween.CutoffFrequency(audioLowPassFilter, to, duration);
            return tweener;
        }

        public static TweenAudioLowPassFilterCutoffFrequency TweenCutoffFrequency(this AudioLowPassFilter audioLowPassFilter, float from, float to, float duration)
        {
            var tweener = UTween.CutoffFrequency(audioLowPassFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}