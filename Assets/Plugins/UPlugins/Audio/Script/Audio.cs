/////////////////////////////////////////////////////////////////////////////
//
//  Script   : Audio.cs
//  Info     : 音频管理快速调用接口
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace Aya.Media.Audio
{
    public static class Audio
    {
        #region Default AudioGroup

        /// <summary>
        /// Master 音频组<para/>
        /// 全局音量控制组，一般不用于播放声音。
        /// </summary>
        public static AudioGroup Master => AudioManager.Ins.Master;

        /// <summary>
        /// BGM 音频组<para/>
        /// Background Music<para/>
        /// 主要用于播放循环背景音乐
        /// </summary>
        public static AudioGroup BGM => AudioManager.Ins.BGM;

        /// <summary>
        /// BGS 音频组<para/>
        /// Background Sound<para/>
        /// 与BGM一起播放的环境音: 水声，风声，瀑布声等
        /// </summary>
        public static AudioGroup BGS => AudioManager.Ins.BGS;

        /// <summary>
        /// SE 音频组<para/>
        /// Sound Effect<para/>
        /// 短音效，比如武器打击，技能效果，UI音效
        /// </summary>
        public static AudioGroup SE => AudioManager.Ins.SE;

        /// <summary>
        /// ME 音频组<para/>
        /// Music Effect<para/>
        /// 长音效，播放时BGM暂时停止，比如游戏开始，胜利，失败的音效
        /// </summary>
        public static AudioGroup ME => AudioManager.Ins.ME;

        /// <summary>
        /// Voice 音频组<para/>
        /// 人声，语音
        /// </summary>
        public static AudioGroup Voice => AudioManager.Ins.Voice;

        [NonSerialized] public static float ClipLengthMultiple;

        #endregion

        #region Volume

        /// <summary>
        /// 获取主音量
        /// </summary>
        /// <returns></returns>
        public static float GetVolume()
        {
            return GetVolume(AudioGroupType.Master);
        }

        /// <summary>
        /// 获取指定组的音量
        /// </summary>
        /// <param name="type">组类型</param>
        /// <returns>音量</returns>
        public static float GetVolume(AudioGroupType type)
        {
            return AudioManager.Ins.GetAudioGroup(type).Volume;
        }

        /// <summary>
        /// 设置主音量
        /// </summary>
        /// <param name="volume">音量</param>
        public static void SetVolume(float volume)
        {
            SetVolume(AudioGroupType.Master, volume);
        }

        /// <summary>
        /// 设置指定组的音量
        /// </summary>
        /// <param name="type">组类型</param>
        /// <param name="volume">音量</param>
        public static void SetVolume(AudioGroupType type, float volume)
        {
            AudioManager.Ins.GetAudioGroup(type).Volume = volume;
        }

        #endregion

        #region Play One Shot

        /// <summary>
        /// 在SE组播放一次音频片段
        /// </summary>
        /// <param name="clipType">音频片段类型</param>
        /// <param name="volumeScale">音量缩放</param>
        public static void PlayOneShot(object clipType, float volumeScale = 1f)
        {
            PlayOneShot(AudioGroupType.SE, null, GetAudioClip(clipType), volumeScale);
        }

        /// <summary>
        /// 播放一次音频片段
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clipType">音频片段类型</param>
        /// <param name="volumeScale">音量缩放</param>
        public static void PlayOneShot(AudioGroupType groupType, object clipType, float volumeScale = 1f)
        {
            PlayOneShot(groupType, null, GetAudioClip(clipType), volumeScale);
        }

        /// <summary>
        /// 在SE组播放一次音频片段
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="volumeScale">音量缩放</param>
        public static void PlayOneShot(AudioClip clip, float volumeScale = 1f)
        {
            PlayOneShot(AudioGroupType.SE, null, clip, volumeScale);
        }

        /// <summary>
        /// 播放一次音频片段
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clip">音频片段</param>
        /// <param name="volumeScale">音量缩放</param>
        public static void PlayOneShot(AudioGroupType groupType, AudioClip clip, float volumeScale = 1f)
        {
            PlayOneShot(groupType, null, clip, volumeScale);
        }

        /// <summary>
        /// 播放一次音频片段(判断重复)
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clip">音频片段</param>
        /// <param name="judgeRepeat">判断重复</param>
        /// <param name="volumeScale">音量缩放</param>
        public static void PlayOneShot(AudioGroupType groupType, AudioClip clip, bool judgeRepeat, float volumeScale = 1f)
        {
            if (judgeRepeat)
            {
                var agent = AudioManager.Ins.GetAudioGroup(groupType).DefaultAudioAgent;
                var audioAgent = agent as AudioAgent;
                if (audioAgent.PlayClips.Contains(clip))
                    return;
            }

            PlayOneShot(groupType, null, clip, volumeScale);
        }

        /// <summary>
        /// 在SE组播放一次音频片段
        /// </summary>
        /// <param name="path">音频资源路径</param>
        /// <param name="volumeScale">音量缩放</param>
        public static void PlayOneShot(string path, float volumeScale = 1f)
        {
            PlayOneShot(AudioGroupType.SE, null, GetAudioClip(path), volumeScale);
        }

        /// <summary>
        /// 播放一次音频片段
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="path">音频资源路径</param>
        /// <param name="volumeScale">音量缩放</param>
        public static void PlayOneShot(AudioGroupType groupType, string path, float volumeScale = 1f)
        {
            PlayOneShot(groupType, null, GetAudioClip(path), volumeScale);
        }

        /// <summary>
        /// 在指定分组和指定对象上播放一次音频片段
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="bindTarget">附加目标物体</param>
        /// <param name="clip">音频片段</param>
        /// <param name="volumeScale">音量缩放</param>
        public static void PlayOneShot(AudioGroupType groupType, Transform bindTarget, AudioClip clip, float volumeScale = 1f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(bindTarget).PlayOneShot(clip, volumeScale);
        }

        #endregion

        #region Play / Pasue / Resume / Stop

        /// <summary>
        /// 在BGM分组播放音频片段
        /// </summary>
        /// <param name="clipType">音频片段类型</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Play(object clipType, bool loop = false, float fadeInTime = 0f)
        {
            Play(AudioGroupType.BGM, null, GetAudioClip(clipType), loop, fadeInTime);
        }

        /// <summary>
        /// 在指定分组播放音频片段
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clipType">音频片段类型</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Play(AudioGroupType groupType, object clipType, bool loop = false, float fadeInTime = 0f)
        {
            Play(groupType, null, GetAudioClip(clipType), loop, fadeInTime);
        }

        /// <summary>
        /// 在BGM分组播放音频片段
        /// </summary>
        /// <param name="path">音频资源路径</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Play(string path, bool loop = false, float fadeInTime = 0f)
        {
            Play(AudioGroupType.BGM, null, GetAudioClip(path), loop, fadeInTime);
        }

        /// <summary>
        /// 在指定分组播放音频片段
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="path">音频资源路径</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Play(AudioGroupType groupType, string path, bool loop = false, float fadeInTime = 0f)
        {
            Play(groupType, null, GetAudioClip(path), loop, fadeInTime);
        }

        /// <summary>
        /// 在BGM分组播放音频片段
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Play(AudioClip clip, bool loop = false, float fadeInTime = 0f)
        {
            Play(AudioGroupType.BGM, null, clip, loop, fadeInTime);
        }

        /// <summary>
        /// 在指定分组播放音频片段
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clip">音频片段</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Play(AudioGroupType groupType, AudioClip clip, bool loop = false, float fadeInTime = 0f)
        {
            Play(groupType, null, clip, loop, fadeInTime);
        }

        /// <summary>
        /// 在指定分组和指定对象上播放音频
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="bindTarget">绑定对象</param>
        /// <param name="clip">音频</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Play(AudioGroupType groupType, Transform bindTarget, AudioClip clip, bool loop = false, float fadeInTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(bindTarget).Play(clip, loop, fadeInTime);
        }

        /// <summary>
        /// 暂停BGM组的音频
        /// </summary>
        /// <param name="fadeOutTime">淡出时间</param>
        public static void Pause(float fadeOutTime = 0f)
        {
            Pause(AudioGroupType.BGM, fadeOutTime);
        }

        /// <summary>
        /// 暂停指定分组的音频
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="fadeOutTime">淡出时间</param>
        public static void Pause(AudioGroupType groupType, float fadeOutTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).Pause(fadeOutTime);
        }

        /// <summary>
        /// 恢复BGM组的音频
        /// </summary>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Resume(float fadeInTime = 0f)
        {
            Resume(AudioGroupType.BGM, fadeInTime);
        }

        /// <summary>
        /// 恢复指定分组的音频
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="fadeInTime">淡入时间</param>
        public static void Resume(AudioGroupType groupType, float fadeInTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).Resume(fadeInTime);
        }

        /// <summary>
        /// 停止BGM组的音频
        /// </summary>
        /// <param name="fadeOutTime">淡出时间</param>
        public static void Stop(float fadeOutTime = 0f)
        {
            Stop(AudioGroupType.BGM, fadeOutTime);
        }

        /// <summary>
        /// 停止指定分组的依附于指定物体的音频
        /// </summary>
        /// <param name="fadeOutTime">淡出时间</param>
        public static void Stop(AudioGroupType groupType, Transform bindTarget, float fadeOutTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(bindTarget).Stop(fadeOutTime);
        }

        /// <summary>
        /// 停止指定分组的音频
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="fadeOutTime">淡出时间</param>
        public static void Stop(AudioGroupType groupType, float fadeOutTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).Stop(fadeOutTime);
        }

        #endregion

        #region AudioGroup / AudioAgent

        /// <summary>
        /// 获取指定类型的音频组
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <returns>结果</returns>
        public static AudioGroup GetAudioGroup(AudioGroupType groupType)
        {
            return AudioManager.Ins.GetAudioGroup(groupType);
        }

        /// <summary>
        /// 获取指定类型和绑定对象的音频代理
        /// </summary>
        /// <param name="groupType">音频组类型</param>
        /// <param name="target">绑定对象(可空，不指定对象则为音频组内默认音频代理)</param>
        /// <returns>结果</returns>
        public static AudioAgent GetAudioAgent(AudioGroupType groupType, Transform target = null)
        {
            return AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(target);
        }

        #endregion

        #region Get AudioClip Resources

        /// <summary>
        /// 获取音频资源
        /// </summary>
        /// <param name="clipType">片段类型</param>
        /// <returns>结果</returns>
        public static AudioClip GetAudioClip(object clipType)
        {
            var clip = GetAudioClip(clipType.ToString());
            return clip;
        }

        /// <summary>
        /// 获取音频资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>结果</returns>
        public static AudioClip GetAudioClip(string path)
        {
            var clip = Resources.Load<AudioClip>(path);
            return clip;
        }

        #endregion
    }
}