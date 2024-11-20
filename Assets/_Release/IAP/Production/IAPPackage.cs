using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IAPPackage : IAPProduction
{
    [SerializeReference] public List<IAPProduction> ProductList = new List<IAPProduction>();

    public override void Init()
    {
        foreach (var production in ProductList)
        {
            production.Init();
        }
    }

    public override void BuySuccess(Action onSuccess = null)
    {
        IsBuy = true;
        BuyCount++;
        foreach (var production in ProductList)
        {
            if (string.IsNullOrEmpty(production.Key)) continue;
            if (!production.CanBuy) continue;
            production.BuySuccess();
        }

        OnBuySuccess();
        Enable();
        onSuccess?.Invoke();
    }

    public override void OnBuySuccess()
    {
        ProductList.ForEach(production => production.OnBuySuccess());
    }

    public override void OnBuyFail()
    {

        ProductList.ForEach(production => production.OnBuyFail());
    }

    public override void Enable()
    {
        ProductList.ForEach(production => production.Enable());
    }
}
