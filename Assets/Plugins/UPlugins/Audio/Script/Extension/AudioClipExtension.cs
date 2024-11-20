/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AudioClipExtension.cs
//  Info     : AudioClip 扩展方法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Aya.Media.Audio
{
    public static class AudioClipExtension
    {
        #region Play One Shot

        /// <summary>
        /// 在SE分组默认音频代理播放一次音频片段
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <returns>clip</returns>
        public static AudioClip PlayOneShot(this AudioClip clip, float volumeScale = 1f)
        {
            PlayOneShot(clip, AudioGroupType.SE, null, volumeScale);
            return clip;
        }

        /// <summary>
        /// 在指定分组默认音频代理播放一次音频片段
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="volumeScale">音量缩放</param>
        ///  <returns>clip</returns>
        public static AudioClip PlayOneShot(this AudioClip clip, AudioGroupType groupType, float volumeScale = 1f)
        {
            PlayOneShot(clip, groupType, null, volumeScale);
            return clip;
        }

        /// <summary>
        /// 播放一次音频片段
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="bindTarget">绑定对象</param>
        /// <param name="volumeScale">音量缩放</param>
        ///  <returns>clip</returns>
        public static AudioClip PlayOneShot(this AudioClip clip, AudioGroupType groupType, Transform bindTarget, float volumeScale = 1f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(bindTarget).PlayOneShot(clip, volumeScale);
            return clip;
        }

        #endregion

        #region Play

        /// <summary>
        /// 在BGM组默认音频代理播放音频片段
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        ///  <returns>clip</returns>
        public static AudioClip Play(this AudioClip clip, bool loop = false, float fadeInTime = 0f)
        {
            Play(clip, AudioGroupType.BGM, null, loop, fadeInTime);
            return clip;
        }

        /// <summary>
        /// 在指定分组默认音频代理播放音频片段
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        ///  <returns>clip</returns>
        public static AudioClip Play(this AudioClip clip, AudioGroupType groupType, bool loop = false, float fadeInTime = 0f)
        {
            Play(clip, groupType, null, loop, fadeInTime);
            return clip;
        }

        /// <summary>
        /// 播放音频片段
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="groupType">音频组类型</param>
        /// <param name="bindTarget">绑定对象</param>
        /// <param name="loop">循环</param>
        /// <param name="fadeInTime">淡入时间</param>
        ///  <returns>clip</returns>
        public static AudioClip Play(this AudioClip clip, AudioGroupType groupType, Transform bindTarget, bool loop = false, float fadeInTime = 0f)
        {
            AudioManager.Ins.GetAudioGroup(groupType).GetAudioAgent(bindTarget).Play(clip, loop, fadeInTime);
            return clip;
        }

        #endregion
    }
}