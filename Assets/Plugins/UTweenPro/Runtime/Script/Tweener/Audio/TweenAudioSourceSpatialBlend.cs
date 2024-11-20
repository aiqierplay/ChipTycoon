using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Spatial Blend", "Audio")]
    [Serializable]
    public class TweenAudioSourceSpatialBlend : TweenValueFloat<AudioSource>
    {
        public override float Value
        {
            get => Target.spatialBlend;
            set => Target.spatialBlend = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioSourceSpatialBlend SpatialBlend(AudioSource audioSource, float to, float duration)
        {
            var tweener = Play<TweenAudioSourceSpatialBlend, AudioSource, float>(audioSource, to, duration);
            return tweener;
        }

        public static TweenAudioSourceSpatialBlend SpatialBlend(AudioSource audioSource, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioSourceSpatialBlend, AudioSource, float>(audioSource, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioSourceExtension
    {
        public static TweenAudioSourceSpatialBlend TweenSpatialBlend(this AudioSource audioSource, float to, float duration)
        {
            var tweener = UTween.SpatialBlend(audioSource, to, duration);
            return tweener;
        }

        public static TweenAudioSourceSpatialBlend TweenSpatialBlend(this AudioSource audioSource, float from, float to, float duration)
        {
            var tweener = UTween.SpatialBlend(audioSource, from, to, duration);
            return tweener;
        }
    }

    #endregion
}