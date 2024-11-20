/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AudioGroup.cs
//  Info     : 音频组 —— 用于管理一组音源
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Aya.Media.Audio
{
    public class AudioGroup : MonoBehaviour
    {
        #region Static Cache

        internal Dictionary<Transform, AudioAgent> AudioAgentDic = new Dictionary<Transform, AudioAgent>();
        internal Queue<AudioAgent> UnusedAudioAgents = new Queue<AudioAgent>();

        #endregion

        #region Public Serialize

        /// <summary>
        /// 音频组类型
        /// </summary>
        public AudioGroupType Type;

        /// <summary>
        /// 混音器
        /// </summary>
        public AudioMixer AudioMixer;

        /// <summary>
        /// 混音器分组
        /// </summary>
        public AudioMixerGroup AudioMixerGroup;

        #endregion

        #region Property

        /// <summary>
        /// 音频组名字
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = Type.ToString();
                }

                return _name;
            }
        }

        private string _name;

        /// <summary>
        /// 默认音频代理(不依附于任何对象)
        /// </summary>
        [NonSerialized] public AudioAgent DefaultAudioAgent;

        /// <summary>
        /// 音频代理列表(暂不使用，代理缓存在AudioAgentDic中）
        /// </summary>
        public List<AudioAgent> AudioAgents => null;

        /// <summary>
        /// 音频代理总数
        /// </summary>
        public int Count => AudioAgentDic.Count;

        #endregion

        #region Volume / Mute

        /// <summary>
        /// 获取 / 设置 组音量
        /// </summary>
        public float Volume
        {
            get => AudioMixer.GetVolume(Name);
            set
            {
                _originalVolume = value;
                AudioMixer.SetVolume(Name, value);
            }
        }

        private float _originalVolume = 1f;

        /// <summary>
        /// 获取 / 设置 静音状态
        /// </summary>
        public bool Mute
        {
            get => Volume > 0f;
            set
            {
                if (value)
                {
                    _originalVolume = Volume;
                    Volume = 0f;
                }
                else
                {
                    Volume = _originalVolume;
                }
            }
        }

        #endregion

        #region Init / Reset

        internal void Init()
        {
            AudioAgentDic.Clear();
            UnusedAudioAgents.Clear();
            DefaultAudioAgent = GetAudioAgent();
        }

        internal void Reset()
        {

        }

        #endregion

        #region Get / Release AudioAgent

        /// <summary>
        /// 获取依附于指定对象的音频代理
        /// </summary>
        /// <param name="bindTarget">附加目标对象(不指定则为默认音频代理)</param>
        /// <returns>结果</returns>
        public AudioAgent GetAudioAgent(object bindTarget = null)
        {
            var trans = bindTarget == null ? transform : bindTarget as Transform;
            if (trans == null) trans = transform;
            if (AudioAgentDic.TryGetValue(trans, out var audioAgent))
            {
                return audioAgent;
            }

            var title = "AudioAgent - " + (trans == transform ? "Default" : trans.name);
            if (UnusedAudioAgents.Count > 0)
            {
                audioAgent = UnusedAudioAgents.Dequeue();
                var audioAgentGo = audioAgent as MonoBehaviour;
                if (audioAgentGo != null)
                {
                    audioAgentGo.name = title;
                    audioAgentGo.gameObject.SetActive(true);
                }
            }
            else
            {
                var groupGo = new GameObject();
                groupGo.transform.SetParent(transform);
                groupGo.name = title;
                audioAgent = groupGo.AddComponent<AudioAgent>();
            }

            audioAgent.Init(this);
            if (trans != transform)
            {
                audioAgent.BindTarget(trans);
            }

            audioAgent.SpatialBlend = 0f;
            AudioAgentDic.Add(trans, audioAgent);
            return audioAgent;
        }

        /// <summary>
        /// 释放一个音频代理，进入待使用队列
        /// </summary>
        /// <param name="audioAgent"></param>
        public void ReleaseAudioAgent(AudioAgent audioAgent)
        {
            var match = false;
            if (AudioAgentDic.ContainsValue(audioAgent))
            {
                var key = default(Transform);
                foreach (var kv in AudioAgentDic)
                {
                    if (!kv.Value.Equals(audioAgent)) continue;
                    key = kv.Key;
                    match = true;
                    break;
                }

                if (match)
                {
                    AudioAgentDic.Remove(key);
                }
            }

            if (match)
            {
                audioAgent.Reset();
                var audioAgentGo = audioAgent as MonoBehaviour;
                audioAgentGo.gameObject.SetActive(false);
                UnusedAudioAgents.Enqueue(audioAgent);
            }
        }

        #endregion

        #region Play / Pause / Resume / Stop / PlayOneShot

        /// <summary>
        /// 使用默认音频代理播放指定音频资源(不可叠加播放)
        /// </summary>
        /// <param name="audioAsset">音频资源</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public void Play(object audioAsset, bool loop = false, float fadeInTime = 0f)
        {
            DefaultAudioAgent.Play(audioAsset, loop, fadeInTime);
        }

        /// <summary>
        /// 播放组内所有音频代理
        /// </summary>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public void Play(bool loop = false, float fadeInTime = 0f)
        {
            foreach (var audioAgent in AudioAgentDic.Values)
            {
                audioAgent.Play(loop, fadeInTime);
            }
        }

        /// <summary>
        /// 暂停组内所有音频代理
        /// </summary>
        /// <param name="fadeOutTime">淡出时间</param>
        public void Pause(float fadeOutTime = 0f)
        {
            foreach (var audioAgent in AudioAgentDic.Values)
            {
                audioAgent.Pause(fadeOutTime);
            }
        }

        /// <summary>
        /// 回复组内所有音频代理
        /// </summary>
        /// <param name="fadeInTime">淡入时间</param>
        public void Resume(float fadeInTime = 0f)
        {
            foreach (var audioAgent in AudioAgentDic.Values)
            {
                audioAgent.Resume();
            }
        }

        /// <summary>
        /// 停止组内所有音频代理
        /// </summary>
        /// <param name="fadeOutTime">淡出时间</param>
        public void Stop(float fadeOutTime = 0f)
        {
            foreach (var audioAgent in AudioAgentDic.Values)
            {
                audioAgent.Stop(fadeOutTime);
            }
        }

        /// <summary>
        /// 使用默认音频代理播放一次指定音频资源(可叠加播放)
        /// </summary>
        /// <param name="audioAsset">音频资源</param>
        /// <param name="volumeScale">音量缩放</param>
        public void PlayOneShot(object audioAsset, float volumeScale = 1f)
        {
            DefaultAudioAgent.PlayOneShot(audioAsset, volumeScale);
        }

        #endregion

        #region Monobehaviour

        internal void Awake()
        {
            AudioAgentDic = new Dictionary<Transform, AudioAgent>();
        }

        #endregion
    }
}