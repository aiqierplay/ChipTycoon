/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AudioAgent.cs
//  Info     : 音频代理 —— 用于管理单个音源
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Aya.Extension;

namespace Aya.Media.Audio
{
    public class AudioAgent : MonoBehaviour
    {
        #region Property

        [NonSerialized] public AudioSource AudioSource;
        [NonSerialized] public AudioClip AudioClip;

        [NonSerialized] public AudioGroup AudioGroup;
        [NonSerialized] public Transform ThisTrans;
        [NonSerialized] public Transform Target;
        [NonSerialized] public GameObject TargetGameObject;
        [NonSerialized] public bool IsAttaching;

        private float _targetVolume;
        private Coroutine _fadeVolumeCoroutine;

        #endregion

        #region Init / Reset

        public void Init(AudioGroup group)
        {
            PlayClips.Clear();
            AudioGroup = group;
            var audioGroup = AudioGroup as AudioGroup;
            if (AudioSource == null)
            {
                AudioSource = gameObject.AddComponent<AudioSource>();
            }

            AudioSource.outputAudioMixerGroup = audioGroup?.AudioMixerGroup;
            ThisTrans = transform;
            IsAttaching = false;
            Target = null;
            SpatialBlend = 0;
        }

        public void Reset()
        {
            AudioGroup = null;
            Target = null;
            if (AudioSource != null)
            {
                AudioSource.outputAudioMixerGroup = null;
            }
        }

        #endregion

        #region Bind Target

        /// <summary>
        /// 附加到指定对象，并自动设置成3D音源
        /// </summary>
        /// <param name="target">目标</param>
        public void BindTarget(object target)
        {
            IsAttaching = false;
            var trans = target as Transform;
            if (trans != null)
            {
                Target = trans;
                TargetGameObject = trans.gameObject;
                IsAttaching = true;
            }

            SpatialBlend = IsAttaching ? 1 : 0;
        }

        #endregion

        #region Volume / VolumeInGroup

        /// <summary>
        /// 获取/设置 音量
        /// </summary>
        public float Volume
        {
            get => AudioSource.volume;
            set => AudioSource.volume = Mathf.Clamp01(value);
        }

        /// <summary>
        /// 获取/设置 所在组的音量
        /// </summary>
        public float VolumeInGroup
        {
            get => AudioGroup.Volume;
            set => AudioGroup.Volume = Mathf.Clamp01(value);
        }

        #endregion

        #region Time / Length / NormalizedTime

        /// <summary>
        /// 播放时间
        /// </summary>
        public float Time => AudioSource.time;

        /// <summary>
        /// 音频时长
        /// </summary>
        public float Length => AudioSource.clip != null ? AudioSource.clip.length : 0f;

        /// <summary>
        /// 归一化时间
        /// </summary>
        public float NormalizedTime => Length < 1e-6f ? 0f : Time / Length;

        #endregion

        #region IsPlaying / Mute / Loop

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying => AudioSource.isPlaying;

        /// <summary>
        /// 获取/设置 静音状态
        /// </summary>
        public bool Mute
        {
            get => AudioSource.mute;
            set => AudioSource.mute = value;
        }

        /// <summary>
        /// 获取/设置 循环状态
        /// </summary>
        public bool Loop
        {
            get => AudioSource.loop;
            set => AudioSource.loop = value;
        }

        #endregion

        #region Priority / Pitch / PanStereo / SpatialBlend / MaxDistance / DopplerLevel

        /// <summary>
        /// 获取/设置 优先级。
        /// </summary>
        public int Priority
        {
            get => AudioSource.priority;
            set => AudioSource.priority = value;
        }

        /// <summary>
        /// 获取/设置 音调
        /// </summary>
        public float Pitch
        {
            get => AudioSource.pitch;
            set => AudioSource.pitch = value;
        }

        /// <summary>
        /// 获取/设置 立体声声相
        /// </summary>
        public float PanStereo
        {
            get => AudioSource.panStereo;
            set => AudioSource.panStereo = value;
        }

        /// <summary>
        /// 获取/设置 空间混合量 (0 - 2D / 1 - 3D)
        /// </summary>
        public float SpatialBlend
        {
            get => AudioSource.spatialBlend;
            set => AudioSource.spatialBlend = value;
        }

        /// <summary>
        /// 获取/设置 最大距离
        /// </summary>
        public float MaxDistance
        {
            get => AudioSource.maxDistance;
            set => AudioSource.maxDistance = value;
        }

        /// <summary>
        /// 获取/设置 多普勒等级
        /// </summary>
        public float DopplerLevel
        {
            get => AudioSource.dopplerLevel;
            set => AudioSource.dopplerLevel = value;
        }

        #endregion

        #region Play / Pause / Resume / Stop / PlayOneShot

        /// <summary>
        /// 播放指定音频资源(不可叠加播放)
        /// </summary>
        /// <param name="audioAsset">音频资源</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public void Play(object audioAsset, bool loop = false, float fadeInTime = 0f)
        {
            if (audioAsset == null) throw new NullReferenceException("AudioAsset is NULL");
            var clip = audioAsset as AudioClip;
            if (clip != null)
            {
                AudioSource.clip = clip;
                AudioSource.loop = loop;
                FadeVolume(AudioSource.Play, null, fadeInTime, 0f, 1f);
            }
            else
            {
                throw new NullReferenceException("AudioClip is NULL");
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public void Play(bool loop = false, float fadeInTime = 0f)
        {
            AudioSource.loop = loop;
            FadeVolume(AudioSource.Play, null, fadeInTime, 0f, 1f);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="fadeOutTime">淡出时间</param>
        public void Pause(float fadeOutTime = 0f)
        {
            FadeVolume(null, AudioSource.Pause, fadeOutTime, Volume, 0f);
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="fadeInTime">淡入时间</param>
        public void Resume(float fadeInTime = 0f)
        {
            FadeVolume(AudioSource.UnPause, null, fadeInTime, 0f, 1f);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="fadeOutTime">淡出时间</param>
        public void Stop(float fadeOutTime = 0f)
        {
            FadeVolume(null, AudioSource.Stop, fadeOutTime, Volume, 0f);
        }

        /// <summary>
        /// 播放一次指定音频资源(可叠加播放)
        /// </summary>
        /// <param name="audioAsset">音频资源</param>
        /// <param name="volumeScale">音量缩放</param>
        public void PlayOneShot(object audioAsset, float volumeScale = 1f)
        {
            if (audioAsset == null) throw new NullReferenceException("AudioAsset is NULL");
            var clip = audioAsset as AudioClip;
            if (clip == null) throw new NullReferenceException("AudioClip is NULL");
            AudioSource.PlayOneShot(clip, volumeScale);
            PlayClips.Add(clip);
            this.ExecuteDelay(() => { PlayClips.Remove(clip); }, clip.length * Audio.ClipLengthMultiple);
        }

        public List<AudioClip> PlayClips = new List<AudioClip>();

        #endregion

        #region Monobehaviour

        internal void Update()
        {
            if (!IsAttaching) return;
            if (Target != null && TargetGameObject.activeSelf)
            {
                // 跟随物体
                ThisTrans.position = Target.position;
            }
            else
            {
                // 回收音频代理
                AudioGroup.ReleaseAudioAgent(this);
            }
        }

        #endregion

        #region Fade In / Fade Out

        /// <summary>
        /// 过度音量(淡入/淡出)
        /// </summary>
        /// <param name="before">前处理</param>
        /// <param name="after">后处理</param>
        /// <param name="fadeTime">过度时间</param>
        /// <param name="fromVolume">开始音量</param>
        /// <param name="toVolume">结束音量</param>
        public void FadeVolume(Action before, Action after, float fadeTime, float fromVolume, float toVolume)
        {
            if (fadeTime < 1e-6f)
            {
                var originalVolume = Volume;
                before?.Invoke();
                Volume = toVolume;
                after?.Invoke();
                Volume = originalVolume;
            }
            else
            {
                if (_fadeVolumeCoroutine != null)
                {
                    StopCoroutine(_fadeVolumeCoroutine);
                }

                _fadeVolumeCoroutine = StartCoroutine(FadeVolumeCoroutine(before, after, fadeTime, fromVolume, toVolume));
            }
        }

        protected IEnumerator FadeVolumeCoroutine(Action before, Action after, float fadeTime, float fromVolume, float toVolume)
        {
            before?.Invoke();
            var timer = 0f;
            while (timer < fadeTime)
            {
                timer += UnityEngine.Time.deltaTime;
                var factor = timer / fadeTime;
                var volume = Mathf.Lerp(fromVolume, toVolume, factor);
                Volume = volume;
                yield return null;
            }

            Volume = toVolume;
            after?.Invoke();
            _fadeVolumeCoroutine = null;
        }

        #endregion
    }
}