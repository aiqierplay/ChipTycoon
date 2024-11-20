using System;
using System.Collections.Generic;
using Aya.Data;
using UnityEngine;

// 存档 Key 值，使用 / 拼接
// 拼接顺序 Config/AB/Level_关卡号/自定义名/数据类型名

// 配置数据路径值，使用 / 拼接
// 拼接顺序 Config/AB/Level_关卡号/自定义名/数据类型名

public class UpgradeManager : EntityBase<UpgradeManager>
{
    public static Dictionary<TextAsset, MultiDictionary<Type, string, UpgradeInfo>> TextAssetInfoCacheDic = new Dictionary<TextAsset, MultiDictionary<Type, string, UpgradeInfo>>();

    #region Get Info

    public UpgradeInfo<T> GetInfo<T>(string saveKey = "") where T : ConfigData
    {
        var dataType = typeof(T);
        return GetInfo(dataType, saveKey) as UpgradeInfo<T>;
    }

    public UpgradeInfo GetInfo(Type dataType, string saveKey = "")
    {
        var textAsset = Config.LoadTextAsset(dataType, saveKey);
        return GetInfo(textAsset, dataType, saveKey);
    }

    public UpgradeInfo<T> GetInfo<T>(TextAsset textAsset, string saveKey = "") where T : ConfigData
    {
        var dataType = typeof(T);
        return GetInfo(textAsset, dataType, saveKey) as UpgradeInfo<T>;
    }

    public UpgradeInfo GetInfo(TextAsset textAsset, Type dataType, string saveKey = "")
    {
        var info = LoadInfo(textAsset, dataType, saveKey);
        return info;
    }

    #endregion

    #region Load Info

    public UpgradeInfo<T> LoadInfo<T>(TextAsset textAsset, string saveKey = "") where T : ConfigData
    {
        var dataType = typeof(T);
        return LoadInfo(textAsset, dataType, saveKey) as UpgradeInfo<T>;
    }

    public UpgradeInfo<T> LoadInfo<T>(string assetPath = "", string saveKey = "") where T : ConfigData
    {
        var dataType = typeof(T);
        return LoadInfo(dataType, saveKey) as UpgradeInfo<T>;
    }

    public UpgradeInfo LoadInfo(Type dataType, string saveKey = "")
    {
        var textAsset = Config.LoadTextAsset(dataType, saveKey);
        return LoadInfo(textAsset, dataType, saveKey);
    }

    public UpgradeInfo LoadInfo(TextAsset textAsset, Type dataType, string saveKey = "")
    {
        if (!TextAssetInfoCacheDic.TryGetValue(textAsset, out var infoDic))
        {
            infoDic = new MultiDictionary<Type, string, UpgradeInfo>();
            TextAssetInfoCacheDic.Add(textAsset, infoDic);
        }

        if (infoDic.TryGetValue(dataType, saveKey, out var existInfo))
        {
            return existInfo;
        }

        var infoType = typeof(UpgradeInfo<>).MakeGenericType(dataType);
        var info = Activator.CreateInstance(infoType) as UpgradeInfo;
        if (info == null) return default;

        var dataList = Config.LoadDataList(textAsset, dataType);
        info.GetType().GetField(nameof(UpgradeInfo.DataList)).SetValue(info, dataList);
        info.Init(saveKey);
        infoDic.Add(dataType, saveKey, info);

        return info;
    }

    #endregion

    public void ClearCache()
    {
        TextAssetInfoCacheDic.Clear();
    }
}
