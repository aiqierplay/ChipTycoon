using System;
using System.Collections;
using Sirenix.OdinInspector;

[Serializable]
public class GuideComplete : GuideCondition
{
    [ValueDropdown(nameof(GetDefineKeys))] public string Key;
    public static IEnumerable GetDefineKeys() => GuideKeyDefine.GetDefineKeys();


    public override bool Check()
    {
        return Guide.IsGuideComplete(Key);
    }
}
