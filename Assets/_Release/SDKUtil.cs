using System;
using Aya.AD;
using Aya.Async;
using Aya.Data.Persistent;
using Aya.Extension;
using Aya.UI.Markup;
using UnityEngine;
#if GA
using GameAnalyticsSDK;
#endif
#if UNITY_IOS && UnityAds
using Unity.Advertisement.IosSupport;
#endif

public static class SDKUtil
{
    public static sBool RemoveAds = new sBool(nameof(RemoveAds), false);

    public static void Init()
    {
        AppManager.Ins.Log("IDFA", GetAdvertisingId());

        try
        {
            RequestTrackingPermission();
            // AnalysisManager.Instance.Init();
            // ADManager.Init();
            // ADManager.Instance.ShowBanner();

            Delay.Ins.ExecuteNextFrame(() =>
            {
                Event("Game Init");
            });
        }
        catch (Exception e)
        {
            Debug.LogError("SDK Error \n" + e);
        }
    }

    #region AAID / IDFA

    public static string GetAdvertisingId()
    {
#if UNITY_ANDROID
        try
        {
            var advertisingId = "DEFAULT";
            var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            var jc2 = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
            var jo2 = jc2.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", jo);
            if (jo2 != null)
            {
                //获取广告id：
                advertisingId = jo2.Call<string>("getId");
                if (string.IsNullOrEmpty(advertisingId))
                {
                    advertisingId = "none";
                }

                //获取广告跟踪状态：当为false时，则无法根据用户行为定向推送广告，但看到的广告数量并不会减少
                var adTrackLimited = jo2.Call<bool>("isLimitAdTrackingEnabled");
            }

            return advertisingId;
        }
        catch
        {
            return "NULL";
        }
#elif UNITY_IOS
        return UnityEngine.iOS.Device.advertisingIdentifier;
#else

        return "Not Found!";
#endif
    }

    #endregion

    #region Permission
   
    public static void RequestTrackingPermission()
    {
#if UNITY_IOS && UnityAds
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
    }

    #endregion

    #region Event
  
    public static void Event(string eventName, params object[] args)
    {
        try
        {
#if false
            if (args == null || args.Length == 0)
            {
                GameAnalytics.NewDesignEvent(eventName);

                if(SDKSetting.Ins.EventLog)
                {
                    Debug.Log("[Event] ".ToMarkup(Color.cyan) + eventName.ToMarkup(Color.white));
                }
            }
            else if(args.Length % 2 ==0)
            {
                var argDic = new sDictionary<string, object>();
                for (var i = 0; i < args.Length; i += 2)
                {
                    var key = args[i].AsString();
                    var value = args[i + 1];
                    argDic.Add(key, value);
                }

                // GameAnalytics.NewDesignEvent(eventName, argDic);

                if (SDKSetting.Ins.EventLog)
                {
                    var param = "\t";
                    foreach (var kv in argDic)
                    {
                        param += "\n  " + kv.Key + " : " + kv.Value.ToMarkup(Color.gray) + " ";
                    }
                    
                    Debug.Log("[Event] ".ToMarkup(Color.cyan) + eventName.ToMarkup(Color.white) + param);
                }
            }
            else
            {
                Debug.LogError("Event " + eventName + " Param Error !");
            }
#endif
        }
        catch (Exception e)
        {
            Debug.LogError("SDK Error \n" + e);
        }
    }

    #endregion

    #region RewardVideo
   
    public static bool IsRewardVideoReady()
    {
        try
        {
            var result = ADManager.Instance.IsRewardedVideoReady();
            Event($"RewardVideo Is Ready : {result}");
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError("SDK Error \n" + e);
            return false;
        }
    }

    public static bool IsRewardVideoReady(string key)
    {
        try
        {
            var result = ADManager.Instance.IsRewardedVideoReady(key);
            Event($"RewardVideo {key} Is Ready : {result}");
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError("SDK Error \n" + e);
            return false;
        }
    }

    public static void RewardVideo(string key, Action onSuccess = null, Action onFail = null, Action onNoAds = null)
    {
        try
        {
            if (ADManager.Instance.IsRewardedVideoReady(key))
            {
                Event($"RewardVideo {key} Start");
                EntityBase.GlobalScale = 0f;
                ADManager.Instance.ShowRewardedVideo(key, result =>
                {
                    if (result)
                    {
                        Event($"RewardVideo {key} Success");
                        onSuccess?.Invoke();
                        EntityBase.GlobalScale = 1f;
                    }
                    else
                    {
                        Event($"RewardVideo {key} Fail");
                        onFail?.Invoke();
                        EntityBase.GlobalScale = 1f;
                    }
                });
            }
            else
            {
                Event($"RewardVideo {key} NoAds");
                onNoAds?.Invoke();
                EntityBase.GlobalScale = 1f;
            }
        }
        catch (Exception e)
        {
            onFail?.Invoke();
            EntityBase.GlobalScale = 1f;
            Debug.LogError("SDK Error \n" + e);
        }
    } 

    #endregion

    public static void Log(string message)
    {
        Debug.Log("[SDK]\t".ToMarkup(Color.yellow) + message);
    }
}
