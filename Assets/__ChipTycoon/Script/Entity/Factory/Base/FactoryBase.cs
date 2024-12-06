using Sirenix.OdinInspector;
using System;
using System.Collections;
using Aya.Async;
using Aya.Events;
using Aya.TweenPro;
using Aya.Util;
using TMPro;
using UnityEngine;

public abstract class FactoryBase : BuildingBase
{
    [GUIColor(0.5f, 1f, 1f)]
    public string DataKey;

    [BoxGroup("Input")] public FactoryPoint Input = new FactoryPoint();
    [BoxGroup("Output")] public FactoryPoint Output = new FactoryPoint();

    [BoxGroup("State")] public GameObject LockObj;
    [BoxGroup("State")] public GameObject UnlockObj;
    // [BoxGroup("State")] public TMP_Text TextUnlockSpent;
    [BoxGroup("State")] public TMP_Text TextUnlockCost;
    [BoxGroup("State")] public int UnlockSpeed = 1;

    // [BoxGroup("Effect")] public UTweenPlayerReference TweenWork;
    [BoxGroup("Effect")] public GameObject FxUnlock;

    [NonSerialized] public int Index;
    [NonSerialized] public FactoryData Data;
    [NonSerialized] public FactoryInfo Info;
    [NonSerialized] public float WorkProgress;
    [NonSerialized] public float WorkDuration = 1f;
    // [NonSerialized] public float WorkInterval = 0.5f;

    public bool IsWorking => WorkerList.Count > 0;

    [GetComponentInChildren, NonSerialized]
    public ProduceLine ProduceLine;

    public virtual void Init(int index)
    {
        Index = index;
        Data = Config.GetData<FactoryData>(index, CurrentLevel.SaveKey);
        Info = FactoryInfo.GetInfo(index, true);
        if (!Info.Unlock && Data.DefaultUnlock)
        {
            Info.Unlock = true;
        }

        Input.Init(this);
        Output.Init(this);
        LoadState();
        RefreshData();
        Refresh();

        ProduceLine.InputNum = Input.Number;
        ProduceLine.OutputNum = Output.Number;
        ProduceLine.WorkDuration = WorkDuration;
        ProduceLine.OnOutput = output =>
        {
            var product = output as Product;
            Output.StackList.AddParabola(product);
            Refresh();
        };

        StartCoroutine(WorkCo());
    }

    [Listen(GameEvent.Upgrade)]
    public void RefreshData()
    {
        WorkDuration = Upgrade.GetInfo<FactoryWorkDurationData>(CurrentLevel.SaveKey + "/" + DataKey).Current.Value;
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

        if (LockObj != null) LockObj.SetActive(!Info.Unlock);
        if (UnlockObj != null) UnlockObj.SetActive(Info.Unlock);
        // if (TextUnlockSpent != null) TextUnlockSpent.text = Info.UnlockSpent.ToString();
        if (TextUnlockCost != null) TextUnlockCost.text = (Data.UnlockCost - Info.UnlockSpent).ToString();
    }

    public virtual void OnClick()
    {
        if (UI.Current is not UIGame) return;
        UI.Show<UIFactory>(this);
        World.Character.DisableMove();
    }
    
    [Button]
    public void TestAddInput()
    {
        Input.Add(10);
    }
    
    [Button]
    public void TestAddOutput()
    {
        Output.Add(10);
    }

    [NonSerialized] public Coroutine UnlockCoroutine;

    public override void OnEnterImpl(Worker worker)
    {
        if (!Info.Unlock)
        {
            UnlockCoroutine = StartCoroutine(UnlockCo());
        }
    }

    public override void OnExitImpl(Worker worker)
    {
        if (UnlockCoroutine != null)
        {
            StopCoroutine(UnlockCoroutine);
            UnlockCoroutine = null;
        }
    }

    public IEnumerator UnlockCo()
    {
        while (!Info.Unlock)
        {
            var spend = UnlockSpeed;
            var spendMax = Data.UnlockCost - Info.UnlockSpent;
            if (spendMax < spend)
            {
                spend = spendMax;
            }

            if (Save.Coin.Value >= spend)
            {
                Info.UnlockSpent += spend;
                Save.Coin.Value -= spend;
                Refresh();
                yield return null;
            }
            
            if (Info.UnlockSpent >= Data.UnlockCost)
            {
                Unlock();
                break;
            }

            yield return null;
        }
    }

    public void Unlock()
    {
        WorkerList.Clear();
        Info.Unlock = true;
        SpawnFx(FxUnlock);
        Refresh();
    }

    public IEnumerator WorkCo()
    {
        WorkProgress = 0f;
        while (!Info.Unlock)
        {
            yield return YieldBuilder.WaitForSeconds(0.1f);
        }

        ProduceLine.Init();

        var inputTimer = 0f;
        while (true)
        {
            while (IsWorking)
            {
                yield return null;
            }

            while (inputTimer <= ProduceLine.InputInterval)
            {
                inputTimer += DeltaTime;
                yield return null;
            }

            while (IsWorking)
            {
                yield return null;
            }

            yield return YieldBuilder.WaitForSeconds(0.1f);

            if (Input.Count > 0 && ProduceLine.CheckCanInput())
            {
                var product = Input.StackList.Pop();
                yield return product.WaitForParabolaFlyTo(ProduceLine.InputStart.position, 2f,
                    RandUtil.RandFloat(0.25f, 0.35f),
                    () =>
                    {
                        GamePool.DeSpawn(product);
                    });

                ProduceLine.AddInput();
                Refresh();

                inputTimer = 0f;
            }

            yield return null;
        }
        // while (true)
        // {
        //     var timer = 0f;
        //     if (Input.Count >= Input.Number && Output.CanAdd)
        //     {
        //         IsWorking = true;
        //         TweenWork.Sample(0f);
        //         while (timer <= WorkDuration)
        //         {
        //             timer += DeltaTime;
        //             if (timer >= WorkDuration)
        //             {
        //                 timer = WorkDuration;
        //             }
        //
        //             WorkProgress = timer / WorkDuration;
        //
        //             if (WorkProgress >= 1)
        //             {
        //                 for (var i = 0; i < Input.Number; i++)
        //                 {
        //                     Input.Remove(Input.Number);
        //                 }
        //
        //                 Output.Add(Output.Number);
        //                 IsWorking = false;
        //
        //                 TweenWork.Sample(WorkProgress);
        //                 Refresh();
        //                 break;
        //             }
        //
        //             Refresh();
        //             TweenWork.Sample(0f);
        //             yield return null;
        //         }
        //
        //         yield return YieldBuilder.WaitForSeconds(WorkInterval);
        //     }
    }
}
