using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("UI/IAP/UI IAP Button")]
public class UIIAPButton : UIButton
{
    [Title("IAP")]
    public string Key;

    public UnityEvent OnSuccessEvent;
    public UnityEvent OnFailEvent;

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
        if (Button != null)
        {
            Button.interactable = checkCanBuy;
        }
    }

    public virtual bool CheckCanBuy()
    {
        return Production != null && Production.CanBuy;
    }

    public override void OnClickImpl()
    {
        if (!CheckCanBuy()) return;
        Production.Buy(OnSuccess, OnFail);
    }

    public virtual void OnSuccess()
    {
        OnSuccessEvent?.Invoke();
    }

    public virtual void OnFail()
    {
        OnFailEvent?.Invoke();
    }
}
