using System.Collections.Generic;
using Aya.Data.Persistent;

public abstract class UpgradeInfo
{
    public string SaveKey;

    public List<ConfigData> DataList;

    public sInt Level;

    public virtual int Count => DataList.Count;
    public virtual bool IsMaxLevel { get; set; }
    public virtual bool CanUpgrade { get; set; }

    public ConfigData this[int index]
    {
        get
        {
            if (index < 0 || index >= Count) return null;
            return DataList[index];
        }
    }

    public ConfigData Current
    {
        get
        {
            var index = Level.Value - 1;
            var data = DataList[index];
            return data;
        }
    }

    public ConfigData Preview
    {
        get
        {
            var index = Level.Value - 2;
            if (index < 0) return null;
            var data = DataList[index];
            return data;
        }
    }

    public ConfigData Next
    {
        get
        {
            if (IsMaxLevel) return null;
            var index = Level.Value;
            var data = DataList[index];
            return data;
        }
    }

    public virtual void Init(string saveKey = "")
    {
        SaveKey = saveKey;
        Level = new sInt(SaveKey, 1);
    }

    public ConfigData GetData(int index)
    {
        return DataList[index];
    }

    public virtual bool Upgrade()
    {
        if (IsMaxLevel) return false;
        Current.Upgrade();
        Level.Value += 1;
        return true;
    }

    public virtual bool ForceUpgrade()
    {
        if (IsMaxLevel) return false;
        Current.ForceUpgrade();
        Level.Value += 1;
        return true;
    }

    public virtual void Clear()
    {
        Level.Value = 1;
    }
}