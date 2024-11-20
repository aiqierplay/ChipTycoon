using System;
using System.Collections.Generic;
using System.Linq;

namespace Aya.AD
{
    public abstract class ADChannelBase
    {
        public bool IsInit { get; protected set; }

        #region Callback

        public Action<bool> OnInited = delegate { };

        public Action OnBannerShowed = delegate { };

        public Action OnInterstitialShowed = delegate { };
        public Action OnInterstitialClosed = delegate { };

        public Action<bool> OnRewardedVideoLoaded = delegate { };
        public Action OnRewardedVideoShowed = delegate { };
        public Action OnRewardedVideoClosed = delegate { };
        public Action<bool> OnRewardedVideoResult = delegate { };

        #endregion

        #region Cache

        public List<ADLocationBase> BannerList { get; protected set; }
        public Dictionary<string, ADLocationBase> BannerDic { get; protected set; }

        public ADLocationBase Banner
        {
            get
            {
                if (BannerList == null || BannerList.Count < 1) return null;
                return BannerList[0];
            }
        }

        public List<ADLocationBase> InterstitialList { get; protected set; }
        public Dictionary<string, ADLocationBase> InterstitialDic { get; protected set; }

        public ADLocationBase Interstitial
        {
            get
            {
                if (InterstitialList == null || InterstitialList.Count < 1) return null;
                return InterstitialList[0];
            }
        }

        public List<ADLocationBase> RewardedVideoList { get; protected set; }
        public Dictionary<string, ADLocationBase> RewardedVideosDic { get; protected set; }

        public ADLocationBase RewardedVideo
        {
            get
            {
                if (RewardedVideoList == null || RewardedVideoList.Count < 1) return null;
                return RewardedVideoList[0];
            }
        }

        #endregion

        public abstract void Init(params object[] args);
        public abstract void OnInit(params object[] args);

        public abstract void Load();

        public abstract void InitBanner(params object[] args);
        public abstract bool IsBannerReady();
        public abstract void ShowBanner(string key);
        public abstract void ShowBanner();
        public abstract void CloseBanner();

        public abstract void InitInterstitial(params object[] args);
        public abstract bool IsInterstitialReady();
        public abstract bool IsInterstitialReady(string key);
        public abstract void ShowInterstitial(string key, Action<bool> onDone = null);
        public abstract void ShowInterstitial(Action<bool> onDone = null);

        public abstract void InitRewardedVideo(params object[] args);
        public abstract bool IsRewardedVideoReady();
        public abstract bool IsRewardedVideoReady(string key);
        public abstract void ShowRewardedVideo(string key, Action<bool> onDone = null);
        public abstract void ShowRewardedVideo(Action<bool> onDone = null);
        public abstract bool IsRewardedVideoShowing();
    }

    public abstract class ADChannelBase<TLocationBanner, TLocationInterstitial, TLocationRewardedVideo> : ADChannelBase
        where TLocationBanner : ADLocationBase
        where TLocationInterstitial : ADLocationBase
        where TLocationRewardedVideo : ADLocationBase
    {
        #region Init

        public override void OnInit(params object[] args)
        {
            var setting = args[0] as ADSetting;
            if (setting == null)
            {
                OnInited(false);
                return;
            }

            InitBanner(args);
            InitInterstitial(args);
            InitRewardedVideo(args);

            IsInit = true;
            OnInited(true);
        }

        #endregion

        #region Load

        public override void Load()
        {
            if (!IsInit) return;
            foreach (var ad in BannerList)
            {
                ad.Load();
            }

            foreach (var ad in InterstitialList)
            {
                ad.Load();
            }

            foreach (var ad in RewardedVideoList)
            {
                ad.Load();
            }
        }

        #endregion

        #region Banner

        public override void InitBanner(params object[] args)
        {
            var setting = args[0] as ADSetting;
            if (setting == null)
            {
                OnInited(false);
                return;
            }

            // Banner
            BannerList = new List<ADLocationBase>();
            foreach (var param in setting.BannerList)
            {
                var banner = Activator.CreateInstance<TLocationBanner>();
                banner.OnShowed += () => { OnBannerShowed(); };
                banner.Init(param);
                BannerList.Add(banner);
            }

            BannerDic = BannerList.ToDictionary(ad => ad.Key);
        }

        public override bool IsBannerReady()
        {
            if (!IsInit) return false;
            if (Banner == null || !Banner.IsReady) return false;
            return Banner.IsReady;
        }

        public override void ShowBanner(string key)
        {
            if (!IsInit) return;
            if (BannerDic.TryGetValue(key, out var ad))
            {
                ad.Show();
            }
        }

        public override void ShowBanner()
        {
            if (!IsInit) return;
            if (Banner == null || !Banner.IsReady) return;
            Banner.Show();
        }

        public override void CloseBanner()
        {
            if (!IsInit) return;
            if (Banner == null || !Banner.IsReady) return;
            Banner.Close();
        }

        #endregion

        #region Interstitial

        public override void InitInterstitial(params object[] args)
        {
            var setting = args[0] as ADSetting;
            if (setting == null)
            {
                OnInited(false);
                return;
            }

            // Interstitial
            InterstitialList = new List<ADLocationBase>();
            foreach (var param in setting.InterstitialList)
            {
                var interstitial = Activator.CreateInstance<TLocationInterstitial>();
                interstitial.OnShowed += () => { OnInterstitialShowed(); };
                interstitial.OnClosed += () => { OnInterstitialClosed(); };
                interstitial.Init(param);
                InterstitialList.Add(interstitial);
            }

            InterstitialDic = InterstitialList.ToDictionary(ad => ad.Key);
        }

        public override bool IsInterstitialReady()
        {
            if (!IsInit) return false;
            if (Interstitial == null || !Interstitial.IsReady) return false;
            return Interstitial.IsReady;
        }

        public override bool IsInterstitialReady(string key)
        {
            if (!IsInit) return false;
            if (InterstitialDic.TryGetValue(key, out var ad))
            {
                return ad.IsReady;
            }

            return ad.IsReady;
        }

        public override void ShowInterstitial(string key, Action<bool> onDone = null)
        {
            if (!IsInit) return;
            if (InterstitialDic.TryGetValue(key, out var ad))
            {
                ad.Show(onDone);
            }
        }

        public override void ShowInterstitial(Action<bool> onDone = null)
        {
            if (!IsInit) return;
            if (Interstitial == null || !Interstitial.IsReady) return;
            Interstitial.Show(onDone);
        }

        #endregion

        #region RewardedVedio

        public override void InitRewardedVideo(params object[] args)
        {
            var setting = args[0] as ADSetting;
            if (setting == null)
            {
                OnInited(false);
                return;
            }

            // RewardedVideo
            RewardedVideoList = new List<ADLocationBase>();
            foreach (var param in setting.RewardedVideoList)
            {
                var rewardedVideo = Activator.CreateInstance<TLocationRewardedVideo>();
                rewardedVideo.OnLoaded += ret => { OnRewardedVideoLoaded(ret); };
                rewardedVideo.OnShowed += () => { OnRewardedVideoShowed(); };
                rewardedVideo.OnClosed += () => { OnRewardedVideoClosed(); };
                rewardedVideo.OnResult += ret => { OnRewardedVideoResult(ret); };
                rewardedVideo.Init(param);
                RewardedVideoList.Add(rewardedVideo);
            }

            RewardedVideosDic = RewardedVideoList.ToDictionary(ad => ad.Key);
        }

        public override bool IsRewardedVideoReady()
        {
            if (!IsInit) return false;
            if (RewardedVideo == null || !RewardedVideo.IsReady) return false;
            return RewardedVideo.IsReady;
        }

        public override bool IsRewardedVideoReady(string key)
        {
            if (!IsInit) return false;
            if (RewardedVideosDic.TryGetValue(key, out var ad))
            {
                return ad.IsReady;
            }

            return false;
        }

        public override void ShowRewardedVideo(string key, Action<bool> onDone = null)
        {
            if (!IsInit) return;
            if (RewardedVideosDic.TryGetValue(key, out var ad))
            {
                ad.Show(onDone);
            }
        }

        public override void ShowRewardedVideo(Action<bool> onDone = null)
        {
            if (!IsInit) return;
            if (RewardedVideo == null || !RewardedVideo.IsReady) return;
            RewardedVideo.Show(onDone);
        }

        public override bool IsRewardedVideoShowing()
        {
            if (!IsInit) return false;
            foreach (var rewardedVideo in RewardedVideoList)
            {
                if (rewardedVideo.IsShowing) return true;
            }

            return false;
        }

        #endregion
    }
}