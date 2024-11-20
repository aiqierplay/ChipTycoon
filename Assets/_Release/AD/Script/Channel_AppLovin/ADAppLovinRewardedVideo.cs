#if APPLOVIN
using Aya.AD;
using Aya.Async;
using Aya.SDK;
using System;

// https://dash.applovin.com/documentation/mediation/unity/ad-formats/rewarded-ads
public class ADAppLovinRewardedVideo : ADAppLovinSource
{
    public override ADLocationType Type => ADLocationType.RewardedVideo;

    public override bool IsReady
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            var result = MaxSdk.IsRewardedAdReady(UnitID);
            SDKDebug.Log("AD", Name + "\tReady\t" + result);
            return result;
#endif
        }
    }

    public Action<bool> OnLoadAction = delegate { };
    public Action<bool> OnShowAction = delegate { };

    public override void Init(params object[] args)
    {
        if (IsInited) return;
        UnitId = args[0] as string;

        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        IsInited = true;
        OnInited(true);
        SDKDebug.Log("AD", Name + "\tInit Success");
    }

    public override void Load(Action<bool> onDone = null)
    {
        if (!IsInited) return;
        if (IsLoading) return;
        OnLoadAction = onDone;
        MaxSdk.LoadRewardedAd(UnitId);
        IsLoading = true;
        SDKDebug.Log("AD", Name + "\tLoad Start");
    }

    public override void Show(Action<bool> onDone = null)
    {
        if (!IsInited) return;
        if (IsShowing) return;
        OnShowAction = onDone;
        IsShowing = true;
        MaxSdk.ShowRewardedAd(UnitId);
        SDKDebug.Log("AD", Name + "\tShow Request");

        // 防止 Close 回调不触发，修改Showing状态
        UnityThread.ExecuteDelay(() => { IsShowing = false; }, 100f);
#if UNITY_EDITOR
        // OnResult?.Invoke(true);
        IsShowing = false;
#endif
    }

    public override void Close()
    {
    }

    #region Callback
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            IsLoading = false;
            OnLoadAction?.Invoke(true);
            SDKDebug.Log("AD", Name + "\tLoad Success");

            SDKUtil.Event($"s_ad_load_success_{Name}");
            SDKUtil.Event("s_ad_load_success");
        });
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            IsLoading = false;
            OnLoadAction?.Invoke(false);
            SDKDebug.Error("AD", Name + "\tLoad Fail   " + errorInfo.Code + " " + errorInfo.AdLoadFailureInfo);

            SDKUtil.Event($"s_ad_load_fail_{Name}");
            SDKUtil.Event("s_ad_load_fail");

            UnityThread.ExecuteDelay(() => Load(), 5f);
        });
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            Banner?.StopAutoRefresh();
            IsShowing = true;
            OnShowed?.Invoke();
            OnShowAction?.Invoke(true);
            GameManager.Ins.GamePause();
            SDKDebug.Log("AD", Name + "\tShow Success");
        });
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            IsShowing = false;
            OnResult?.Invoke(false);
            OnShowAction?.Invoke(false);
            UnityThread.ExecuteDelay(() => Load(), 5f);
            SDKDebug.Error("AD", Name + "\tShow Fail   " + errorInfo.Code + " " + errorInfo.AdLoadFailureInfo);
        });
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            SDKDebug.Log("AD", Name + "\tClicked");
        });
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            Banner?.StartAutoRefresh();
            IsShowing = false;
            GameManager.Ins.GameResume();
            Load();
            SDKDebug.Log("AD", Name + "\tHidden");
        });
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            IsShowing = false;
            var result = reward.IsValid();
            OnResult?.Invoke(result);
            SDKDebug.Log("AD", Name + "\tReward " + (result ? "Success" : "Failed"));
        });
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        
    }

    #endregion
}
#endif