using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

    public abstract class ThemeAdapter : MonoBehaviour
    {
        public virtual void OnEnable()
        {
            if (!Application.isPlaying) return;
            ThemeAdapterManager.Ins.Register(this);
        }

        public virtual void OnDisable()
        {
            if (!Application.isPlaying) return;
            ThemeAdapterManager.Ins.DeRegister(this);
        }

        public abstract void Apply();
    }

public abstract class ThemeAdapter<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter> : ThemeAdapter
    where TThemeSetting : ThemeSetting<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter>
    where TTheme : Theme<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter>
    where TThemeAdapter : ThemeAdapter<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter>
    where TThemeStyleData : ThemeStyleData
{
    [ValueDropdown(nameof(StyleKeyGetter))]
    public string StyleKey;

    public string ThemeKey => GetSettingInstance().CurrentThemeKey;

    public IEnumerable<string> StyleKeyGetter()
    {
        if (string.IsNullOrEmpty(ThemeKey)) return null;
        if (!GetSettingInstance().DataDic.TryGetValue(ThemeKey, out var theme)) return null;
        return theme.StyleList.Select(data => data.Key);
    }

    public abstract TThemeSetting GetSettingInstance();

    public TTheme GetTheme()
    {
        if (string.IsNullOrEmpty(ThemeKey)) return null;
        if (!GetSettingInstance().DataDic.TryGetValue(ThemeKey, out var theme)) return null;
        return theme;
    }

    public TThemeStyleData GetThemeStyleData()
    {
        if (string.IsNullOrEmpty(ThemeKey)) return null;
        if (!GetSettingInstance().DataDic.TryGetValue(ThemeKey, out var theme)) return null;
        if (string.IsNullOrEmpty(StyleKey)) return null;
        if (!theme.StyleDic.TryGetValue(StyleKey, out var themeStyleData)) return null;
        return themeStyleData;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Apply();
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }

#if UNITY_EDITOR
    public virtual void OnValidate()
    {
        if (Application.isPlaying) return;
        Apply();
        EditorUtility.SetDirty(this);
    }
#endif
}