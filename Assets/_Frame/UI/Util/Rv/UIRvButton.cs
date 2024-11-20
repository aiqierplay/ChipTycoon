using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIRvButton : UIBase
{
    [Title("Reward Video")]
    public string RvKey;
    public GameObject RvActiveObj;
    public GameObject RvDisableObj;

    [GetComponentInChildren, NonSerialized] public Button Button;

    [NonSerialized] public int Counter;

    protected override void Awake()
    {
        base.Awake();
        Button.onClick.AddListener(OnClick);
    }

    protected override void OnEnable()
    {
        Counter = 0;
        base.OnEnable();
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        RefreshRv();
    }

    public virtual bool IsReady()
    {
        var ready = SDKUtil.IsRewardVideoReady(RvKey);
        return ready;
    }

    public virtual void RefreshRv()
    {
        var isReady = IsReady();
        Button.interactable = isReady;
        if (RvActiveObj != null) RvActiveObj.SetActive(isReady);
        if (RvDisableObj != null) RvDisableObj.SetActive(!isReady);
    }

    public virtual void OnClick()
    {
        if (!IsReady()) return;
        Counter++;
        SDKUtil.RewardVideo(RvKey, () =>
        {
            OnRvSuccess();
            Refresh();
        }, () =>
        {
            OnRvFail();
            Refresh();
        });
    }

    public abstract void OnRvSuccess();

    public abstract void OnRvFail();
}