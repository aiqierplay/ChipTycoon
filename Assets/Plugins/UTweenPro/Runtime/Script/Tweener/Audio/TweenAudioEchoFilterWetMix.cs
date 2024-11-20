using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Wet Mix", "Audio")]
    [Serializable]
    public class TweenAudioEchoFilterWetMix : TweenValueFloat<AudioEchoFilter>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public override float Value
        {
            get => Target.wetMix;
            set => Target.wetMix = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioEchoFilterWetMix WetMix(AudioEchoFilter audioEchoFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioEchoFilterWetMix, AudioEchoFilter, float>(audioEchoFilter, to, duration);
            return tweener;
        }

        public static TweenAudioEchoFilterWetMix WetMix(AudioEchoFilter audioEchoFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioEchoFilterWetMix, AudioEchoFilter, float>(audioEchoFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioEchoFilterExtension
    {
        public static TweenAudioEchoFilterWetMix TweenWetMix(this AudioEchoFilter audioEchoFilter, float to, float duration)
        {
            var tweener = UTween.WetMix(audioEchoFilter, to, duration);
            return tweener;
        }

        public static TweenAudioEchoFilterWetMix TweenWetMix(this AudioEchoFilter audioEchoFilter, float from, float to, float duration)
        {
            var tweener = UTween.WetMix(audioEchoFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}