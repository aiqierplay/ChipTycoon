using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Stereo Pan", "Audio")]
    [Serializable]
    public class TweenAudioSourceStereoPan : TweenValueFloat<AudioSource>
    {
        public override bool RequireClampMin => true;
        public override float MinValue => -1;
        public override bool RequireClampMax => true;
        public override float MaxValue => 1;

        public override float Value
        {
            get => Target.panStereo;
            set => Target.panStereo = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioSourceStereoPan StereoPan(AudioSource audioSource, float to, float duration)
        {
            var tweener = Play<TweenAudioSourceStereoPan, AudioSource, float>(audioSource, to, duration);
            return tweener;
        }

        public static TweenAudioSourceStereoPan StereoPan(AudioSource audioSource, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioSourceStereoPan, AudioSource, float>(audioSource, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioSourceExtension
    {
        public static TweenAudioSourceStereoPan TweenStereoPan(this AudioSource audioSource, float to, float duration)
        {
            var tweener = UTween.StereoPan(audioSource, to, duration);
            return tweener;
        }

        public static TweenAudioSourceStereoPan TweenStereoPan(this AudioSource audioSource, float from, float to, float duration)
        {
            var tweener = UTween.StereoPan(audioSource, from, to, duration);
            return tweener;
        }
    }

    #endregion
}