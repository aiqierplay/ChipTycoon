/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AudioVolumeSlider.cs
//  Info     : 音量控制滑条组件
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;

namespace Aya.Media.Audio
{
    [AddComponentMenu("Audio/UI Volume Slider")]
    [RequireComponent(typeof(Slider))]
    [DisallowMultipleComponent]
    public class UIVolumeSlider : MonoBehaviour
    {
        public AudioGroupType GroupType;

        private Slider _slider;
        private float _min;
        private float _max;

        public void Awake()
        {
            _slider = GetComponent<Slider>();
            _min = _slider.minValue;
            _max = _slider.maxValue;
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        public void OnEnable()
        {
            _slider.value = Mathf.Lerp(_min, _max, Audio.GetVolume(GroupType));
        }

        public void OnValueChanged(float value)
        {
            var volume = (_slider.value - _min) / (_max - _min);
            Audio.SetVolume(GroupType, volume);
        }
    }
}
