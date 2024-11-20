using System;
using System.Collections.Generic;

[Serializable]
public class LevelListData
{
    public int StartRandIndex = -1;
    public List<Level> RandList = new List<Level>();

    public bool EnableRandLevel => StartRandIndex > 0;
    public int Count => RandList.Count;
}
