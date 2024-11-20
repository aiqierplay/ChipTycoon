/////////////////////////////////////////////////////////////////////////////
//
//  Script   : TransformExtension.cs
//  Info     : Transform 扩展，用于快速调用绑定于 Transform 的音频代理
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Aya.Media.Audio
{
    public static class TransformExtension
    {
        #region Volume

        /// <summary>
        /// 获取指定组当前对象绑定音频代理的音量
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="type">组类型</param>
        /// <returns>音量</returns>
        public static float GetVolume(this Transform transform, AudioGroupType type)
        {
            return AudioManager.Ins.GetAudioGroup(type).GetAudioAgent(transform).Volume;
        }

        /// <summary>
        /// 设置指定组当前对象绑定音频代理的音量
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="type">组类型</param>
        /// <param name="volume">音量</param>
        /// <returns>transform</returns>
        public static Transform SetVolume(this Transform transform, AudioGroupType type, float volume)
        {
            AudioManager.Ins.GetAudioGroup(type).GetAudioAgent(transform).Volume = volume;
            return transform;
        }

        #endregion

        #region Play One Shot

        /// <summary>
        /// 在SE组当前对象绑定音频代理播放一次音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="clipType">音频片段类型</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <returns>transform</returns>
        public static Transform PlayOneShot(this Transform transform, object clipType, float volumeScale = 1f)
        {
            PlayOneShot(transform, AudioGroupType.SE, Audio.GetAudioClip(clipType), volumeScale);
            return transform;
        }

        /// <summary>
        /// 在当前对象绑定音频代理播放一次音频片段
        /// </summary>
        /// <param name="transform">附加对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clipType">音频片段类型</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <returns>transform</returns>
        public static Transform PlayOneShot(this Transform transform, AudioGroupType groupType, object clipType, float volumeScale = 1f)
        {
            PlayOneShot(transform, groupType, Audio.GetAudioClip(clipType), volumeScale);
            return transform;
        }

        /// <summary>
        /// 在SE组当前对象绑定音频代理播放一次音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="clip">音频片段</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <returns>transform</returns>
        public static Transform PlayOneShot(this Transform transform, AudioClip clip, float volumeScale = 1f)
        {
            PlayOneShot(transform, AudioGroupType.SE, clip, volumeScale);
            return transform;
        }

        /// <summary>
        /// 在SE组当前对象绑定音频代理播放一次音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="path">音频资源路径</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <returns>transform</returns>
        public static Transform PlayOneShot(this Transform transform, string path, float volumeScale = 1f)
        {
            PlayOneShot(transform, AudioGroupType.SE, Audio.GetAudioClip(path), volumeScale);
            return transform;
        }

        /// <summary>
        /// 在当前对象绑定音频代理播放一次音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="path">音频资源路径</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <returns>transform</returns>
        public static Transform PlayOneShot(this Transform transform, AudioGroupType groupType, string path, float volumeScale = 1f)
        {
            PlayOneShot(transform, groupType, Audio.GetAudioClip(path), volumeScale);
            return transform;
        }

        /// <summary>
        /// 在指定分组当前对象绑定音频代理播放一次音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clip">音频片段</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <returns>transform</returns>
        public static Transform PlayOneShot(this Transform transform, AudioGroupType groupType, AudioClip clip, float volumeScale = 1f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(transform).PlayOneShot(clip, volumeScale);
            return transform;
        }

        #endregion

        #region Play / Pasue / Resume / Stop

        /// <summary>
        /// 在BGM分组当前对象绑定音频代理播放音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="clipType">音频片段类型</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        /// <returns>transform</returns>
        public static Transform Play(this Transform transform, object clipType, bool loop = false, float fadeInTime = 0f)
        {
            Play(transform, AudioGroupType.BGM, Audio.GetAudioClip(clipType), loop, fadeInTime);
            return transform;
        }

        /// <summary>
        /// 在制定分组当前对象绑定音频代理播放音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clipType">音频片段类型</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        /// <returns>transform</returns>
        public static Transform Play(this Transform transform, AudioGroupType groupType, object clipType, bool loop = false, float fadeInTime = 0f)
        {
            Play(transform, groupType, Audio.GetAudioClip(clipType), loop, fadeInTime);
            return transform;
        }

        /// <summary>
        /// 在BGM分组当前对象绑定音频代理播放音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="path">音频资源路径</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        /// <returns>transform</returns>
        public static Transform Play(this Transform transform, string path, bool loop = false, float fadeInTime = 0f)
        {
            Play(transform, AudioGroupType.BGM, Audio.GetAudioClip(path), loop, fadeInTime);
            return transform;
        }

        /// <summary>
        /// 在指定分组当前对象绑定音频代理播放音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="path">音频资源路径</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        /// <returns>transform</returns>
        public static Transform Play(this Transform transform, AudioGroupType groupType, string path, bool loop = false, float fadeInTime = 0f)
        {
            Play(transform, groupType, Audio.GetAudioClip(path), loop, fadeInTime);
            return transform;
        }

        /// <summary>
        /// 在BGM分组当前对象绑定音频代理播放音频片段
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="clip">音频片段</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        /// <returns>transform</returns>
        public static Transform Play(this Transform transform, AudioClip clip, bool loop = false, float fadeInTime = 0f)
        {
            Play(transform, AudioGroupType.BGM, clip, loop, fadeInTime);
            return transform;
        }

        /// <summary>
        /// 在指定分组和指定对象上播放音频
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="clip">音频</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        /// <returns>transform</returns>
        public static Transform Play(this Transform transform, AudioGroupType groupType, AudioClip clip, bool loop = false, float fadeInTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(transform).Play(clip, loop, fadeInTime);
            return transform;
        }

        /// <summary>
        /// 暂停BGM组当前对象绑定音频代理的音频
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="fadeOutTime">淡出时间</param>
        /// <returns>transform</returns>
        public static Transform Pause(this Transform transform, float fadeOutTime = 0f)
        {
            Pause(transform, AudioGroupType.BGM, fadeOutTime);
            return transform;
        }

        /// <summary>
        /// 暂停指定分组当前对象绑定音频代理的音频
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="fadeOutTime">淡出时间</param>
        /// <returns>transform</returns>
        public static Transform Pause(this Transform transform, AudioGroupType groupType, float fadeOutTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(transform).Pause(fadeOutTime);
            return transform;
        }

        /// <summary>
        /// 恢复BGM组当前对象绑定音频代理的音频
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="fadeInTime">淡入时间</param>
        /// <returns>transform</returns>
        public static Transform Resume(this Transform transform, float fadeInTime = 0f)
        {
            Resume(transform, AudioGroupType.BGM, fadeInTime);
            return transform;
        }

        /// <summary>
        /// 恢复指定分组当前对象绑定音频代理的音频
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="fadeInTime">淡入时间</param>
        /// <returns>transform</returns>
        public static Transform Resume(this Transform transform, AudioGroupType groupType, float fadeInTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(transform).Resume(fadeInTime);
            return transform;
        }

        /// <summary>
        /// 停止BGM组当前对象绑定音频代理的音频
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="fadeOutTime">淡出时间</param>
        /// <returns>transform</returns>
        public static Transform Stop(this Transform transform, float fadeOutTime = 0f)
        {
            Stop(transform, AudioGroupType.BGM, fadeOutTime);
            return transform;
        }

        /// <summary>
        /// 停止指定分组当前对象绑定音频代理的音频
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="fadeOutTime">淡出时间</param>
        /// <returns>transform</returns>
        public static Transform Stop(this Transform transform, AudioGroupType groupType, float fadeOutTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(transform).Stop(fadeOutTime);
            return transform;
        }

        #endregion

        #region AudioGroup / AudioAgent

        /// <summary>
        /// 获取指定类型的音频组
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <returns>结果</returns>
        public static AudioGroup GetAudioGroup(this Transform transform, AudioGroupType groupType)
        {
            return AudioManager.Ins.GetAudioGroup(groupType);
        }

        /// <summary>
        /// 获取指定类型和绑定对象的音频代理
        /// </summary>
        /// <param name="transform">绑定对象</param>
        /// <param name="groupType">音频组类型</param>
        /// <returns>结果</returns>
        public static AudioAgent GetAudioAgent(this Transform transform, AudioGroupType groupType)
        {
            return AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(transform);
        }

        #endregion
    }
}