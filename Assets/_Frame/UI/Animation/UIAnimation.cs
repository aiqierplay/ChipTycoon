using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.TweenPro;
using Aya.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using PlayMode = Aya.TweenPro.PlayMode;

[Serializable]
// [HideMonoScript]
public class UIAnimation : EntityBase
{
    public List<UITweenPlayerHandler> AnimationList;

    [Button("Refresh Cache")]
    public void RefreshCache()
    {
        var handlerList = GetComponentsInChildren<UITweenPlayerHandler>(true).ToList();
        AnimationList = handlerList.SortAsc(handler => handler.Control);
        foreach (var handler in AnimationList)
        {
            var tweenPlayer = handler.TweenPlayer;
            tweenPlayer.Animation.SetPrepareSample(PrepareSampleMode.None);
            tweenPlayer.Animation.SetTimeMode(TimeMode.UnScaled);
        }
    }

    #region Cache

    [NonSerialized] public bool IsCached;
    [NonSerialized] public List<UITweenPlayerHandler> ControlList;
    [NonSerialized] public List<UITweenPlayerHandler> ShowList;
    [NonSerialized] public List<UITweenPlayerHandler> HideList;
    [NonSerialized] public List<UITweenPlayerHandler> ShowHideList;

    [NonSerialized] public List<UITweenPlayerHandler> TriggerList;

    public void Cache()
    {
        if (IsCached) return;
        ControlList = AnimationList.FindAll(t => t.Control != UIAnimationControlMode.None);
        ShowList = AnimationList.FindAll(t => t.Control == UIAnimationControlMode.Show);
        HideList = AnimationList.FindAll(t => t.Control == UIAnimationControlMode.Hide);
        ShowHideList = AnimationList.FindAll(t => t.Control == UIAnimationControlMode.ShowHide);
        TriggerList = AnimationList.FindAll(t => t.Trigger != UIAnimationTriggerMode.None);
        IsCached = true;
    }

    #endregion

    [NonSerialized] public float InTime;
    [NonSerialized] public float OutTime;
    [NonSerialized] public bool IsAnimating;
    [NonSerialized] public Coroutine AnimationCoroutine;

    protected override void Awake()
    {
        base.Awake();
        Cache();
        InitTriggerEvent();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        for (var i = 0; i < ShowList.Count; i++)
        {
            var data = ShowList[i];
            data.Sample(0f);
        }

        for (var i = 0; i < ShowHideList.Count; i++)
        {
            var data = ShowHideList[i];
            data.Sample(0f);
        }
    }

    public void Show(Action onDone = null)
    {
        Cache();
        InTime = 0f;
        ShowList.ForEach(tween =>
        {
            tween.Restart();
            if (tween.Duration > InTime) InTime = tween.Duration;
        });
        ShowHideList.ForEach(tween =>
        {
            tween.Restart();
            if (tween.Duration > InTime) InTime = tween.Duration;
        });

        IsAnimating = true;
        if (AnimationCoroutine != null) StopCoroutine(AnimationCoroutine);
        AnimationCoroutine = this.ExecuteDelay(() =>
        {
            IsAnimating = false;
            AnimationCoroutine = null;
            onDone?.Invoke();
        }, InTime, false);
    }

    public void Hide(Action onDone = null)
    {
        Cache();
        OutTime = 0f;
        HideList.ForEach(tween =>
        {
            tween.Restart();
            if (tween.Duration > OutTime) OutTime = tween.Duration;
        });
        ShowHideList.ForEach(tween =>
        {
            tween.Stop();
            tween.Play(false);
            if (tween.Duration > OutTime) OutTime = tween.Duration;
        });

        IsAnimating = true;
        if (AnimationCoroutine != null) StopCoroutine(AnimationCoroutine);

        if (gameObject.activeInHierarchy)
        {
            AnimationCoroutine = this.ExecuteDelay(() =>
            {
                IsAnimating = false;
                AnimationCoroutine = null;
                onDone?.Invoke();
            }, OutTime, false);
        }
        else
        {
            IsAnimating = false;
            AnimationCoroutine = null;
            onDone?.Invoke();
        }
    }

    public void Pause()
    {
        ControlList.ForEach(tween => tween.Pause());
        OutTime = 0f;
        InTime = 0f;
        IsAnimating = false;
    }

    public void InitTriggerEvent()
    {
        TriggerList.ForEach(tween =>
        {
            var listener = UIEventListener.Get(tween.GameObject);
            switch (tween.Trigger)
            {
                case UIAnimationTriggerMode.Enter:
                    listener.onEnter += (go, ev) => { tween.Restart(); };
                    break;
                case UIAnimationTriggerMode.Leave:
                    listener.onExit += (go, ev) => { tween.Restart(); };
                    break;
                case UIAnimationTriggerMode.Hover:
                    listener.onEnter += (go, ev) =>
                    {
                        tween.TweenPlayer.Animation.PlayMode = PlayMode.PingPong;
                        tween.Restart();
                    };
                    listener.onExit += (go, ev) =>
                    {
                        tween.TweenPlayer.Animation.PlayMode = PlayMode.Once;
                        tween.Play(false);
                    };
                    break;
                case UIAnimationTriggerMode.Down:
                    listener.onDown += (go, ev) => { tween.Restart(); };
                    break;
                case UIAnimationTriggerMode.Up:
                    listener.onUp += (go, ev) => { tween.Restart(); };
                    break;
                case UIAnimationTriggerMode.Click:
                    listener.onClick += (go, ev) => { tween.Restart(); };
                    break;
                case UIAnimationTriggerMode.DoubleClick:
                    listener.onDoubleClick += (go, ev) => { tween.Restart(); };
                    break;
            }
        });
    }
}