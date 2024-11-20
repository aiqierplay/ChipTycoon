using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;
#if GA
using GameAnalyticsSDK;
#endif

[CreateAssetMenu(fileName = "ABTestSetting", menuName = "Setting/AB Test Setting")]
public class ABTestSetting : SettingBase<ABTestSetting>
{
    #region AB Test Config

    public string Config => GetValue<string>("Config");
    public string Level => GetValue<string>("Level");
    public string UI => GetValue<string>("UI");

    #endregion

    [TableList] public List<ABTestGroup> GroupList;

    public Dictionary<string, ABTestGroup> GroupDic { get; set; }

    [Button("Clear All Test"), GUIColor(1f, 0.75f, 0.75f)]
    public void ClearAllTest()
    {
        GroupList.ForEach(g => g.TestKey = null);
    }

    public override void Init()
    {
        base.Init();
        GroupDic = GroupList.ToDictionary(g => g.Group);

        foreach (var abTestGroup in GroupList)
        {
            abTestGroup.Init();
        }
    }

    public ABTestGroup GetGroup(string groupKey)
    {
        return GroupDic.TryGetValue(groupKey, out var group) ? group : null;
    }

    public string GetValue(string groupKey, string configKey = null, string defaultValue = null)
    {
        var group = GetGroup(groupKey);
        if (group == null) return defaultValue;
        return group.GetValue(configKey, defaultValue);
    }

    public T GetValue<T>(string groupKey, string configKey = null, T defaultValue = default)
    {
        var group = GetGroup(groupKey);
        if (group == null) return defaultValue;
        return group.GetValue<T>(configKey);
    }
}
