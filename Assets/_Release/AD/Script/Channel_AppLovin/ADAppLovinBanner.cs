#if APPLOVIN
using Aya.AD;
using Aya.Async;
using Aya.SDK;
using System;
using UnityEngine;

// https://dash.applovin.com/documentation/mediation/unity/ad-formats/banners
public class ADAppLovinBanner : ADAppLovinSource
{
    public override ADLocationType Type => ADLocationType.Banner;

    public override bool IsReady => IsBannerReady;

    protected bool IsBannerReady;

    public Action<bool> OnLoadAction = delegate { };
    public Action<bool> OnShowAction = delegate { };

    public override void Init(params object[] args)
    {
        if (IsInited) return;
        UnitId = args[0] as string;

        IsBannerReady = false;
        MaxSdk.CreateBanner(UnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerBackgroundColor(UnitId, Color.clear);
        MaxSdk.SetBannerExtraParameter(UnitId, "adaptive_banner", "true");

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

        // var density = MaxSdkUtils.GetScreenDensity();
        // var dp = pixels / density;

        IsInited = true;
        OnInited(true);
        SDKDebug.Log("AD", Name + "\tInit Success");
    }

    public override void Load(Action<bool> onDone = null)
    {
        if (!IsInited) return;
        if (IsLoading) return;
        OnLoadAction = onDone;

        IsBannerReady = false;
        StopAutoRefresh();
        MaxSdk.LoadBanner(UnitId);

        IsLoading = true;
        SDKDebug.Log("AD", Name + "\tLoad Start");
    }

    public override void Show(Action<bool> onDone = null)
    {
        if (!IsInited) return;
        if (IsShowing) return;
        OnShowAction = onDone;
        IsShowing = true;
        MaxSdk.ShowBanner(UnitId);

        SDKDebug.Log("AD", Name + "\tShow Request");

        // 防止 Close 回调不触发，修改Showing状态
        UnityThread.ExecuteDelay(() => { IsShowing = false; }, 100f);
#if UNITY_EDITOR
        OnResult(true);
        IsShowing = false;
#endif
    }

    public override void Close()
    {
        MaxSdk.HideBanner(UnitId);
        SDKDebug.Log("AD", Name + "\tClose Request");
    }

    public void StartAutoRefresh()
    {
        MaxSdk.StartBannerAutoRefresh(UnitId);
        SDKDebug.Log("AD", Name + "\tStart AutoRefresh");
    }

    public void StopAutoRefresh()
    {
        MaxSdk.StopBannerAutoRefresh(UnitId);
        SDKDebug.Log("AD", Name + "\tStop AutoRefresh");
    }

    #region Callback

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            IsBannerReady = true;
            IsLoading = false;
            OnLoadAction?.Invoke(true);
            if (GameManager.Ins.IsGaming)
            {
                Show();
            }

            SDKDebug.Log("AD", Name + "\tLoad Success");
        });
    }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            Close();
            StopAutoRefresh();
            IsBannerReady = false;
            IsLoading = false;
            OnLoadAction?.Invoke(false);
            UnityThread.ExecuteDelay(() => Load(), 5f);
            SDKDebug.Error("AD", Name + "\tLoad Fail   " + errorInfo.Code + " " + errorInfo.AdLoadFailureInfo);
        });
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            SDKDebug.Log("AD", Name + "\tClicked");
        });
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

    }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            StartAutoRefresh();
            GameManager.Ins.GamePause();
            SDKDebug.Log("AD", Name + "\tExpanded");
        });
    }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            StopAutoRefresh();
            GameManager.Ins.GameResume();
            SDKDebug.Log("AD", Name + "\tCollapsed");
        });
    }

    #endregion
}
#endif