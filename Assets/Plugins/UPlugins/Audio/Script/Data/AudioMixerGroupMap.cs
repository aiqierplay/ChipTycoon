/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AudioMixerGroupMap.cs
//  Info     : 音频组类型 - 混音器分组 映射
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine.Audio;

namespace Aya.Media.Audio
{
    [Serializable]
    public class AudioMixerGroupMap
    {
        public AudioGroupType Type;
        public AudioMixerGroup Group;
    }
}
