using System;
using System.Collections.Generic;
using Aya.Data.Json;
using Aya.Data.Persistent;

[Serializable]
public class SaveInfoCollectionKeyList
{
    public List<string> KeyList = new List<string>();

    [JsonIgnore] public string Key;
    [JsonIgnore] public static Dictionary<string, SaveInfoCollectionKeyList> KeyListCacheDic = new Dictionary<string, SaveInfoCollectionKeyList>();

    public void Init(string key)
    {
        Key = key;
    }

    public void Add(string key)
    {
        KeyList.Add(key);
        Save();
    }

    public void ClearAll()
    {
        KeyList.Clear();
        Save();
    }

    public void Save()
    {
        USave.SetObject(Key, this);
        USave.Save();
    }
}