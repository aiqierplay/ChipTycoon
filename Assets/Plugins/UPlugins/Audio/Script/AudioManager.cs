/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AudioManager.cs
//  Info     : 音频管理器
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
using Aya.Extension;

namespace Aya.Media.Audio
{
    public class AudioManager : MonoBehaviour
    {
        #region Instance

        protected static AudioManager Instance;

        internal static AudioManager Ins
        {
            get
            {
                if (Instance != null) return Instance;
                Instance = (AudioManager) FindObjectOfType(typeof(AudioManager));
                return Instance;
            }
        }

        protected AudioManager()
        {
        }

        #endregion

        #region Public Serialize

        /// <summary>
        /// 主混音器
        /// </summary>
        public AudioMixer AudioMixer;

        /// <summary>
        /// 音频组列表
        /// </summary>
        public List<AudioMixerGroupMap> GroupMaps;

        #endregion

        #region Static Cache

        internal static Dictionary<AudioGroupType, AudioGroup> AudioGroupDic;
        internal static Dictionary<AudioGroupType, AudioMixerGroup> AudioMixerGroupDic;

        /// <summary>
        /// Master 音频组<para/>
        /// 全局音量控制组，一般不用于播放声音。
        /// </summary>
        [NonSerialized] public AudioGroup Master;

        /// <summary>
        /// BGM 音频组<para/>
        /// Background Music<para/>
        /// 主要用于播放循环背景音乐
        /// </summary>
        [NonSerialized] public AudioGroup BGM;

        /// <summary>
        /// BGS 音频组<para/>
        /// Background Sound<para/>
        /// 与BGM一起播放的环境音: 水声，风声，瀑布声等
        /// </summary>
        [NonSerialized] public AudioGroup BGS;

        /// <summary>
        /// SE 音频组<para/>
        /// Sound Effect<para/>
        /// 短音效，比如武器打击，技能效果，UI音效
        /// </summary>
        [NonSerialized] public AudioGroup SE;

        /// <summary>
        /// ME 音频组<para/>
        /// Music Effect<para/>
        /// 长音效，播放时BGM暂时停止，比如游戏开始，胜利，失败的音效
        /// </summary>
        [NonSerialized] public AudioGroup ME;

        /// <summary>
        /// Voice 音频组<para/>
        /// 人声，语音
        /// </summary>
        [NonSerialized] public AudioGroup Voice;

        #endregion

        #region Init

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        internal static void Init()
        {
            if (Ins == null) return;
            AudioGroupDic = new Dictionary<AudioGroupType, AudioGroup>();
            AudioMixerGroupDic = Ins.GroupMaps.ToDictionary(map => map.Type, map => map.Group);

            Ins.Master = Ins.GetAudioGroup(AudioGroupType.Master);
            Ins.BGM = Ins.GetAudioGroup(AudioGroupType.BGM);
            Ins.BGS = Ins.GetAudioGroup(AudioGroupType.BGS);
            Ins.SE = Ins.GetAudioGroup(AudioGroupType.SE);
            Ins.ME = Ins.GetAudioGroup(AudioGroupType.ME);
            Ins.Voice = Ins.GetAudioGroup(AudioGroupType.Voice);
        }

        #endregion

        #region Get AudioGroup

        /// <summary>
        /// 获取指定类型的音频组，不存在则创建
        /// </summary>
        /// <param name="type">音频组类型</param>
        /// <returns>结果</returns>
        public AudioGroup GetAudioGroup(AudioGroupType type)
        {
            if (AudioGroupDic.TryGetValue(type, out var audioGroup))
            {
                return audioGroup;
            }

            var groupGo = new GameObject();
            groupGo.transform.SetParent(transform);
            groupGo.name = "AudioGroup - " + type;
            var audioGroupGo = groupGo.AddComponent<AudioGroup>();
            audioGroupGo.Type = type;
            audioGroupGo.AudioMixer = AudioMixer;
            audioGroupGo.AudioMixerGroup = AudioMixerGroupDic.GetValue(type);
            audioGroupGo.Init();
            audioGroup = audioGroupGo;
            AudioGroupDic.Add(type, audioGroup);
            return audioGroup;
        }

        #endregion
    }
}