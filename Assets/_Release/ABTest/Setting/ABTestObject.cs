using System.Collections.Generic;
using UnityEngine;

public class ABTestObject : EntityBase
{
    public string Key;
    public List<GameObject> TargetList;

    public void Start()
    {
        var active = ABTestSetting.Ins.GetValue<bool>(Key);
        if (TargetList == null) return;
        foreach (var target in TargetList)
        {
            if (target == null) continue;
            target.SetActive(active);
        }
    }
}
