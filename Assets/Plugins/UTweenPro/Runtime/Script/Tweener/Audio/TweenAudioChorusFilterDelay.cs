using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Delay", "Audio")]
    [Serializable]
    public class TweenAudioChorusFilterDelay : TweenValueFloat<AudioChorusFilter>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => 0.1f;
        public override bool RequireClampMax => true;
        public override float MaxValue => 100f;

        public override float Value
        {
            get => Target.delay;
            set => Target.delay = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioChorusFilterDelay Delay(AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterDelay, AudioChorusFilter, float>(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterDelay Delay(AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterDelay, AudioChorusFilter, float>(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioChorusFilterExtension
    {
        public static TweenAudioChorusFilterDelay TweenDelay(this AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = UTween.Delay(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterDelay TweenDelay(this AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = UTween.Delay(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}