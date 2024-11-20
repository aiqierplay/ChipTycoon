using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TweenAnimation
    {
        [NonSerialized] public int InstanceID;

        public TweenAnimation()
        {
            InstanceID = UTween.InstanceIDCounter;
            UTween.InstanceIDCounter++;
            Reset();
        }

        #region Property

        public bool EnableIdentifier = false;

        public object Owner;

        [Tooltip("Identifier :\n" +
                 " Unique ID used to identify a specific animation")]
        public string Identifier;

        [Min(1e-6f),
         Tooltip("Duration :\n" +
                 " The duration of the animation (unit: seconds.)")]
        public float Duration;

        [Min(0),
         Tooltip("Delay :\n" +
                 " How long to wait before the animation starts playing (unit: seconds.)")]
        public float Delay;

        [Tooltip("Backward :\n" +
                 " Play animation in reverse.")]
        public bool Backward;

        [Tooltip("Play Mode :\n" +
                 " 1. Once : play only once.\n" +
                 " 2. Loop : Play the specified number of times in a loop,  0 means unlimited..\n" +
                 " 3. PingPong : Play back and forth the specified number of times, 0 means unlimited.")]
        public PlayMode PlayMode;

        [Min(0),
         Tooltip("PlayCount :\n" +
                 " Specify the number of times to play, 0 is unlimited, it only takes effect when the Play Mode is Loop or PingPong.")]
        public int PlayCount;

        [Tooltip("Auto Play Mode :\n" +
                 " Specify when the animation will autoplay.")]
        public AutoPlayMode AutoPlay;

        [Tooltip("Update Mode :\n" +
                 " Specifies the trigger timing for animation state updates.")]
        public UpdateMode UpdateMode;

        [Min(0),
         Tooltip("Interval 1:\n" +
                 " The interval time for the animation to repeat, it only takes effect when the Play Mode is Loop or PingPong. (unit: seconds)")]
        public float Interval;

        [Min(0),
         Tooltip("Interval 2:\n" +
                 " The interval time for the animation to repeat, it only takes effect when the Play Mode is PingPong. (unit: seconds)")]
        public float Interval2;

        [Tooltip("Time Mode :\n" +
                 " Delta time mode for animation update calculation.\n" +
                 " 1. Normal : Time.deltaTime\n" +
                 " 2. UnScaled : Time.unscaledDeltaTime\n" +
                 " 3. Smooth : Time.smoothDeltaTime")]
        public TimeMode TimeMode;

        [Min(1e-6f),
         Tooltip("Self Time Scale :\n" +
                 " Self time scaling, which will be multiplied by this value based on the animation update delta time to achieve independent speed change.")]
        public float SelfScale;

        [Tooltip("Prepare Sample Mode :\n" +
                 " Specifies the timing of the animation pre sampling the 0 time point.")]
        public PrepareSampleMode PrepareSampleMode;

        [Tooltip("Auto Kill :\n" +
                 " Animation components will be automatically destroyed after playback ends.")]
        public bool AutoKill;

        [Tooltip("Speed Based :\n" +
                 " Duration will become speed, animation will automatically calculate total play time based on speed.")]
        public bool SpeedBased;

        [SerializeReference]
        public List<Tweener> TweenerList = new List<Tweener>();

        public Func<bool> StopCondition;

        public OnStartEvent OnStart = new OnStartEvent();
        public OnPlayEvent OnPlay = new OnPlayEvent();
        public OnLoopStartEvent OnLoopStart = new OnLoopStartEvent();
        public OnLoopEndEvent OnLoopEnd = new OnLoopEndEvent();
        public OnUpdateEvent OnUpdate = new OnUpdateEvent();
        public OnPauseEvent OnPause = new OnPauseEvent();
        public OnResumeEvent OnResume = new OnResumeEvent();
        public OnStopEvent OnStop = new OnStopEvent();
        public OnCompleteEvent OnComplete = new OnCompleteEvent();

        [SerializeField] internal bool FoldOut = true;
        [SerializeField] internal bool FoldOutCallback = false;
        [SerializeField] internal CallbackType CallbackType = CallbackType.OnStart;

        #endregion

        #region State Property

        [NonSerialized] public UTweenPlayer TweenPlayer;
        [NonSerialized] public TweenControlMode ControlMode;
        [NonSerialized] public PlayState State;
        [NonSerialized] public bool Forward;
        [NonSerialized] public bool StartForward;
        [NonSerialized] public int LoopCounter;
        [NonSerialized] public int FrameCounter;
        [NonSerialized] public bool IsDelaying;
        [NonSerialized] public bool IsInterval;
        [NonSerialized] public float CurrentInterval;

        public bool SingleMode => TweenerList.Count == 1;
        public Tweener Tweener => TweenerList.Count > 0 ? TweenerList[0] : default;
        public Tweener FirstTweener => Tweener;
        public Tweener LastTweener => TweenerList.Count > 0 ? TweenerList[TweenerList.Count - 1] : default;
        public bool IsSubAnimation;

        public bool IsPlaying => State == PlayState.Playing;

        public bool IsCompleted => State == PlayState.Completed;
        public bool IsInProgress => State == PlayState.Playing || State == PlayState.Paused;

        public float Progress
        {
            get => PlayTimer / Duration;
            internal set => PlayTimer = Mathf.Clamp01(value) * Duration;
        }

        public float NormalizedProgress
        {
            get
            {
                if (Application.isPlaying) return RuntimeNormalizedProgress;
#if UNITY_EDITOR
                if (!Application.isPlaying) return EditorNormalizedProgress;
#endif
                return default;
            }
            internal set
            {
                if (Application.isPlaying) RuntimeNormalizedProgress = value;
#if UNITY_EDITOR
                if (!Application.isPlaying) EditorNormalizedProgress = value;
#endif
            }
        }

        #endregion

        #region Internal Property

        internal bool IsInitialized;
        internal bool IsStateCached;
        internal bool IsPreSampled;
        internal float InitNormalizedProgress;

        internal bool CacheApplicationIsPlaying;
        internal bool CacheSingleMode;
        internal int CacheTweenerListCount;
        internal bool CacheSupportGizmos;
        internal Tweener CacheSingleTweener;

        internal float DelayTimer;
        internal float PlayTimer;
        internal float IntervalTimer;
        internal float RuntimeDuration;
        internal float RuntimeNormalizedProgress;

        #endregion

        #region Mono Behaviour

        public virtual void Awake()
        {
            if (PrepareSampleMode == PrepareSampleMode.Awake)
            {
                ResetCacheState();
                PreSample();
                Sample(0f);
            }

            if (AutoPlay == AutoPlayMode.Awake) Play();
        }

        public virtual void OnEnable()
        {
            if (PrepareSampleMode == PrepareSampleMode.Enable)
            {
                ResetCacheState();
                PreSample();
                Sample(0f);
            }

            if (AutoPlay == AutoPlayMode.Enable) Play();
        }

        public virtual void Start()
        {
            if (PrepareSampleMode == PrepareSampleMode.Start)
            {
                ResetCacheState();
                PreSample();
                Sample(0f);
            }

            if (AutoPlay == AutoPlayMode.Start) Play();
        }

        public virtual void OnDisable()
        {
            if (IsPlaying)
            {
                Stop();
            }
        }

        #endregion

        #region Play / Pasue / Resume / Stop

        public TweenAnimation Play(bool forward = true)
        {
            return Play(0f, forward);
        }

        public TweenAnimation Play(float initNormalizedProgress, bool forward = true)
        {
            if (State == PlayState.Playing) return this;
            if (State != PlayState.Paused)
            {
                ResetCacheState();
                InitNormalizedProgress = initNormalizedProgress;
                StartForward = forward && !Backward;

                if (Application.isPlaying)
                {
                    if (UTweenManager.Ins != null)
                    {
                        UTweenManager.Ins.AddTweenAnimation(this);
                    }
                }
                else
                {
#if UNITY_EDITOR
                    TweenPlayer.PreviewStart();
                    RecordObject();
#endif
                }
            }

            State = PlayState.Playing;
            OnPlay.Invoke();
            return this;
        }

        public void AfterAddToManager()
        {
            if (Delay < 1e-6f)
            {
                if (!IsInitialized)
                {
                    Initialize();
                }

                Sample(StartForward ? 0f : 1f);
            }
        }

        public TweenAnimation Pause()
        {
            if (State != PlayState.Playing) return this;
            State = PlayState.Paused;
            OnPause.Invoke();
            return this;
        }

        public TweenAnimation Resume()
        {
            if (State != PlayState.Paused) return this;
            State = PlayState.Playing;
            OnResume.Invoke();
            return this;
        }

        public TweenAnimation Stop()
        {
            if (State != PlayState.Completed)
            {
                State = PlayState.Stopped;
            }

            RuntimeNormalizedProgress = 0f;
#if UNITY_EDITOR
            EditorNormalizedProgress = 0f;
#endif
            OnStop.Invoke();

            if (Application.isPlaying)
            {
                UTweenManager.Ins?.RemoveTweenAnimation(this);
            }
            else
            {
#if UNITY_EDITOR
                RestoreObject();
                TweenPlayer.PreviewEnd();
#endif
            }

            return this;
        }

        public TweenAnimation PlayForward()
        {
            Play(true);
            return this;
        }

        public TweenAnimation PlayBackward()
        {
            Play(false);
            return this;
        }

        #endregion

        #region // TODO Play AsCoroutine

        // public Coroutine PlayAsCoroutine(MonoBehaviour monoBehaviour, bool forward = true)
        // {
        //     if (State != PlayState.Paused)
        //     {
        //         IsInitialized = false;
        //         IsStateCached = false;
        //         StartForward = forward && !Backward;
        //         foreach (var tweener in TweenerList)
        //         {
        //             tweener.IsPrepared = false;
        //         }
        //     }
        //
        //     State = PlayState.Playing;
        //     OnPlay.Invoke();
        //
        //     var coroutine = monoBehaviour.StartCoroutine(TweenAnimationCoroutine());
        //     return coroutine;
        // }
        //
        // internal IEnumerator TweenAnimationCoroutine()
        // {
        //     while (IsInProgress)
        //     {
        //         var scaledDeltaTime = Time.fixedDeltaTime;
        //         var unscaledDeltaTime = Time.fixedUnscaledDeltaTime;
        //         var smoothDeltaTime = Time.fixedDeltaTime;
        //         UTweenManager.Ins.UpdateImpl(this, scaledDeltaTime, unscaledDeltaTime, smoothDeltaTime);
        //         yield return null;
        //     }
        // }

        #endregion

        #region Add / Remove Tweener

        public void AddTweener(Tweener tweener)
        {
            if (TweenerList.Contains(tweener)) return;
            if (tweener.Animation != null)
            {
                tweener.Animation.RemoveTweener(tweener);
                tweener.Animation = null;
            }

            TweenerList.Add(tweener);
            tweener.Animation = this;
        }

        public void RemoveTweener(Tweener tweener)
        {
            if (!TweenerList.Contains(tweener)) return;
            TweenerList.Remove(tweener);
            tweener.Animation = null;
        }

        #endregion

        #region Initialize / Update / Sample

        internal void CacheState(bool forceCache = false)
        {
            if (IsStateCached && !forceCache) return;
            CacheApplicationIsPlaying = Application.isPlaying;
            CacheSingleMode = SingleMode;
            CacheTweenerListCount = TweenerList.Count;
            CacheSingleTweener = CacheSingleMode ? TweenerList[0] : null;
            CacheSupportGizmos = false;
            for (var i = 0; i < CacheTweenerListCount; i++)
            {
                var tweener = TweenerList[i];
                if (!tweener.SupportGizmos) continue;
                CacheSupportGizmos = true;
                break;
            }

            IsStateCached = true;
        }

        internal void Initialize(bool isPreview = false)
        {
            if (IsInitialized) return;
            CacheState();

            FrameCounter = 0;
            if (SingleMode && SpeedBased)
            {
                RuntimeDuration = Tweener.GetSpeedBasedDuration();
            }
            else
            {
                RuntimeDuration = Duration;
            }

            if (InitNormalizedProgress > 0f)
            {
                PlayTimer = RuntimeDuration * InitNormalizedProgress;
                DelayTimer = Delay;
            }
            else
            {
                PlayTimer = 0f;
                DelayTimer = 0f;
            }

            Forward = StartForward;
            IsDelaying = false;
            IsInterval = false;
            IntervalTimer = 0f;
            LoopCounter = 0;

            PreSample();

            if (!isPreview)
            {
                if (Delay > 0f)
                {
                    IsDelaying = true;
                }
            }

            IsInitialized = true;
        }

        public void Update(float deltaTime)
        {
            deltaTime *= SelfScale;
            if (!IsInitialized)
            {
                Initialize();
            }

            if (StopCondition != null && StopCondition())
            {
                Stop();
                return;
            }

            if (FrameCounter == 0)
            {
                OnStart.Invoke();
                if (PlayMode != PlayMode.Once)
                {
                    LoopStart();
                    OnLoopStart.Invoke();
                }
            }

            if (IsDelaying)
            {
                DelayTimer += deltaTime;
                PlayTimer = 0f;
                if (DelayTimer >= Delay)
                {
                    IsDelaying = false;
                }

                return;
            }

            if (IsInterval)
            {
                IntervalTimer += deltaTime;
                if (IntervalTimer >= CurrentInterval)
                {
                    IsInterval = false;
                    LoopStart();
                    OnLoopStart.Invoke();
                }

                return;
            }

            if (State == PlayState.Playing)
            {
                PlayTimer += deltaTime;
                FrameCounter++;
            }

            if (PlayTimer < RuntimeDuration)
            {
                RuntimeNormalizedProgress = Forward ? PlayTimer / RuntimeDuration : (RuntimeDuration - PlayTimer) / RuntimeDuration;
                Sample(RuntimeNormalizedProgress);
                OnUpdate.Invoke();
            }
            else
            {
                var tweenerCount = TweenerList.Count;
                for (var i = 0; i < tweenerCount; i++)
                {
                    var tweener = TweenerList[i];
                    if (!tweener.Active) continue;
                    tweener.IsCurrentLoopFinished = false;
                }

                if (PlayMode == PlayMode.Once)
                {
                    RuntimeNormalizedProgress = Forward ? 1f : 0f;
                    Sample(RuntimeNormalizedProgress);
                    Complete();
                    Stop();
                }
                else if (PlayMode == PlayMode.Loop)
                {
                    LoopCounter++;
                    PlayTimer = 0f;
                    RuntimeNormalizedProgress = Forward ? 1f : 0f;
                    Sample(RuntimeNormalizedProgress);
                    if (LoopCounter >= PlayCount && PlayCount > 0)
                    {
                        LoopEnd();
                        OnLoopEnd.Invoke();
                        Complete();
                        Stop();
                    }
                    else
                    {
                        if (Interval > 0)
                        {
                            IsInterval = true;
                            IntervalTimer = 0f;
                            CurrentInterval = Interval;
                        }
                        else
                        {
                            LoopStart();
                            OnLoopStart.Invoke();
                            for (var i = 0; i < tweenerCount; i++)
                            {
                                var tweener = TweenerList[i];
                                if (!tweener.Active) continue;
                                tweener.IsCurrentLoopFinished = false;
                            }
                        }
                    }
                }
                else if (PlayMode == PlayMode.PingPong)
                {
                    PlayTimer = 0f;
                    RuntimeNormalizedProgress = Forward ? 1f : 0f;
                    Sample(RuntimeNormalizedProgress);
                    Forward = !Forward;
                    if (Forward == StartForward) LoopCounter++;
                    if (LoopCounter >= PlayCount && PlayCount > 0)
                    {
                        LoopEnd();
                        OnLoopEnd.Invoke();
                        Complete();
                        Stop();
                    }
                    else
                    {
                        CurrentInterval = Forward == StartForward ? Interval : Interval2;
                        if (CurrentInterval > 0)
                        {
                            IsInterval = true;
                            IntervalTimer = 0f;
                        }
                        else
                        {
                            if (Forward == StartForward)
                            {
                                LoopStart();
                                OnLoopStart.Invoke();
                            }

                            for (var i = 0; i < tweenerCount; i++)
                            {
                                var tweener = TweenerList[i];
                                if (!tweener.Active) continue;
                                tweener.IsCurrentLoopFinished = false;
                            }
                        }
                    }
                }
            }
        }

        public void Sample(float normalizedDuration)
        {
            try
            {
                if (!IsInitialized)
                {
                    Initialize();
                }

#if UNITY_EDITOR
                if (!CacheApplicationIsPlaying)
                {
                    if (Mode == TweenEditorMode.Component && !PreviewSampled)
                    {
                        PreviewSampled = true;
                        RecordObject();
                    }
                }
#endif

                if (!IsPreSampled)
                {
                    PreSample();
                }

                if (CacheSingleMode)
                {
                    if (CacheSingleTweener.Animation == null) CacheSingleTweener.Animation = this;
                    if (CacheSingleTweener.Active)
                    {
                        var factor = CacheSingleTweener.GetFactor(normalizedDuration, out var valid);
                        if (valid)
                        {
                            CacheSingleTweener.Sample(factor);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < CacheTweenerListCount; i++)
                    {
                        var tweener = TweenerList[i];
                        if (!tweener.Active) continue;
                        if (tweener.Animation == null) tweener.Animation = this;
                        var factor = tweener.GetFactor(normalizedDuration, out var valid);
                        if (!valid) continue;
                        tweener.Sample(factor);
                    }
                }

#if UNITY_EDITOR
                if (!CacheApplicationIsPlaying) SetDirty();
#endif
            }
            catch (Exception exception)
            {
                UTweenCallback.OnException(exception);
                UTweenManager.Ins?.RemoveTweenAnimation(this);
            }
        }

        #region PreSample / Loop

        internal void PreSample()
        {
            if (IsPreSampled) return;
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                tweener.Animation = this;
                if (!tweener.Active) continue;
                tweener.PrepareSample();
            }

            IsPreSampled = true;
        }

        internal void StopSample()
        {
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                if (!tweener.Active) continue;
                tweener.StopSample();
            }
        }

        internal void LoopStart()
        {
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                if (!tweener.Active) continue;
                tweener.LoopStart();
            }
        }

        internal void LoopEnd()
        {
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                if (!tweener.Active) continue;
                tweener.LoopEnd();
            }
        }

        #endregion

        internal void Complete()
        {
            State = PlayState.Completed;
            OnComplete.Invoke();
        }

        #endregion

        #region Record / Restore / SetDirty

        // Editor Only
        public void SetDirty()
        {
            if (Application.isPlaying) return;
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                if (!tweener.Active) continue;
                tweener.SetDirty();
            }
        }

        // Editor Only
        public void RecordObject()
        {
            if (Application.isPlaying) return;
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                if (!tweener.Active) continue;
                try
                {
                    tweener.RecordObject();
                }
                catch (Exception e)
                {
                    UTweenCallback.OnException(e);
                }
            }
        }

        // Editor Only
        public void RestoreObject()
        {
            if (Application.isPlaying) return;
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                if (!tweener.Active) continue;
                try
                {
                    tweener.RestoreObject();
                }
                catch (Exception e)
                {
                    UTweenCallback.OnException(e);
                }
            }
        }

        #endregion

        #region Reset / DeSpawn

        public void Reset()
        {
            State = PlayState.None;

            ResetCacheState();

            Owner = null;

            Duration = 1f;
            Delay = 0f;
            Backward = false;
            PlayMode = PlayMode.Once;
            PlayCount = 1;
            AutoPlay = AutoPlayMode.None;
            UpdateMode = UpdateMode.Update;
            Interval = 0f;
            Interval2 = 0f;
            TimeMode = TimeMode.Normal;
            SelfScale = 1f;
            PrepareSampleMode = PrepareSampleMode.Enable;
            AutoKill = false;
            SpeedBased = false;

            foreach (var tweener in TweenerList)
            {
                tweener.Reset();
            }

            ResetCallback();
        }

        public virtual void ResetCacheState()
        {
            IsInitialized = false;
            IsStateCached = false;
            IsPreSampled = false;
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                tweener.IsPrepared = false;
            }
        }

        public virtual void ResetCallback()
        {
            StopCondition = null;

            OnPlay.Reset();
            OnLoopStart.Reset();
            OnLoopEnd.Reset();
            OnUpdate.Reset();
            OnPause.Reset();
            OnResume.Reset();
            OnStop.Reset();
            OnComplete.Reset();
        }

        internal void DeSpawn()
        {
            StopSample();

            if (ControlMode == TweenControlMode.Component)
            {
                if (Application.isPlaying && AutoKill) Object.Destroy(TweenPlayer.gameObject);
                return;
            }

            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                UTweenPool.DeSpawn(tweener);
            }

            TweenerList.Clear();
            UTweenPool.DeSpawn(this);
        }

        #endregion

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            if (!CacheSupportGizmos) return;
            var tweenerCount = TweenerList.Count;
            for (var i = 0; i < tweenerCount; i++)
            {
                var tweener = TweenerList[i];
                if (!tweener.Active || !tweener.FoldOut) continue;
                if (!tweener.SupportGizmos) continue;
                tweener.OnDrawGizmos();
            }
        }

#endif
    }
}
