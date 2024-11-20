using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
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

    
    [NonSerialized] public int Index; 
    [NonSerialized] public FactoryInfo Info;

    public void Init(int index)
    {
        Index = index;
        Info = FactoryInfo.GetInfo(index, true);
    }
}
