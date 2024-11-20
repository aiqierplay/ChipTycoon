using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Dry Mix", "Audio")]
    [Serializable]
    public class TweenAudioChorusFilterDryMix : TweenValueFloat<AudioChorusFilter>
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
        public static TweenAudioChorusFilterDryMix DryMix(AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterDryMix, AudioChorusFilter, float>(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterDryMix DryMix(AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioChorusFilterDryMix, AudioChorusFilter, float>(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioChorusFilterExtension
    {
        public static TweenAudioChorusFilterDryMix TweenDryMix(this AudioChorusFilter audioChorusFilter, float to, float duration)
        {
            var tweener = UTween.DryMix(audioChorusFilter, to, duration);
            return tweener;
        }

        public static TweenAudioChorusFilterDryMix TweenDryMix(this AudioChorusFilter audioChorusFilter, float from, float to, float duration)
        {
            var tweener = UTween.DryMix(audioChorusFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}