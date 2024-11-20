using System;
using Aya.Data.Json;
using UnityEngine;

[Serializable]
public abstract class SaveInfoMatrix<TInfo> : SaveInfoCollection<Vector2Int, TInfo> where TInfo : SaveInfoMatrix<TInfo>, new()
{
    [JsonIgnore] public int X;
    [JsonIgnore] public int Y;

    public virtual void Init(int x, int y, bool saveByLevel)
    {
        var key = new Vector2Int(x, y);
        Init(key, saveByLevel);
    }

    public override void Init(Vector2Int key, bool saveByLevel)
    {
        X = key.x;
        Y = key.y;
        base.Init(key, saveByLevel);
    }

    public static string GetSaveKey(int x, int y, bool saveByLevel)
    {
        var key = new Vector2Int(x, y);
        return GetSaveKey(key, saveByLevel);
    }

    public static TInfo GetInfo(int x, int y, bool saveByLevel)
    {
        var key = new Vector2Int(x, y);
        return GetInfo(key, saveByLevel);
    }
}