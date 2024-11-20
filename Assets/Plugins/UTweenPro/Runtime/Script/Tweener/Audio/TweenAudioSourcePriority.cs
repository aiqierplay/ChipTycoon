using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Priority", "Audio")]
    [Serializable]
    public class TweenAudioSourcePriority : TweenValueInteger<AudioSource>
    {
        public override bool RequireClampMin => true;
        public override int MinValue => 0;
        public override bool RequireClampMax => true;
        public override int MaxValue => 256;

        public override int Value
        {
            get => Target.priority;
            set => Target.priority = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenAudioSourcePriority Priority(AudioSource audioSource, int to, float duration)
        {
            var tweener = Play<TweenAudioSourcePriority, AudioSource, int>(audioSource, to, duration);
            return tweener;
        }

        public static TweenAudioSourcePriority Priority(AudioSource audioSource, int from, int to, float duration)
        {
            var tweener = Play<TweenAudioSourcePriority, AudioSource, int>(audioSource, from, to, duration);
            return tweener;
        }
    }

    public static partial class AudioSourceExtension
    {
        public static TweenAudioSourcePriority TweenPriority(this AudioSource audioSource, int to, float duration)
        {
            var tweener = UTween.Priority(audioSource, to, duration);
            return tweener;
        }

        public static TweenAudioSourcePriority TweenPriority(this AudioSource audioSource, int from, int to, float duration)
        {
            var tweener = UTween.Priority(audioSource, from, to, duration);
            return tweener;
        }
    }

    #endregion
}