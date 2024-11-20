using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Cutoff Frequency", "Audio")]
    [Serializable]
    public class TweenAudioHighPassFilterCutoffFrequency : TweenValueFloat<AudioHighPassFilter>
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
        public static TweenAudioHighPassFilterCutoffFrequency CutoffFrequency(AudioHighPassFilter audioHighPassFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioHighPassFilterCutoffFrequency, AudioHighPassFilter, float>(audioHighPassFilter, to, duration);
            return tweener;
        }

        public static TweenAudioHighPassFilterCutoffFrequency CutoffFrequency(AudioHighPassFilter audioHighPassFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioHighPassFilterCutoffFrequency, AudioHighPassFilter, float>(audioHighPassFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioHighPassFilterExtension
    {
        public static TweenAudioHighPassFilterCutoffFrequency TweenCutoffFrequency(this AudioHighPassFilter audioHighPassFilter, float to, float duration)
        {
            var tweener = UTween.CutoffFrequency(audioHighPassFilter, to, duration);
            return tweener;
        }

        public static TweenAudioHighPassFilterCutoffFrequency TweenCutoffFrequency(this AudioHighPassFilter audioHighPassFilter, float from, float to, float duration)
        {
            var tweener = UTween.CutoffFrequency(audioHighPassFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}