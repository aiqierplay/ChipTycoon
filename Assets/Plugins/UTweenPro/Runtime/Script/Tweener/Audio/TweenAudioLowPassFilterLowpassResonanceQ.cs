using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Lowpass Resonance Q", "Audio")]
    [Serializable]
    public class TweenAudioLowPassFilterLowpassResonanceQ : TweenValueFloat<AudioLowPassFilter>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => 1f;
        public override bool RequireClampMax => true;
        public override float MaxValue => 10f;

        public override float Value
        {
            get => Target.lowpassResonanceQ;
            set => Target.lowpassResonanceQ = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioLowPassFilterLowpassResonanceQ LowpassResonanceQ(AudioLowPassFilter audioLowPassFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioLowPassFilterLowpassResonanceQ, AudioLowPassFilter, float>(audioLowPassFilter, to, duration);
            return tweener;
        }

        public static TweenAudioLowPassFilterLowpassResonanceQ LowpassResonanceQ(AudioLowPassFilter audioLowPassFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioLowPassFilterLowpassResonanceQ, AudioLowPassFilter, float>(audioLowPassFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioLowPassFilterExtension
    {
        public static TweenAudioLowPassFilterLowpassResonanceQ TweenLowpassResonanceQ(this AudioLowPassFilter audioLowPassFilter, float to, float duration)
        {
            var tweener = UTween.LowpassResonanceQ(audioLowPassFilter, to, duration);
            return tweener;
        }

        public static TweenAudioLowPassFilterLowpassResonanceQ TweenLowpassResonanceQ(this AudioLowPassFilter audioLowPassFilter, float from, float to, float duration)
        {
            var tweener = UTween.LowpassResonanceQ(audioLowPassFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}