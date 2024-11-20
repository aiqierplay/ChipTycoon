using UnityEngine;

[AddComponentMenu("UI/IAP/UI IAP Restore Button")]
public class UIIAPRestoreButton : UIButton
{
    public override void OnClickImpl()
    {
        IAPManager.Instance.RestorePurchase();
    }
}
