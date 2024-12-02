using System;

[Serializable]
public class FactoryInfo : SaveInfoList<FactoryInfo>
{
    public bool Unlock;
    public int UnlockSpent;

    public int InputCount;
    public int OutputCount;

    public override void Reset()
    {
        base.Reset();
        InputCount = 0;
        OutputCount = 0;
        UnlockSpent = 0;
        Unlock = false;
    }
}