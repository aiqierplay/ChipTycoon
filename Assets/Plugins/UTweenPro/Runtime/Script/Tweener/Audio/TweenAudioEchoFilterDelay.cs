using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Delay", "Audio")]
    [Serializable]
    public class TweenAudioEchoFilterDelay : TweenValueFloat<AudioEchoFilter>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => 10f;
        public override bool RequireClampMax => true;
        public override float MaxValue => 500f;

        public override float Value
        {
            get => Target.delay;
            set => Target.delay = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioEchoFilterWetMix Delay(AudioEchoFilter audioEchoFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioEchoFilterWetMix, AudioEchoFilter, float>(audioEchoFilter, to, duration);
            return tweener;
        }

        public static TweenAudioEchoFilterWetMix Delay(AudioEchoFilter audioEchoFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioEchoFilterWetMix, AudioEchoFilter, float>(audioEchoFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioEchoFilterExtension
    {
        public static TweenAudioEchoFilterWetMix TweenDelay(this AudioEchoFilter audioEchoFilter, float to, float duration)
        {
            var tweener = UTween.Delay(audioEchoFilter, to, duration);
            return tweener;
        }

        public static TweenAudioEchoFilterWetMix TweenDelay(this AudioEchoFilter audioEchoFilter, float from, float to, float duration)
        {
            var tweener = UTween.Delay(audioEchoFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}