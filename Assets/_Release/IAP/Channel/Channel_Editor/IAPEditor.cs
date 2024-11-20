using System;

public class IAPEditor : IAPChannelBase
{
    
    public override void Init(IAPSetting setting)
    {
    }

    public override void RemoveAds(Action<bool> onDone)
    {
        onDone?.Invoke(true);
    }

    public override bool IsRemoveAds()
    {
        return true;
    }

    public override void Buy(IAPProduction production, Action<bool> onDone)
    {
        onDone?.Invoke(true);
    }

    public override bool IsOwn(IAPProduction production)
    {
        return true;
    }

    public override void RestorePurchase()
    {
        
    }
}
