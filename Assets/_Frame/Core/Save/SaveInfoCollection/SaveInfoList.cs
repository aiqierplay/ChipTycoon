using System;
using Aya.Data.Json;

[Serializable]
public abstract class SaveInfoList<TInfo> : SaveInfoCollection<int, TInfo> where TInfo : SaveInfoList<TInfo>, new()
{
    public int Index;

    [JsonIgnore] public virtual TInfo PreviousInfo => Index <= 0 ? null : GetInfo(Index - 1, SaveByLevel);
    [JsonIgnore] public virtual TInfo NextInfo => GetInfo(Index + 1, SaveByLevel);

    public override void Init(int key, bool saveByLevel)
    {
        Index = key;
        base.Init(key, saveByLevel);
    }
}