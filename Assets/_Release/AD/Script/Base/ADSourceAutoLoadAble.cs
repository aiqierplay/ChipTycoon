using System;
using System.Collections.Generic;
using Aya.Async;
using Aya.SDK;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aya.AD
{
    public enum SourceState
    {
        Unload = 0,
        Loading = 1,
        Ready = 2,
        Showing,
    }

    public abstract class ADSource : ADSourceBase
    {
        public int MaxFailLoadCount { get; } = 10;
        public int LoadFailCount { get; set; }
        public string UnitId { get; protected set; }
        public SourceState SourceState { get; private set; }

        public event Action OnShowRequest = delegate { };
        public event Action OnLoadRequest = delegate { };

        private Action<bool> _onLoadedCallback;

        public SourceState GetState()
        {
            return SourceState;
        }

        public override string ToString()
        {
            return Name + "_" + UnitId;
        }

        public override bool IsReady => SourceState == SourceState.Ready;

        public override void Init(params object[] args)
        {
            UnitId = args[0] as string;
            SourceState = SourceState.Unload;
            IsInited = true;
            SDKDebug.Log("AD", ToString() + "\t Init request");
        }

        public override void Load(Action<bool> onDone = null)
        {
            SourceState = SourceState.Loading;
            IsLoading = true;
            _onLoadedCallback = onDone;
            SDKDebug.Log("AD", ToString() + "\t Load request");
            OnLoadRequest?.Invoke();
        }

        public override void Show(Action<bool> onDone = null)
        {
            SourceState = SourceState.Showing;
            SDKDebug.Log("AD", ToString() + "\t Show request");
            OnShowRequest?.Invoke();
        }

        public override void Close()
        {
            SDKDebug.Log("AD", ToString() + "\t Close request");
        }

        public virtual void OnInit(bool isInit, string msg = null)
        {
            if (isInit) SDKDebug.Log("AD", ToString() + "\t OnInit Success!");
            else SDKDebug.Log("AD", ToString() + "\t OnInit Failed! " + msg ?? "");
            OnInited?.Invoke(isInit);
        }

        public virtual void OnLoad(bool isLoaded, string msg = null)
        {
            SourceState = isLoaded ? SourceState.Ready : SourceState.Unload;
            IsLoading = false;
            if (isLoaded) SDKDebug.Log("AD", ToString() + "\t OnLoad Success!");
            else SDKDebug.Log("AD", ToString() + "\t OnLoad Failed! " + msg ?? "");
            OnLoaded?.Invoke(isLoaded);
            _onLoadedCallback?.Invoke(isLoaded);
            _onLoadedCallback = null;
        }

        public virtual void OnShow(bool isShow, string msg = null)
        {
            SourceState = isShow ? SourceState.Showing : SourceState.Unload;
            if (isShow) SDKDebug.Log("AD", ToString() + "\t OnShow Success!");
            else SDKDebug.Log("AD", ToString() + "\t OnShow Failed! " + msg ?? "");
            if (isShow) OnShowed?.Invoke();
        }

        public virtual void OnClose()
        {
            SourceState = SourceState.Unload;
            SDKDebug.Log("AD", ToString() + "\t OnClose");
            OnCloseed?.Invoke();
        }

        public virtual void OnReward()
        {
            SourceState = SourceState.Unload;
            SDKDebug.Log("AD", ToString() + "\t OnReward");
            OnResult?.Invoke(true);
        }

    }

    public class ADRewardSource : ADSource
    {
        public override ADLocationType Type => ADLocationType.RewardedVideo;

        private Action<bool> _onShowedCallback;

        private bool _isReward;
        private bool _isClosed;

        public override void Show(Action<bool> onDone = null)
        {
            base.Show(onDone);
            _onShowedCallback = onDone;
            _isReward = false;
            _isClosed = false;
        }

        public override void OnReward()
        {
            base.OnReward();
            _isReward = true;
            UnityThread.ExecuteWhen(() =>
            {
                _onShowedCallback?.Invoke(_isReward);
                _onShowedCallback = null;
            }, () => _isClosed);
        }

        public override void OnClose()
        {
            //_onShowedCallback?.Invoke(isReward);
            //_onShowedCallback = null;
            base.OnClose();
            _isClosed = true;
        }
    }

    public class ADInterstitialSource : ADSource
    {
        public override ADLocationType Type => ADLocationType.Interstitial;

        private Action<bool> _onShowedCallback;

        public override void Show(Action<bool> onDone = null)
        {
            base.Show(onDone);
            _onShowedCallback = onDone;
        }

        public override void OnClose()
        {
            _onShowedCallback?.Invoke(true);
            _onShowedCallback = null;
            base.OnClose();
        }
    }

    public abstract class ADChannelAutoLoadAble<TLocationBanner, TLocationInterstitial, TLocationRewardedVideo> :
        ADChannelBase<TLocationBanner, TLocationInterstitial, TLocationRewardedVideo>
        where TLocationBanner : ADLocationBase
        where TLocationInterstitial : ADLocationBase
        where TLocationRewardedVideo : ADLocationBase
    {
        class AutoLoaderSource
        {
            public ADSource Source { get; private set; }

            public bool NeedLoad => Source.SourceState == SourceState.Unload ||
                                    Source.SourceState != SourceState.Ready && OverTime;


            float timer = 0;

            public AutoLoaderSource(ADSource source)
            {
                Source = source;
                Source.OnLoadRequest += () => SetTimer(20);
                Source.OnLoaded += b =>
                {
                    if (!b) SetTimer(5);
                };
                Source.OnShowRequest += () => SetTimer(10);
            }

            public void Load(Action<bool> callBack)
            {
                Source.Load(callBack);
            }

            public void SetTimer(float time)
            {
                timer = Time.time + time;
            }

            public bool OverTime => Time.time > timer;

        }

        private readonly Dictionary<string, ADSource> _allSource = new Dictionary<string, ADSource>();
        private readonly List<AutoLoaderSource> _autoLoadSources = new List<AutoLoaderSource>();
        private readonly Queue<AutoLoaderSource> _waiting = new Queue<AutoLoaderSource>();
        private AutoLoaderSource _currentSources;

        public ADSource GetSource(string id)
        {
            if (_allSource.TryGetValue(id, out var source)) return source;
            return null;
        }

        public virtual void Update()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable) return;
            foreach (var s in _autoLoadSources)
            {
                if (s != _currentSources && s.NeedLoad && !_waiting.Contains(s))
                {
                    _waiting.Enqueue(s);
                }
            }

            if (_currentSources == null && _waiting.Count > 0) _currentSources = _waiting.Dequeue();
            if (_currentSources != null)
            {
                if (_currentSources.NeedLoad)
                {
                    var s = _currentSources;
                    _currentSources.Load((b) =>
                    {
                        if (s == _currentSources) // 防止回调掉多次 导致当前source被置空
                            _currentSources = null;
                    });
                }
                else if (_currentSources.Source.SourceState == SourceState.Loading && _currentSources.OverTime)
                {
                    _currentSources = null;
                }
                else if (_currentSources.Source.SourceState == SourceState.Loading && _currentSources.OverTime)
                {
                    _currentSources = null;
                }
            }
        }

        public void AddAutoLoadSources()
        {
            foreach (var list in RewardedVideoList)
            {
                foreach (var source in list.Sources)
                {
                    var ads = source as ADSource;
                    if (ads == null) continue;
                    _autoLoadSources.Add(new AutoLoaderSource(ads));
                    _allSource[ads.UnitId] = ads;
                }

            }

            foreach (var list in InterstitialList)
            {
                foreach (var source in list.Sources)
                {
                    var ads = source as ADSource;
                    if (ads == null) continue;
                    _autoLoadSources.Add(new AutoLoaderSource(ads));
                    _allSource[ads.UnitId] = ads;
                }

            }
        }

        public override void OnInit(params object[] args)
        {
            base.OnInit(args);

            AddAutoLoadSources();

            var obj = new GameObject("ADChannelAutoLoadAble");
            obj.AddComponent<UpdateListener>().action = Update;
            obj.hideFlags = HideFlags.HideInHierarchy;
            Object.DontDestroyOnLoad(obj);
        }

        public override void Load()
        {
            // 只加载banner 其他不做任何处理 走自动加载
            foreach (var b in BannerList) b.Load();
        }
    }

    public class UpdateListener : MonoBehaviour
    {
        public Action action;

        public void Update()
        {
            action?.Invoke();
        }
    }
}