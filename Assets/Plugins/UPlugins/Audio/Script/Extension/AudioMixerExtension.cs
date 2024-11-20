/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AudioMixerExtension.cs
//  Info     : AudioMixer 扩展方法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Audio;

namespace Aya.Media.Audio
{
    public static class AudioMixerExtension
    {
        /// <summary>
        /// AudioMixer 最小音量值
        /// </summary>
        internal static float AudioMixerMinVolume = -80f;

        /// <summary>
        /// AudioMixer 最大音量值
        /// </summary>
        internal static float AudioMixerMaxVolume = 0f;

        /// <summary>
        /// 设指指定组的音量
        /// </summary>
        /// <param name="mixer">混音器</param>
        /// <param name="groupName">组名</param>
        /// <param name="linearVolume">音量（0 - 1）</param>
        /// <returns>mixer</returns>
        public static AudioMixer SetVolume(this AudioMixer mixer, string groupName, float linearVolume)
        {
            var dbVolume = 20f * Mathf.Log10(linearVolume);
            if (linearVolume == 0f) dbVolume = AudioMixerMinVolume;
            mixer.SetFloat(groupName + "Volume", dbVolume);
            return mixer;
        }

        /// <summary>
        /// 获取指定组的音量
        /// </summary>
        /// <param name="mixer">混音器</param>
        /// <param name="groupName">组名</param>
        /// <returns>音量（0 - 1）</returns>
        public static float GetVolume(this AudioMixer mixer, string groupName)
        {
            mixer.GetFloat(groupName + "Volume", out var dbVolume);
            var linearVolume = Mathf.Pow(10f, dbVolume / 20f);
            return linearVolume;
        }
    }
}