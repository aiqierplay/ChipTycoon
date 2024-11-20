using TMPro;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(TMP_Text))]
public class UITextStyleAdapter : ThemeAdapter<TextStyleSetting, TextStyle, TextStyleData, UITextStyleAdapter>
{
    [NonSerialized] public TMP_Text Target;

    public override TextStyleSetting GetSettingInstance() => TextStyleSetting.Ins;

    public override void Apply()
    {
        if (Target == null) Target = GetComponent<TMP_Text>();
        if (Target == null) return;
        var styleData = GetThemeStyleData();
        if (styleData == null) return;
        Target.font = styleData.Font;
        Target.fontSize = styleData.FontSize;
        Target.fontStyle = styleData.FontStyles;
    }
}