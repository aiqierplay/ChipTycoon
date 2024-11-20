using System;
using Aya.Events;

[Serializable]
public class IAPRemoveAds : IAPProduction
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

#if SuperSonic && SuperSonic_Stage_3
    public override void Buy(Action onSuccess = null, Action onFail = null)
    {
        IAPManager.Instance.RemoveAds(result =>
        {
            if (result)
            {
                IsBuy = true;
                BuyCount++;
                OnBuySuccess();
                Enable();
                onSuccess?.Invoke();
            }
            else
            {
                OnBuyFail();
                onFail?.Invoke();
            }
        });
    }
#endif
}
