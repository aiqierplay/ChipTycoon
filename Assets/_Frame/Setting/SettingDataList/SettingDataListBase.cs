using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public abstract class SettingDataListBase<TSetting, TData> : SettingBase<TSetting>
    where TSetting : SettingDataListBase<TSetting, TData>
    where TData : SettingDataBase
{
    public List<TData> DataList;

    public Dictionary<string, TData> DataDic
    {
        get
        {
            if (DataDicCache != null) return DataDicCache;
            DataDicCache = DataList.ToDictionary(data => data.Key);
            return DataDicCache;
        }
    }

    protected Dictionary<string, TData> DataDicCache;

    public ValueDropdownList<string> GetValueDropdownKeyList()
    {
        var result = new ValueDropdownList<string>();
        foreach (var subSetting in DataList)
        {
            var item = new ValueDropdownItem<string>(subSetting.Key, subSetting.Key);
            result.Add(item);
        }

        return result;
    }
}
