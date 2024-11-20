using System;
using Aya.Events;

[Serializable]
public class IAPRemoveAdsForPackage : IAPProduction
{
    public override void Enable()
    {
        SDKUtil.RemoveAds.Value = true;
        UEvent.Dispatch(GameEvent.NoAds);
    }

    public override void OnBuySuccess()
    {
    }

    public override void OnBuyFail()
    {
       
    }

    public override void Buy(Action onSuccess = null, Action onFail = null)
    {
        IsBuy = true;
        BuyCount++;
    }
}
