using Aya.Data.Json;
using Aya.Data.Persistent;
using System;
using System.Collections.Generic;

[Serializable]
public abstract class SaveInfoCollection<TKey, TInfo> where TInfo : SaveInfoCollection<TKey, TInfo>, new()
{
    [JsonIgnore] public TKey Key;
    [JsonIgnore] public string SaveKey;
    [JsonIgnore] public bool SaveByLevel = false;

    
    [JsonIgnore] public static Dictionary<TKey, TInfo> InfoCacheDic = new Dictionary<TKey, TInfo>();
    [JsonIgnore] public static int CurrentLevelIndex = -1;

    public virtual void Init(TKey key, bool saveByLevel)
    {
        Key = key;
        SaveByLevel = saveByLevel;
        SaveKey = GetSaveKey(key, SaveByLevel);
    }

    public virtual void Save()
    {
        if (string.IsNullOrEmpty(SaveKey)) SaveKey = GetSaveKey(Key, SaveByLevel);
        USave.SetObject(SaveKey, this);
        USave.Save();
    }

    public virtual void Reset()
    {
       
    }

    public virtual void ResetSave()
    {
        Reset();
        Save();
    }

    public static string GetSaveKey(TKey key, bool saveByLevel)
    {
        if (saveByLevel)
        {
            var levelSaveKey = LevelManager.Ins.CurrentLevel.SaveKey;
            return $"{typeof(TInfo).Name}_{levelSaveKey}_{key}";
        }
        else
        {
            return $"{typeof(TInfo).Name}_{key}";
        }
    }

    public static SaveInfoCollectionKeyList GetKeyList(bool saveByLevel)
    {
        string key;
        if (saveByLevel)
        {
            var levelSaveKey = LevelManager.Ins.CurrentLevel.SaveKey;
            key = $"{typeof(TInfo).Name}_{levelSaveKey}_KeyList";
        }
        else
        {
            key = $"{typeof(TInfo).Name}_KeyList";
        }

        if (SaveInfoCollectionKeyList.KeyListCacheDic.TryGetValue(key, out var keyList))
        {
            return keyList;
        }

        keyList = USave.GetObject<SaveInfoCollectionKeyList>(key);
        if (keyList == null)
        {
            keyList = new SaveInfoCollectionKeyList();
        }

        keyList.Init(key);
        SaveInfoCollectionKeyList.KeyListCacheDic.Add(key, keyList);
        return keyList;
    }

    public static TInfo GetInfo(TKey key, bool saveByLevel)
    {
        if (IsLevelChanged()) InfoCacheDic.Clear();
        if (InfoCacheDic.TryGetValue(key, out var info))
        {
            return info;
        }

        var saveKey = GetSaveKey(key, saveByLevel);
        info = USave.GetObject<TInfo>(saveKey);
        if (info == null)
        {
            info = new TInfo();
            info.Init(key, saveByLevel);
            info.Reset();
            var keyList = GetKeyList(saveByLevel);
            keyList.Add(saveKey);
        }
        else
        {
            info.Init(key, saveByLevel);
        }

        InfoCacheDic.Add(key, info);
        return info;
    }

    public static void ClearAll(bool saveByLevel)
    {
        var keyList = GetKeyList(saveByLevel);
        foreach (var key in keyList.KeyList)
        {
            USave.SetObject<TInfo>(key, null);
        }

        keyList.ClearAll();
        USave.Save();
    }

    public static bool IsLevelChanged()
    {
        var levelIndex = LevelManager.Ins.CurrentLevel.Index;
        if (CurrentLevelIndex < 0)
        {
            CurrentLevelIndex = levelIndex;
            return false;
        }

        if (CurrentLevelIndex != levelIndex)
        {
            CurrentLevelIndex = levelIndex;
            return true;
        }

        return false;
    }
}