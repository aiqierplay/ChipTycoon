using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class ItemRvBase<TTarget> : ItemBase<TTarget>
    where TTarget : EntityBase
{
    [Title("Reward Video")]
    public string RvKey;
    public bool ActiveRv = true;
    public GameObject RvActiveObj;
    public GameObject RvDisableObj;

    [NonSerialized] public float RefreshTimer;
    [NonSerialized] public float RefreshInterval = 1f;

    public virtual void RefreshRv()
    {
        var isReady = IsReady();
        if (RvActiveObj != null) RvActiveObj.SetActive(isReady);
        if (RvDisableObj != null) RvDisableObj.SetActive(!isReady);
    }

    public virtual bool IsReady()
    {
        var ready = SDKUtil.IsRewardVideoReady(RvKey);
        return ready;
    }

    public virtual void Update()
    {
        RefreshTimer += DeltaTime;
        if (RefreshTimer >= RefreshInterval)
        {
            RefreshTimer -= RefreshInterval;
            RefreshRv();
        }
    }

    public override void OnTargetEffect(TTarget target)
    {
        if (ActiveRv)
        {
            OnBeforeRewardVideo(target);
            if (IsReady())
            {
                SDKUtil.RewardVideo(RvKey, () =>
                    {
                        OnRewardVideo(target, true);
                        Refresh();
                    },
                    () =>
                    {
                        OnRewardVideo(target, false);
                        Refresh();
                    });
            }
            else
            {
                OnRewardVideo(target, false);
                Refresh();
            }
        }
        else
        {
            OnBeforeRewardVideo(target);
            OnRewardVideo(target, true);
            Refresh();
        }
    }

    public abstract void OnBeforeRewardVideo(TTarget target);

    public abstract void OnRewardVideo(TTarget target, bool success);
}