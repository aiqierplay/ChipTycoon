using System;

[Serializable]
public class LevelSaveInfo : SaveInfoList<LevelSaveInfo>
{
    public bool IsLock = true;
    public bool IsPass = false;

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
    }
}