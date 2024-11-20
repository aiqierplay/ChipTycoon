using System;
using System.Collections.Generic;
using Aya.Data.Persistent;

public class UpgradeInfo<T> : UpgradeInfo where T : ConfigData
{
    public override void Init(string saveKey = "")
    {
        SaveKey = saveKey + "/" + typeof(T).Name;
        Level = new sInt(SaveKey, 1);
    }

    public new T this[int index] => base[index] as T;

    public new T GetData(int index)
    {
        var data = base.GetData(index) as T;
        return data;
    }

    public T GetData(Predicate<T> predicate)
    {
        for (var i = 0; i < DataList.Count; i++)
        {
            var data = DataList[i];
            var dataTemp = data as T;
            if (predicate(dataTemp)) return dataTemp;
        }

        return default;
    }

    public List<T> GetDataList(Predicate<T> predicate = null)
    {
        var result = new List<T>();
        foreach (var data in DataList)
        {
            var dataTemp = data as T;
            if (predicate == null || predicate(dataTemp)) result.Add(dataTemp);
        }

        return result;
    }

    public override bool IsMaxLevel => Level >= Count;
    public override bool CanUpgrade => Level < Count && Current.CanUpgrade();

    public new T Current => base.Current as T;
    public new T Preview => base.Preview as T;
    public new T Next => base.Next as T;

    public override bool Upgrade()
    {
        if (IsMaxLevel) return false;
        if (!Current.CanUpgrade()) return false;
        Current.Upgrade();
        Level.Value += 1;
        return true;
    }
}