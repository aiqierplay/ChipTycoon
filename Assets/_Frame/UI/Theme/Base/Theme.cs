using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

[Serializable]
public abstract class Theme<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter> : SubSettingBase<TThemeSetting, TTheme>
    where TThemeSetting : SubSettingRootBase<TThemeSetting, TTheme>
    where TTheme : Theme<TThemeSetting, TTheme, TThemeStyleData, TThemeAdapter>
    where TThemeStyleData : ThemeStyleData
{
    [BoxGroup("Theme")] public List<TThemeStyleData> StyleList;

    public Dictionary<string, TThemeStyleData> StyleDic
    {
        get
        {
            if (StyleDicCache != null) return StyleDicCache;
            StyleDicCache = StyleList.ToDictionary(data => data.Key);
            return StyleDicCache;
        }
    }

    protected Dictionary<string, TThemeStyleData> StyleDicCache;
}