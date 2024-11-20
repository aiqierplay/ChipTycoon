using System;
using TMPro;
using UnityEngine;

[Serializable]
public class FactoryPoint
{
    public string Type;
    public int Max = -1;
    public Transform Pos;
    public StackList StackList;
    public int Number;
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
    }

    public void Refresh()
    {
        if (TextCount != null) TextCount.text = Count.ToString();
        if (MaxTipObj != null) MaxTipObj.SetActive(!CanAdd);
    }
}