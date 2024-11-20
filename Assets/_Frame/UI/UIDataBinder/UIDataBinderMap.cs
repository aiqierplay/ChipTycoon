using System;
using Aya.Data;
using Aya.DataBinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIDataBinderMap
{
    public static DataConverter DefaultConverter = new CommonConverter();

    public static MultiDictionary<Type, Type, string> AutoCacheTypeDic = new MultiDictionary<Type, Type, string>()
    {
        { typeof(string), typeof(Text), nameof(Text.text) },
        { typeof(string), typeof(TextMeshProUGUI), nameof(TextMeshProUGUI.text) },
        { typeof(string), typeof(TextMeshPro), nameof(TextMeshPro.text) },
        { typeof(Sprite), typeof(Image), nameof(Image.sprite) },
        { typeof(int), typeof(Text), nameof(Text.text) },
        { typeof(int), typeof(TextMeshProUGUI), nameof(TextMeshProUGUI.text) },
        { typeof(int), typeof(TextMeshPro), nameof(TextMeshPro.text) },
        { typeof(float), typeof(Text), nameof(Text.text) },
        { typeof(float), typeof(TextMeshProUGUI), nameof(TextMeshProUGUI.text) },
        { typeof(float), typeof(TextMeshPro), nameof(TextMeshPro.text) },
        { typeof(float), typeof(Slider), nameof(Slider.value) },
        { typeof(float), typeof(Image), nameof(Image.fillAmount) },
        { typeof(long), typeof(Text), nameof(Text.text) },
        { typeof(long), typeof(TextMeshProUGUI), nameof(TextMeshProUGUI.text) },
        { typeof(long), typeof(TextMeshPro), nameof(TextMeshPro.text) },
        { typeof(double), typeof(Text), nameof(Text.text) },
        { typeof(double), typeof(TextMeshProUGUI), nameof(TextMeshProUGUI.text) },
        { typeof(double), typeof(TextMeshPro), nameof(TextMeshPro.text) },
        { typeof(decimal), typeof(Text), nameof(Text.text) },
        { typeof(decimal), typeof(TextMeshProUGUI), nameof(TextMeshProUGUI.text) },
        { typeof(decimal), typeof(TextMeshPro), nameof(TextMeshPro.text) },
        { typeof(bool), typeof(Toggle), nameof(Toggle.isOn) }
    };
}