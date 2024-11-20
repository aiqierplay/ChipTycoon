using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ProgressTriggerData
{
    [TableColumnWidth(55, false), Range(0f, 1f)] public float Progress;
    [TableColumnWidth(90, false)] public ConditionCompareType Compare;
    public TriggerCallback Callback;

    [NonSerialized] public bool Active;

    public virtual void Init()
    {
        Active = true;
    }

    public virtual void Invoke()
    {
        if (!Active) return;
        Active = false;
        Callback.Invoke();
    }
}

[Serializable]
public class ProgressTrigger: CustomTrigger
{
    [TableList] public List<ProgressTriggerData> DataList;

    [NonSerialized] public Func<float> ProgressGetter;

    public override bool CanTrigger => false;

    public ProgressTrigger(Func<float> progressGetter)
    {
        Init(progressGetter);
    }

    public void Init(Func<float> progressGetter)
    {
        ProgressGetter = progressGetter;
    }

    public override void Update(float deltaTime)
    {
        if (IsRunning)
        {
            var progress = ProgressGetter();
            foreach (var triggerData in DataList)
            {
                if (!triggerData.Active) continue;
                if (ConditionCompareUtil.Compare(progress, triggerData.Progress, triggerData.Compare))
                {
                    triggerData.Invoke();
                }
            }
        }
    }
}
