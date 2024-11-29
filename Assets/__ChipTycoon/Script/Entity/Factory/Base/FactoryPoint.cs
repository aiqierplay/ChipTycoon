using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public class FactoryPoint
{
    public bool Enable;
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

    public void Init()
    {
        StackList.Init();
        StackList.Prefab = TypeData.Prefab;
        Refresh();
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