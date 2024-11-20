#if SuperSonic && SuperSonic_Stage_3
using System;
using Aya.Async;
using Aya.SDK;
using SupersonicWisdomSDK;

// https://support.supersonic.com/hc/en-us/articles/9961385042589-Wisdom-SDK-Integration-Guide-Stage-3
public class IAPSuperSonic : IAPChannelBase
{
    public override void Init(IAPSetting setting)
    {
        
    }

    public static void OnInit(bool success)
    {
        SDKDebug.Log("IAP", "Init " + success);
    }

    public override void RemoveAds(Action<bool> onDone)
    {
        SupersonicWisdom.Api.BuyNoAds((product, isPurchased, failReason) =>
        {
            UnityThread.ExecuteUpdate(() => { onDone?.Invoke(isPurchased); });
        });
    }

    public override bool IsRemoveAds()
    {
        return SupersonicWisdom.Api.IsNoAds();
    }

    public override void Buy(IAPProduction production, Action<bool> onDone)
    {
#if UNITY_EDITOR
        onDone?.Invoke(true);
#else
        SupersonicWisdom.Api.BuyProduct(production.Key,
            (productId, isPurchased, failReason) =>
            {
                UnityThread.ExecuteUpdate(() => { onDone?.Invoke(isPurchased); });
            });
#endif
    }

    public override bool IsOwn(IAPProduction production)
    {
#if UNITY_EDITOR
        return production.IsBuy;
#else
        return SupersonicWisdom.Api.IsProductOwned(production.Key);
#endif
    }

    public override void RestorePurchase()
    {
#if UNITY_EDITOR
        OnRestore(true);
#else
        SupersonicWisdom.Api.RestorePurchases();
#endif
    }

    public void OnRestore(bool success)
    {
        if (success)
        {
            var productList = IAPSetting.Ins.IAPProductList;
            foreach (var production in productList)
            {
                if (IsOwn(production) && production is IAPRemoveAds)
                {
                    production.BuySuccess();
                }
            }
        }
    }
}
#endif