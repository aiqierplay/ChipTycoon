using System;

public abstract class IAPChannelBase
{
    public abstract void Init(IAPSetting setting);

    public abstract void RemoveAds(Action<bool> onDone);

    public abstract bool IsRemoveAds();

    public abstract void Buy(IAPProduction production, Action<bool> onDone);

    public abstract bool IsOwn(IAPProduction production);

    public abstract void RestorePurchase();
}