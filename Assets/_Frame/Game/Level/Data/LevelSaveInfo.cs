using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelSaveInfo : SaveInfoList<LevelSaveInfo>
{
    public int Power;

    public bool IsLock = true;
    public bool IsPass = false;

    public Vector3 DiggerPos = new Vector3();

    public int DropProductCount;
    public List<int> DiggableState = new List<int>();

    public void UnLock()
    {
        if (!IsLock) return;
        IsLock = false;
        Save();
    }

    public void Pass()
    {
        if (IsPass) return;
        IsPass = true;
        Save();
    }

    public override void Reset()
    {
        IsLock = true;
        IsPass = false;
        DiggerPos = Vector3.zero;
        if (Index <= 1) IsLock = false;
        DiggableState.Clear();
        DropProductCount = 0;
    }
}