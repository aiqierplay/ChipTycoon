using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Dry Mix", "Audio")]
    [Serializable]
    public class TweenAudioEchoFilterDryMix : TweenValueFloat<AudioEchoFilter>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public override float Value
        {
            get => Target.dryMix;
            set => Target.dryMix = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioEchoFilterWetMix DryMix(AudioEchoFilter audioEchoFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioEchoFilterWetMix, AudioEchoFilter, float>(audioEchoFilter, to, duration);
            return tweener;
        }

        public static TweenAudioEchoFilterWetMix DryMix(AudioEchoFilter audioEchoFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioEchoFilterWetMix, AudioEchoFilter, float>(audioEchoFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioEchoFilterExtension
    {
        public static TweenAudioEchoFilterWetMix TweenDryMix(this AudioEchoFilter audioEchoFilter, float to, float duration)
        {
            var tweener = UTween.DryMix(audioEchoFilter, to, duration);
            return tweener;
        }

        public static TweenAudioEchoFilterWetMix TweenDryMix(this AudioEchoFilter audioEchoFilter, float from, float to, float duration)
        {
            var tweener = UTween.DryMix(audioEchoFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}