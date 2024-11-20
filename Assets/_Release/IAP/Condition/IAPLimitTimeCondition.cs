using System;
using Aya.Data.Persistent;
using UnityEngine;

[Serializable]
public class IAPLimitTimeCondition : IAPCondition
{
    public string Key;
    public int LevelIndex;
    public int ValidTime;

    [NonSerialized] public sDateTime FirstUnlockTime;

    public override bool Check()
    {
        if (SaveManager.Ins == null)
        {
            return false;
        }
        if (SaveManager.Ins.LevelIndex < LevelIndex) return false;
        FirstUnlockTime = new sDateTime("IAP_Condition_" + Key, DateTime.Now);
        if (FirstUnlockTime.Value < 0)
        {
             var time = new System.DateTime(1970, 1, 1, 0, 0, 0);
             FirstUnlockTime.Value = time.Millisecond;
        }
        var timeSpan = DateTime.Now - FirstUnlockTime;
        var seconds = timeSpan.TotalSeconds;
        return seconds <= ValidTime;
    }
}
