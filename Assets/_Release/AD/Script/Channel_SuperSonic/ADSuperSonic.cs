#if SuperSonic && (SuperSonic_Stage2 || SuperSonic_Stage3)
using System;
using System.Collections.Generic;
using Aya.SDK;
using SupersonicWisdomSDK;
using UnityEngine;

namespace Aya.AD
{
    public class ADSuperSonic : ADChannelBase<ADSuperSonicLocationBanner, ADSuperSonicLocationInterstitial, ADSuperSonicLocationRewardedVideo>
    {
        public static float LastShowInterstitialTime = -1;
        public static float ShowInterstitialInterval = 20;

        public static void ResetInterstitialTime()
        {
            LastShowInterstitialTime = Time.realtimeSinceStartup;
        }

        public override void Init(params object[] args)
        {
            OnInit(args);
            Load();
        }

        public ADSuperSonicLocationRewardedVideo RewardedVideoLocation;

        public override void InitRewardedVideo(params object[] args)
        {
            var setting = args[0] as ADSetting;
            if (setting == null)
            {
                OnInited(false);
                return;
            }

            var param = setting.RewardedVideoList[0];
            RewardedVideoList = new List<ADLocationBase>();
            RewardedVideoLocation = new ADSuperSonicLocationRewardedVideo();

            RewardedVideoLocation.OnLoaded += ret => { OnRewardedVideoLoaded(ret); };
            RewardedVideoLocation.OnShowed += () => { OnRewardedVideoShowed(); };
            RewardedVideoLocation.OnClosed += () => { OnRewardedVideoClosed(); };
            RewardedVideoLocation.OnResult += ret => { OnRewardedVideoResult(ret); };
            RewardedVideoLocation.Init(param);

            RewardedVideoList.Add(RewardedVideoLocation);
        }

        public override bool IsRewardedVideoReady()
        {
            if (!IsInit) return false;
            return RewardedVideoLocation.IsReady;
        }

        public override bool IsRewardedVideoReady(string key)
        {
            if (!IsInit) return false;
            return RewardedVideoLocation.IsReady;
        }

        public override void ShowRewardedVideo(string key, Action<bool> onDone = null)
        {
            if (!IsInit) return;
            RewardedVideoLocation.SuperSonicRvKey = key;
            RewardedVideoLocation.Show(onDone);
        }

        public override void ShowRewardedVideo(Action<bool> onDone = null)
        {
            if (!IsInit) return;
            RewardedVideoLocation.SuperSonicRvKey = "UnDefine";
            RewardedVideoLocation.Show(onDone);
        }
    }

#region AD Editor Location

    public class ADSuperSonicLocationBanner : ADLocationBase<ADSuperSonicSourceBanner>
    {
    }

    public class ADSuperSonicLocationInterstitial : ADLocationBase<AdSuperSonicSourceInterstitial>
    {
    }

    public class ADSuperSonicLocationRewardedVideo : ADLocationBase<AdSuperSonicSourceRewardedVideo>
    {
        public string SuperSonicRvKey;
    }

#endregion

#region AD Editor Source

    public class ADSuperSonicSourceBanner : ADSuperSonicSourceBase
    {
        public override ADLocationType Type => ADLocationType.Banner;

        public override void Show(Action<bool> onDone = null)
        {
            SupersonicWisdom.Api.DisplayBanner();
        }

        public override void Close()
        {
            SupersonicWisdom.Api.DestroyBanner();
        }
    }

    public class AdSuperSonicSourceInterstitial : ADSuperSonicSourceBase
    {
        public override ADLocationType Type => ADLocationType.Interstitial;

        public override void Init(params object[] args)
        {
            base.Init(args);
            SupersonicWisdom.Api.AddOnAdClosedListener(adUnit => {
                if (adUnit == SwAdUnit.Interstitial)
                {
                    GameEntity.GlobalScale = 1f;
                }
            });

            SupersonicWisdom.Api.AddOnAdOpenedListener(adUnit => {
                if (adUnit == SwAdUnit.Interstitial)
                {
                    GameEntity.GlobalScale = 0f;
                }
            });
        }

        public override void Show(Action<bool> onDone = null)
        {
            var time = Time.realtimeSinceStartup;
            if (time - ADSuperSonic.LastShowInterstitialTime >= ADSuperSonic.ShowInterstitialInterval)
            {
                ADSuperSonic.ResetInterstitialTime();
            }
            else
            {
                return;
            }

#if UNITY_EDITOR
            OnShowed();
            onDone?.Invoke(true);
            OnResult(true);
#else
            SupersonicWisdom.Api.ShowInterstitial(UnitId);
#endif
        }
    }

    public class AdSuperSonicSourceRewardedVideo : ADSuperSonicSourceBase
    {
        public override ADLocationType Type => ADLocationType.RewardedVideo;

        public ADSuperSonicLocationRewardedVideo SuperSonicRvLocation => Location as ADSuperSonicLocationRewardedVideo;
        public string SuperSonicRvKey => SuperSonicRvLocation.SuperSonicRvKey;

#if UNITY_EDITOR
        public override bool IsReady => true;
#else
        public override bool IsReady => SupersonicWisdom.Api.IsRewardedVideoAvailable();
#endif

        public override void Init(params object[] args)
        {
            base.Init(args);

            SupersonicWisdom.Api.AddOnRewardedVideoAdAvailableEventListener(info =>
            {

            });

            SupersonicWisdom.Api.AddOnRewardedVideoAdUnavailableEventListener(() =>
            {

            });

            SupersonicWisdom.Api.AddOnAdClosedListener(adUnit => {
                if (adUnit == SwAdUnit.RewardedVideo)
                {
                    ADSuperSonic.ResetInterstitialTime();

                    if (!IsRewarded)
                    {
                        OnDone?.Invoke(false);
                        OnResult(false);
                    }
                    else
                    {
                        OnShowed();
                        OnDone?.Invoke(true);
                        OnResult(true);
                    }

                    SDKDebug.Log("AD", $"RewardedVideo {SuperSonicRvKey} Closed with Result {IsRewarded}");
                    IsRewarded = false;
                }
            });

            SupersonicWisdom.Api.AddOnAdOpenedListener(adUnit => {
                if (adUnit == SwAdUnit.RewardedVideo)
                {

                }
            });

            SupersonicWisdom.Api.AddOnRewardedVideoAdRewardedListener(result =>
            {
                IsRewarded = true;
                SDKDebug.Log("AD", $"RewardedVideo {SuperSonicRvKey} Rewarded");
            });
        }


        internal Action<bool> OnDone;
        internal bool IsRewarded;

        public override void Show(Action<bool> onDone = null)
        {
            OnDone = onDone;
            IsRewarded = false;
#if UNITY_EDITOR
            OnShowed();
            onDone?.Invoke(true);
            OnResult(true);
            ADSuperSonic.ResetInterstitialTime();
#else
            SupersonicWisdom.Api.ShowRewardedVideo(SuperSonicRvKey);
#endif

            SDKDebug.Log("AD", $"RewardedVideo {SuperSonicRvKey} Show");
        }
    }

    public abstract class ADSuperSonicSourceBase : ADSourceBase
    {
        public override ADLocationType Type => ADLocationType.None;

        public override bool IsReady => true;

        public string UnitId;

        public override void Init(params object[] args)
        {
            IsInited = true;
            UnitId = args[0] as string;
            OnInited(true);
        }

        public override void Load(Action<bool> onDone = null)
        {
            OnLoaded(true);
            onDone?.Invoke(true);
        }

        public override void Show(Action<bool> onDone = null)
        {
            OnShowed();
            onDone?.Invoke(true);
            OnResult(true);
        }

        public override void Close()
        {
            OnCloseed();
        }
    }

#endregion
}

#endif