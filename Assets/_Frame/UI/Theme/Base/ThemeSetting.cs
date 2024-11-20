using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public abstract class
    ThemeSetting<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter> : SubSettingRootBase<TThemeSetting, TTheme>
    where TThemeSetting : ThemeSetting<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter>
    where TTheme : Theme<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter>
    where TThemeAdapter : ThemeAdapter<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter>
    where TThemeStyleData : ThemeStyleData
{
    [ValueDropdown(nameof(PaletteKeyGetter))] [BoxGroup("Theme")]
    public string CurrentThemeKey;

    public IEnumerable<string> PaletteKeyGetter()
    {
        return DataList.Select(subSetting => subSetting.Key);
    }

#if UNITY_EDITOR
    public virtual void OnValidate()
    {
        if (Application.isPlaying) return;
        PreviewApply();
    }

    [Button(SdfIconType.ArrowRepeat)]
    [BoxGroup("Theme")]
    [GUIColor(1f, 1f, 0.5f)]
    public virtual void PreviewApply()
    {
        RefreshLoadEditor();
        var adapterList = FindObjectsByType<TThemeAdapter>(FindObjectsSortMode.None);
        foreach (var adapter in adapterList)
        {
            adapter.Apply();
        }
    }
#endif
}