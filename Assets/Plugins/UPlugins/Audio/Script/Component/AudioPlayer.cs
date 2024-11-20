/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AudioPlayer.cs
//  Info     : 音频播放组件
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;

namespace Aya.Media.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        public enum PlayModeType
        {
            Sequence = 1,
            Random = 2,
        }

        [Header("Audio Resources")]
        public List<AudioClip> AudioClips;

        [Header("Parameter")]
        public AudioGroupType AudioGroup = AudioGroupType.SE;
        public PlayModeType PlayMode = PlayModeType.Random;
        public bool BindTarget = true;
        public bool Loop = true;
        public bool AutoPlayOnEnable = true;
        public float IntervalTimeMin;
        public float IntervalTimeMax;
        public bool TimeScale = true;

        public bool IsPlaying { get; protected set; }

        private List<AudioClip> _clips;
        private Coroutine _playCoroutine;
        private int _loopCount;
        private AudioClip CurrentClip
        {
            get
            {
                if (_clips == null || _clips.Count == 0) return null;
                return _clips[0];
            }
        }

        public void Awake()
        {

        }

        public void OnEnable()
        {
            IsPlaying = false;
            if (AutoPlayOnEnable)
            {
                Play();
            }
        }

        public void OnDisable()
        {
            Stop();
        }

        protected IEnumerator PlayCoroutine()
        {
            _loopCount = 0;
            do
            {
                var clip = CurrentClip;
                if (clip == null)
                {
                    if (AudioClips.Count == 0)
                    {
                        IsPlaying = false;
                        yield break;
                    }

                    _loopCount++;
                    if (_loopCount > 1 && !Loop)
                    {
                        IsPlaying = false;
                        yield break;
                    }

                    _clips = new List<AudioClip>();
                    _clips.AddRange(AudioClips);
                    if (PlayMode == PlayModeType.Random)
                    {
                        _clips.RandSort();
                    }

                    clip = CurrentClip;
                }
                _clips.Remove(clip);

                Audio.PlayOneShot(AudioGroup, (BindTarget ? transform : null), clip);
                var waitTime = clip.length + Random.Range(IntervalTimeMin, IntervalTimeMax);
                if (TimeScale)
                {
                    yield return new WaitForSeconds(waitTime);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(waitTime);
                }
            } while (true);
        }

        public void Play()
        {
            IsPlaying = true;
            _playCoroutine = StartCoroutine(PlayCoroutine());
        }

        public void Stop()
        {
            IsPlaying = false;
            if (_playCoroutine == null) return;
            StopCoroutine(_playCoroutine);
            _playCoroutine = null;
        }
    }
}
