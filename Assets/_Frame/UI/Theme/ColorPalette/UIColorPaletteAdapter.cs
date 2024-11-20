using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MaskableGraphic))]
public class
    UIColorPaletteAdapter : ThemeAdapter<ColorPaletteSetting, ColorPalette, ColorPaletteData, UIColorPaletteAdapter>
{
    [NonSerialized] public MaskableGraphic Target;

    public override ColorPaletteSetting GetSettingInstance() => ColorPaletteSetting.Ins;

    public override void Apply()
    {
        if (Target == null) Target = GetComponent<MaskableGraphic>();
        if (Target == null) return;
        var styleData = GetThemeStyleData();
        if (styleData == null) return;
        Target.color = styleData.Color;
    }
}