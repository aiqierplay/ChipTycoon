using System;
using System.Collections.Generic;
using Aya.Data.Persistent;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class IAPProduction
{
    [VerticalGroup("Info")] public string Key;
    [VerticalGroup("Info")] public float Cost;
    [VerticalGroup("Info")] public int BuyLimit = 1;
    [VerticalGroup("Info"), SerializeReference] public List<IAPCondition> ConditionList = new List<IAPCondition>();

    public string IsBuyKey
    {
        get
        {
            if (string.IsNullOrEmpty(_isBuyKey)) _isBuyKey = "IAP_IsBuy_" + Key;
            return _isBuyKey;
        }
    }

    private string _isBuyKey;

    public bool IsBuy
    {
        get => PlayerPrefs.GetInt(IsBuyKey, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(IsBuyKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public string BuyCountKey
    {
        get
        {
            if (string.IsNullOrEmpty(_buyCountKey)) _buyCountKey = "IAP_BuyCount_" + Key;
            return _buyCountKey;
        }
    }

    private string _buyCountKey;

    public int BuyCount
    {
        get => PlayerPrefs.GetInt(BuyCountKey, 0);
        set
        {
            PlayerPrefs.SetInt(BuyCountKey, value);
            PlayerPrefs.Save();
        }
    }

    public virtual bool CanBuy
    {
        get
        {
            if (ConditionList != null && ConditionList.Count > 0)
            {
                foreach (var condition in ConditionList)
                {
                    if (!condition.Check()) return false;
                }
            }

            if (BuyLimit > 0 && BuyCount >= BuyLimit) return false;
            return true;
        }
    }

    public virtual void Init()
    {
        if (IsBuy)
        {
            Enable();
        }
    }

    public virtual void Buy(Action onSuccess = null, Action onFail = null)
    {
        IAPManager.Instance.Buy(this , result =>
        {
            if (result)
            {
                BuySuccess(onSuccess);
            }
            else
            {
                BuyFail(onFail);
            }
        });
    }

    public virtual void BuySuccess(Action onSuccess = null)
    {
        IsBuy = true;
        BuyCount++;
        OnBuySuccess();
        Enable();
        onSuccess?.Invoke();
    }

    public virtual void BuyFail(Action onFail = null)
    {
        OnBuyFail();
        onFail?.Invoke();
    }
    
    // 默认关闭，购买后生效的功能，每次启动游戏都需要激活，比如去广告
    public abstract void Enable();

    // 购买后一次性发放物品，金币，道具等，在此处理
    public abstract void OnBuySuccess();

    public abstract void OnBuyFail();
}
