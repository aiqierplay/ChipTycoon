using Aya.Data;
using Aya.Extension;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ConfigManager : EntityBase<ConfigManager>
{
    public static MultiDictionary<Type, string, TextAsset> TextAssetLoadCacheDic = new MultiDictionary<Type, string, TextAsset>();
    public static Dictionary<TextAsset, List<ConfigData>> TextAssetDataListCacheDic = new Dictionary<TextAsset, List<ConfigData>>();
    public static Dictionary<TextAsset, object> TextAssetDataListTCacheDic = new Dictionary<TextAsset, object>();

    public const string RootPath = "Config/";
    public static Func<string, TextAsset> LoadAction = Resources.Load<TextAsset>;

    #region Get Data

    public T GetData<T>(int index, string assetLoadPath = "") where T : ConfigData
    {
        var dataType = typeof(T);
        return GetData(dataType, index, assetLoadPath) as T;
    }

    public ConfigData GetData(Type dataType, int index, string assetLoadPath = "")
    {
        var textAsset = LoadTextAsset(dataType, assetLoadPath);
        var dataList = GetDataList(textAsset, dataType);
        if (index < 0 || index >= dataList.Count) return null;
        return dataList[index];
    }

    public T GetData<T>(Predicate<T> predicate, string assetLoadPath = "") where T : ConfigData
    {
        var dataType = typeof(T);
        var textAsset = LoadTextAsset(dataType, assetLoadPath);
        var dataList = GetDataList<T>(textAsset);
        for (var i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            if (predicate != null && predicate(data)) return data;
        }

        return null;
    }

    public T GetData<T>(TextAsset textAsset, int index) where T : ConfigData
    {
        var dataType = typeof(T);
        var dataList = GetDataList(textAsset, dataType);
        if (index < 0 || index >= dataList.Count) return null;
        return dataList[index] as T;
    }

    public ConfigData GetData(TextAsset textAsset, Type dataType, int index)
    {
        var dataList = GetDataList(textAsset, dataType);
        if (index < 0 || index >= dataList.Count) return null;
        return dataList[index];
    }

    public T GetData<T>(TextAsset textAsset, Predicate<T> predicate) where T : ConfigData
    {
        var dataList = GetDataList<T>(textAsset);
        for (var i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            if (predicate != null && predicate(data)) return data;
        }

        return null;
    }

    #endregion

    #region Get Data List

    public List<T> GetDataList<T>(string assetLoadPath = "") where T : ConfigData
    {
        var dataType = typeof(T);
        var textAsset = LoadTextAsset(dataType, assetLoadPath);
        return GetDataList<T>(textAsset);
    }

    public List<T> GetDataList<T>(Predicate<T> predicate, string assetLoadPath = "") where T : ConfigData
    {
        var dataType = typeof(T);
        var textAsset = LoadTextAsset(dataType, assetLoadPath);
        return GetDataList(textAsset, predicate);
    }

    public List<ConfigData> GetDataList(Type dataType, string assetLoadPath = "")
    {
        var textAsset = LoadTextAsset(dataType, assetLoadPath);
        var dataList = LoadDataList(textAsset, dataType);
        return dataList;
    }

    public List<T> GetDataList<T>(TextAsset textAsset, Predicate<T> predicate) where T : ConfigData
    {
        var result = new List<T>();
        var dataList = GetDataList<T>(textAsset);
        for (var i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i] as T;
            if (predicate == null || predicate(data)) result.Add(data);
        }

        return result;
    }

    public List<T> GetDataList<T>(TextAsset textAsset) where T : ConfigData
    {
        if (TextAssetDataListTCacheDic.TryGetValue(textAsset, out var cacheDataList))
        {
            var result = cacheDataList as List<T>;
            return result;
        }

        var dataList = new List<T>();
        var dataType = typeof(T);
        var tempDataList = GetDataList(textAsset, dataType);
        for (var i = 0; i < tempDataList.Count; i++)
        {
            var data = tempDataList[i] as T;
            dataList.Add(data);
        }

        TextAssetDataListTCacheDic.Add(textAsset, dataList);
        return dataList;
    }

    public List<ConfigData> GetDataList(TextAsset textAsset, Type dataType)
    {
        if (TextAssetDataListCacheDic.TryGetValue(textAsset, out var dataList))
        {
            return dataList;
        }
        else
        {
            return LoadDataList(textAsset, dataType);
        }
    }

    #endregion

    #region Load Data List

    public List<ConfigData> LoadDataList(TextAsset textAsset, Type dataType)
    {
        List<ConfigData> dataList;
        if (TextAssetDataListCacheDic.TryGetValue(textAsset, out var cacheDataList))
        {
            dataList = cacheDataList;
        }
        else
        {
            var lineArray = textAsset.text.Split('\n');
            var listType = typeof(List<>).MakeGenericType(typeof(ConfigData));
            dataList = Activator.CreateInstance(listType) as List<ConfigData>;
            if (dataList == null) return default;
            var fieldList = new List<FieldInfo>();
            var fieldCount = 0;
            var dataCount = lineArray.Length - 1;
            for (var rowIndex = 0; rowIndex < lineArray.Length; rowIndex++)
            {
                var line = lineArray[rowIndex].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                var strArray = line.Split(new[] { ',' }, StringSplitOptions.None);
                if (rowIndex == 0)
                {
                    for (var columnIndex = 0; columnIndex < strArray.Length; columnIndex++)
                    {
                        var fieldName = strArray[columnIndex];
                        var fieldInfo = dataType.GetField(fieldName);
                        fieldList.Add(fieldInfo);
                    }

                    fieldCount = fieldList.Count;
                }
                else
                {
                    var dataIndex = rowIndex - 1;
                    var data = Activator.CreateInstance(dataType) as ConfigData;
                    for (var columnIndex = 0; columnIndex < fieldCount && columnIndex < strArray.Length; columnIndex++)
                    {
                        var filedInfo = fieldList[columnIndex];
                        var strValue = strArray[columnIndex].Trim();
                        if (string.IsNullOrEmpty(strValue)) continue;
                        var value = ConfigValueConvertor.Convert(strValue, filedInfo.FieldType);
                        filedInfo.SetValue(data, value);
                    }

                    data.SetField(nameof(ConfigData.IsMaxLevel), dataIndex >= dataCount - 1);
                    dataList.Add(data);
                }
            }

            TextAssetDataListCacheDic.Add(textAsset, dataList);
        }

        return dataList;
    }

    #endregion

    #region Load TextAsset

    public TextAsset LoadTextAsset(Type dataType, string assetLoadPath = "")
    {
        var path = RootPath;
        if (assetLoadPath == null) assetLoadPath = "";
        if (TextAssetLoadCacheDic.TryGetValue(dataType, assetLoadPath, out var existTextAsset))
        {
            return existTextAsset;
        }

        // AB
        var abConfig = ABTestSetting.Ins.Config;
        if (!string.IsNullOrEmpty(abConfig))
        {
            path = abConfig;
            if (!string.IsNullOrEmpty(assetLoadPath))
            {
                path += $"{assetLoadPath}/";
            }

            path += dataType.Name;
        }

        var textAsset = LoadAction(path);
        if (textAsset == null)
        {
            path = RootPath;
            if (!string.IsNullOrEmpty(assetLoadPath))
            {
                path += $"{assetLoadPath}/";
            }

            path += dataType.Name;
            textAsset = LoadAction(path);
        }

        if (textAsset == null)
        {
            path = RootPath;
            path += dataType.Name;
            textAsset = LoadAction(path);
        }

        if (textAsset == null)
        {
            Log(dataType, assetLoadPath, "Load Failed!");
            return null;
        }

        TextAssetLoadCacheDic.Add(dataType, assetLoadPath, textAsset);
        return textAsset;
    }

    #endregion

    public void ClearCache()
    {
        TextAssetLoadCacheDic.Clear();
        TextAssetDataListCacheDic.Clear();
        TextAssetDataListTCacheDic.Clear();
    }
}