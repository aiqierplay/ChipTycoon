using System;
using System.Collections.Generic;

[Serializable]
public class LevelSaveInfo : SaveInfoList<LevelSaveInfo>
{
    public float Power;

    public bool IsLock = true;
    public bool IsPass = false;

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
        if (Index <= 1) IsLock = false;
        DiggableState.Clear();
        DropProductCount = 0;
    }
}