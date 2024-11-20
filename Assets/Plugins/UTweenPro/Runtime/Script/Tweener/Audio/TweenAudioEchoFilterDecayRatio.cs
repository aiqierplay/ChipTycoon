using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Decay Ratio", "Audio")]
    [Serializable]
    public class TweenAudioEchoFilterDecayRatio : TweenValueFloat<AudioEchoFilter>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public override float Value
        {
            get => Target.decayRatio;
            set => Target.decayRatio = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioEchoFilterWetMix DecayRatio(AudioEchoFilter audioEchoFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioEchoFilterWetMix, AudioEchoFilter, float>(audioEchoFilter, to, duration);
            return tweener;
        }

        public static TweenAudioEchoFilterWetMix DecayRatio(AudioEchoFilter audioEchoFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioEchoFilterWetMix, AudioEchoFilter, float>(audioEchoFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioEchoFilterExtension
    {
        public static TweenAudioEchoFilterWetMix TweenDecayRatio(this AudioEchoFilter audioEchoFilter, float to, float duration)
        {
            var tweener = UTween.DecayRatio(audioEchoFilter, to, duration);
            return tweener;
        }

        public static TweenAudioEchoFilterWetMix TweenDecayRatio(this AudioEchoFilter audioEchoFilter, float from, float to, float duration)
        {
            var tweener = UTween.DecayRatio(audioEchoFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}