using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener(" Reverb Zone Mix", "Audio")]
    [Serializable]
    public class TweenAudioSourceReverbZoneMix : TweenValueFloat<AudioSource>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => 0f;
        public override bool RequireClampMax => true;
        public override float MaxValue => 1.1f;

        public override float Value
        {
            get => Target.reverbZoneMix;
            set => Target.reverbZoneMix = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioSourceReverbZoneMix ReverbZoneMix(AudioSource audioSource, float to, float duration)
        {
            var tweener = Play<TweenAudioSourceReverbZoneMix, AudioSource, float>(audioSource, to, duration);
            return tweener;
        }

        public static TweenAudioSourceReverbZoneMix ReverbZoneMix(AudioSource audioSource, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioSourceReverbZoneMix, AudioSource, float>(audioSource, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioSourceExtension
    {
        public static TweenAudioSourceReverbZoneMix TweenReverbZoneMix(this AudioSource audioSource, float to, float duration)
        {
            var tweener = UTween.ReverbZoneMix(audioSource, to, duration);
            return tweener;
        }

        public static TweenAudioSourceReverbZoneMix TweenReverbZoneMix(this AudioSource audioSource, float from, float to, float duration)
        {
            var tweener = UTween.ReverbZoneMix(audioSource, from, to, duration);
            return tweener;
        }
    }

    #endregion
}