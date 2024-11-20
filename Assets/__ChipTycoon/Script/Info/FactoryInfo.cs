using System;
using System.Collections.Generic;

[Serializable]
public class FactoryInfo : SaveInfoList<FactoryInfo>
{
    public int InputCount;
    public int OutputCount;

    public override void Reset()
    {
        base.Reset();
        InputCount = 0;
        OutputCount = 0;
    }
}