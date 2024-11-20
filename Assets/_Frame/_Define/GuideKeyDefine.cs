using System.Collections;
using Aya.Extension;
using Sirenix.OdinInspector;

public static class GuideKeyDefine
{
    public static int LaunchGame = 0;

    #region Editor
  
    public static IEnumerable GetDefineKeys()
    {
        var dropdown = new ValueDropdownList<string>();
        foreach (var fieldInfo in typeof(GuideKeyDefine).GetFields())
        {
            var key = fieldInfo.Name;
            var value = fieldInfo.GetValue(null).CastType<string>();
            dropdown.Add(key, value);
        }

        return dropdown;
    } 

    #endregion
}