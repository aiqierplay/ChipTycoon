using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Distortion Level", "Audio")]
    [Serializable]
    public class TweenAudioDistortionFilterDistortionLevel : TweenValueFloat<AudioDistortionFilter>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;
       

        public override float Value
        {
            get => Target.distortionLevel;
            set => Target.distortionLevel = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioDistortionFilterDistortionLevel DistortionLevel(AudioDistortionFilter audioDistortionFilter, float to, float duration)
        {
            var tweener = Play<TweenAudioDistortionFilterDistortionLevel, AudioDistortionFilter, float>(audioDistortionFilter, to, duration);
            return tweener;
        }

        public static TweenAudioDistortionFilterDistortionLevel DistortionLevel(AudioDistortionFilter audioDistortionFilter, float from, float to, float duration)
        {
            var tweener = Play<TweenAudioDistortionFilterDistortionLevel, AudioDistortionFilter, float>(audioDistortionFilter, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioDistortionFilterExtension
    {
        public static TweenAudioDistortionFilterDistortionLevel TweenDistortionLevel(this AudioDistortionFilter audioDistortionFilter, float to, float duration)
        {
            var tweener = UTween.DistortionLevel(audioDistortionFilter, to, duration);
            return tweener;
        }

        public static TweenAudioDistortionFilterDistortionLevel TweenDistortionLevel(this AudioDistortionFilter audioDistortionFilter, float from, float to, float duration)
        {
            var tweener = UTween.DistortionLevel(audioDistortionFilter, from, to, duration);
            return tweener;
        }
    }

    #endregion
}