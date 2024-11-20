using System;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

[CreateAssetMenu(fileName = "IAPSetting", menuName = "Setting/IAP Setting")]
public class IAPSetting : SettingBase<IAPSetting>
{
    [SerializeReference] public List<IAPProduction> IAPProductList;

    public Dictionary<string, IAPProduction> IapProductDic { get; set; }
    public Dictionary<Type, IAPProduction> IapFunctionTypeDic { get; set; }

    public override void Init()
    {
        base.Init();
        IapProductDic = IAPProductList.ToDictionary(i => i.Key);
        IAPProductList.ForEach(i => i.Init());

        IapFunctionTypeDic = new Dictionary<Type, IAPProduction>();
        IAPProductList.ForEach(i =>
        {
            IapFunctionTypeDic.TryAdd(i.GetType(), i);
        });
    }

    public bool HasFunction<T>()
    {
        return HasFunction(typeof(T));
    }

    public bool HasFunction(Type type)
    {
        if (IapFunctionTypeDic.TryGetValue(type, out var function))
        {
            return function.IsBuy;
        }

        return false;
    }
}
