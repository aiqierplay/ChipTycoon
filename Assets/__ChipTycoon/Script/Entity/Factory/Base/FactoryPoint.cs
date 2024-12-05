using Aya.Physical;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum FactoryPointMode
{
    Input = 0,
    Output = 1,
}

[Serializable]
public class FactoryPoint
{
    public bool Enable;
    public FactoryPointMode Mode;
    public GameObject RootObj;

    [ValueDropdown(nameof(TypeGetter))]
    public string Type;

    public IEnumerable TypeGetter() => ProductSetting.Ins.GetValueDropdownKeyList();

    public int Max = -1;
    public Transform Pos;
    public StackList StackList;
    public int Number = 1;
    public TMP_Text TextCount;
    public GameObject MaxTipObj;

    public ColliderListenerEnter ColliderListenerEnter;
    public ColliderListenerExit ColliderListenerExit;

    public ProductTypeData TypeData => ProductSetting.Ins.DataDic[Type];

    public bool CanAdd
    {
        get
        {
            if (Max <= 0) return true;
            return StackList.Count < Max;
        }
    }

    public bool CanWork => Count >= Number;

    public int Count => StackList.Count;
    public bool IsEmpty => StackList.Count == 0;

    [NonSerialized] public FactoryBase Factory;

    public void Init(FactoryBase factory)
    {
        Factory = factory;
        StackList.Init();
        StackList.Prefab = TypeData.Prefab;

        if (ColliderListenerEnter != null)
        {
            ColliderListenerEnter.Clear();
            ColliderListenerEnter.onTriggerEnter.Add<Worker>(OnEnter, LayerManager.Ins.Player);
        }

        if (ColliderListenerExit != null)
        {
            ColliderListenerExit.Clear();
            ColliderListenerExit.onTriggerExit.Add<Worker>(OnExit, LayerManager.Ins.Player);
        }

        Refresh();
    }

    public virtual void OnEnter(Worker worker)
    {
        if (worker == null) return;
        if (worker.Type != WorkerType.Player) return;
        worker.OnEnter(Factory, this);
    }

    public virtual void OnExit(Worker worker)
    {
        if (worker == null) return;
        if (worker.Type != WorkerType.Player) return;
        worker.OnExit(Factory, this);
    }

    public void Refresh()
    {
        if (RootObj != null) RootObj.SetActive(Enable);
        if (TextCount != null) TextCount.text = Count.ToString();
        if (MaxTipObj != null) MaxTipObj.SetActive(!CanAdd);
    }

    public void Add(int count)
    {
        StackList.Add(count, true);
        Refresh();
    }

    public void Remove(int count)
    {
        StackList.Remove(count);
        Refresh();
    }
}