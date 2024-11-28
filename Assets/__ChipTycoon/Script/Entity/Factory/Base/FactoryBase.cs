using Sirenix.OdinInspector;
using System;
using System.Collections;
using Aya.Async;
using TMPro;
using UnityEngine;

public abstract class FactoryBase : BuildingBase
{
    [BoxGroup("Input")] public FactoryPoint Input = new FactoryPoint();

    [BoxGroup("Output")] public FactoryPoint Output = new FactoryPoint();

    [BoxGroup("State")] public GameObject LockObj;
    [BoxGroup("State")] public GameObject CanUnlockTip;
    [BoxGroup("State")] public GameObject UnlockObj;
    [BoxGroup("State")] public TMP_Text TextUnlockCost;

    public float WorkDuration = 1f;
    public float WorkInterval = 0.5f;
    
    [NonSerialized] public int Index; 
    [NonSerialized] public FactoryInfo Info;
    [NonSerialized] public bool IsWorking = false;
    [NonSerialized] public float WorkProgress;

    public virtual void Init(int index)
    {
        Index = index;
        Info = FactoryInfo.GetInfo(index, true);
        Input.Init();
        Output.Init();
        LoadState();
        Refresh();

        StartCoroutine(WorkCo());
    }

    public virtual void LoadState()
    {
        Input.Add(Info.InputCount);
        Output.Add(Info.OutputCount);
    }

    public virtual void SaveState()
    {
        Info.InputCount = Input.Count;
        Info.OutputCount = Output.Count;
        Info.Save();
    }

    public override void Refresh()
    {
        base.Refresh();
        Input.Refresh();
        Output.Refresh();
    }

    public IEnumerator WorkCo()
    {
        IsWorking = false;
        WorkProgress = 0f;
        while (true)
        {
            var timer = 0f;
            if (Input.Count >= Input.Number && Output.CanAdd)
            {
                IsWorking = true;
                while (timer <= WorkDuration)
                {
                    timer += DeltaTime;
                    if (timer >= WorkDuration)
                    {
                        timer = WorkDuration;
                    }

                    WorkProgress = timer / WorkDuration;

                    if (WorkProgress >= 1)
                    {
                        for (var i = 0; i < Input.Number; i++)
                        {
                            Input.Remove(Input.Number);
                        }

                        Output.Add(Output.Number);
                        IsWorking = false;

                        Refresh();
                        break;
                    }

                    Refresh();
                    yield return null;
                }

                yield return YieldBuilder.WaitForSeconds(WorkInterval);
            }

            yield return null;
        }
    }
}
