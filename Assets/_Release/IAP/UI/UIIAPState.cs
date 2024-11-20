using System;
using Sirenix.OdinInspector;
using UnityEngine;

[AddComponentMenu("UI/IAP/UI IAP State")]
public class UIIAPState : UIBase
{
    [Title("IAP")]
    public string Key;

    public GameObject CanBuyShowObj;
    public GameObject CantBuyShowObj;
    public GameObject IsOwnObj;

    [NonSerialized] public IAPProduction Production;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (string.IsNullOrEmpty(Key)) return;
        if (IAPSetting.Ins.IapProductDic.TryGetValue(Key, out var production))
        {
            Production = production;
        }
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        var checkCanBuy = CheckCanBuy();
        
        if (CanBuyShowObj != null)
        {
            CanBuyShowObj.SetActive(checkCanBuy);
        }

        if (CantBuyShowObj != null)
        {
            CantBuyShowObj.SetActive(!checkCanBuy);
        }

        if (IsOwnObj != null)
        {
            var isOwn = IAPManager.Instance.IsOwn(Production);
            IsOwnObj.SetActive(isOwn);
        }
    }

    public virtual bool CheckCanBuy()
    {
        var result = Production != null && Production.CanBuy;
        return result;
    }
}
