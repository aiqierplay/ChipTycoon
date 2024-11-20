using System;
using System.Collections.Generic;
using Aya.Data.Persistent;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;
using Aya.UI.Markup;
#if SuperSonic
using SupersonicWisdomSDK;
#endif

[Serializable]
public class ABTestGroup
{
    [VerticalGroup("Param"), TableColumnWidth(60)] public string Group;

#if SuperSonic
    [VerticalGroup("Param")] public string SdkKey;
#endif

    [VerticalGroup("Param"), TableColumnWidth(40)] public string TestKey;
    [VerticalGroup("Param"), TableColumnWidth(40)] public string TestValue;

#if SMILE_SDK
    [VerticalGroup("Smile SDK"), TableColumnWidth(60)] public string Div;
    [VerticalGroup("Smile SDK"), TableColumnWidth(60)] public string Param;
#endif

    [TableList, TableColumnWidth(140)] public List<ABTestConfig> ConfigList;

    [NonSerialized] public Dictionary<string, ABTestConfig> ConfigDic;

    [NonSerialized] public sString SaveKey;

    // #if GA
    //     private bool _isRemoteConfigReady;
    // #endif

    public string CurrentKey
    {
        get
        {
            // #if GA
            //             if (!_isRemoteConfigReady)
            //             {
            //                 _isRemoteConfigReady = GameAnalytics.IsRemoteConfigsReady();
            //             }
            //
            //             if(_isRemoteConfigReady)
            //             {
            //                 var value = GameAnalytics.GetRemoteConfigsValueAsString(Group, ConfigList.First().Value);
            //                 return value;
            //             }
            // #endif

            if (!string.IsNullOrEmpty(TestKey) && ConfigDic.ContainsKey(TestKey)) return TestKey;

#if SMILE_SDK
            SmileSDK.ABTest.triggerDiversion(Div);
            var value = SmileSDK.ABTest.GetParamStringValue(Param, "0");
            if (string.IsNullOrEmpty(value))
            {
                value = "0";
            }

            return ConfigList[value.AsInt()].Group;
#endif

#if SuperSonic
            try
            {
                var key = SupersonicWisdom.Api.GetConfigValue(SdkKey, ConfigList[0].Key);
                return key;
            }
            catch
            {
                Debug.Log("ABTest".ToMarkup(Color.red) +  "Get " + Group + " Failed");
                return ConfigList[0].Key;
            }

#endif

            return SaveKey.Value;
        }
    }

    public ABTestConfig CurrentConfig => GetConfig(CurrentKey);

    public string CurrentValue
    {
        get
        {
            if (!string.IsNullOrEmpty(TestValue)) return TestValue;
            return CurrentConfig.Value;
        }
    }

    public virtual void Init()
    {
        SaveKey = new sString($"{nameof(ABTestGroup)}_{Group}", null);
        ConfigDic = ConfigList.ToDictionary(c => c.Key);
        if (string.IsNullOrEmpty(SaveKey.Value))
        {
            SaveKey.Value = ConfigList.Random(c => c.Weight).Key;
        }
    }

    public ABTestConfig GetConfig(string configKey)
    {
        return ConfigDic.TryGetValue(configKey, out var config) ? config : null;
    }

    public string GetValue(string configKey = null, string defaultValue = null)
    {
        if (!string.IsNullOrEmpty(TestValue)) return TestValue;
        if (string.IsNullOrEmpty(configKey)) configKey = CurrentKey;
        return ConfigDic.TryGetValue(configKey, out var config) ? config.Value : defaultValue;
    }

    public T GetValue<T>(T defaultValue = default) where T : struct
    {
        var value = GetValue(CurrentKey);
        if (string.IsNullOrEmpty(value)) return defaultValue;
        var result = value.CastType<T>();
        return result;
    }

    public T GetValue<T>(string configKey = null, T defaultValue = default)
    {
        if (string.IsNullOrEmpty(configKey)) configKey = CurrentKey;
        var value = GetValue(configKey);
        if (string.IsNullOrEmpty(value)) return defaultValue;
        var result = value.CastType<T>();
        return result;
    }
}