#if APPLOVIN
using Aya.AD;
using Aya.Async;
using Aya.SDK;
using System;

// https://dash.applovin.com/documentation/mediation/unity/ad-formats/interstitials
public class ADAppLovinInterstitial : ADAppLovinSource
{
    public override ADLocationType Type => ADLocationType.Interstitial;

    public override bool IsReady
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            var result = MaxSdk.IsInterstitialReady(UnitID);
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

        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        IsInited = true;
        OnInited(true);
        SDKDebug.Log("AD", Name + "\tInit Success");
    }

    public override void Load(Action<bool> onDone = null)
    {
        if (!IsInited) return;
        if (IsLoading) return;
        OnLoadAction = onDone;
        MaxSdk.LoadInterstitial(UnitId);
        IsLoading = true;
        SDKDebug.Log("AD", Name + "\tLoad Start");
    }

    public override void Show(Action<bool> onDone = null)
    {
        if (!IsInited) return;
        if (IsShowing) return;
        OnResult = onDone;
        IsShowing = true;
        MaxSdk.ShowInterstitial(UnitId);
        SDKDebug.Log("AD", Name + "\tShow Request");

        // 防止 Close 回调不触发，修改Showing状态
        UnityThread.ExecuteDelay(() => { IsShowing = false; }, 100f);
#if UNITY_EDITOR
        // OnResult(true);
        IsShowing = false;
#endif
    }

    public override void Close()
    {
    }

    #region Callback

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            IsLoading = false;
            OnLoadAction?.Invoke(true);
            SDKDebug.Log("AD", Name + "\tLoad Success");
        });
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            IsLoading = false;
            OnLoadAction?.Invoke(false);
            SDKDebug.Error("AD", Name + "\tLoad Fail   " + errorInfo.Code + " " + errorInfo.AdLoadFailureInfo);
            UnityThread.ExecuteDelay(() => Load(), 5f);
        });
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
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

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
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

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            SDKDebug.Log("AD", Name + "\tClicked");
        });
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        UnityThread.ExecuteUpdate(() =>
        {
            Banner?.StartAutoRefresh();
            // MaxSdk.SetMuted(true);
            IsShowing = false;
            OnResult?.Invoke(true);
            GameManager.Ins.GameResume();
            Load();
            SDKDebug.Log("AD", Name + "\tHidden");
        });
    }

    #endregion
}
#endif