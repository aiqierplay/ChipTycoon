using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Highpass Resonance Q", "Audio")]
    [Serializable]
    public class TweenAudioHighPassFilterHighpassResonanceQ : TweenValueFloat<AudioHighPassFilter>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => 1f;
        public override bool RequireClampMax => true;
        public override float MaxValue => 10f;

        public override float Value
        {
            get => Target.highpassResonanceQ;
            set => Target.highpassResonanceQ = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioHighPassFilterHighpassResonanceQ HighpassResonanceQ(AudioHighPassFilter audioHighPassFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioHighPassFilterHighpassResonanceQ, AudioHighPassFilter, float>(audioHighPassFilter, to, duration);
            return tweener;
        }

        public static TweenAudioHighPassFilterHighpassResonanceQ HighpassResonanceQ(AudioHighPassFilter audioHighPassFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioHighPassFilterHighpassResonanceQ, AudioHighPassFilter, float>(audioHighPassFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioHighPassFilterExtension
    {
        public static TweenAudioHighPassFilterHighpassResonanceQ TweenHighpassResonanceQ(this AudioHighPassFilter audioHighPassFilter, float to, float duration)
        {
            var tweener = UTween.HighpassResonanceQ(audioHighPassFilter, to, duration);
            return tweener;
        }

        public static TweenAudioHighPassFilterHighpassResonanceQ TweenHighpassResonanceQ(this AudioHighPassFilter audioHighPassFilter, float from, float to, float duration)
        {
            var tweener = UTween.HighpassResonanceQ(audioHighPassFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}