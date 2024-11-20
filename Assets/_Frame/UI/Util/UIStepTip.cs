using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class UIStepNode
{
    public GameObject Step;
    public GameObject Select;
}

public class UIStepTip : UIBase<UIStepTip>
{
    [TableList] public List<UIStepNode> StepList;

    [NonSerialized] public int StepCount;
    [NonSerialized] public int SelectIndex;

    public void Init(int stepCount = -1)
    {
        StepCount = stepCount < 0 ? StepList.Count : stepCount;
    }

    public virtual void Select(int index)
    {
        SelectIndex = Mathf.Clamp(index, 0, StepList.Count - 1);
        Refresh();
    }

    public override void Refresh(bool immediately = false)
    {
        for (var i = 0; i < StepList.Count; i++)
        {
            var step = StepList[i];
            step.Step.SetActive(i < StepCount);
            step.Select.SetActive(i == SelectIndex);
        }
    }
}
