using System;
using Aya.Extension;
using Aya.Timers;
using Sirenix.OdinInspector;

public abstract class UIBase : EntityBase
{
    [FoldoutGroup("UI")] public bool RefreshOnEnable = false;

    [FoldoutGroup("UI")]
    public bool AutoRefresh;
    [FoldoutGroup("UI")]

    [ShowIf(nameof(AutoRefresh))]
    [SuffixLabel("sec")]
    public float AutoRefreshInterval = 1f;

    [NonSerialized] public TimerEvent RefreshTimer;

    [NonSerialized] public bool Interactable;
    [GetComponentInChildren, NonSerialized] public UIAnimation UIAnimation;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (RefreshOnEnable)
        {
            this.ExecuteEndOfFrame(() => Refresh());
        }

        if (AutoRefresh)
        {
            var timerId = Timer.Ins.Interval(() => Refresh(), AutoRefreshInterval, false);
            RefreshTimer = Timer.Ins.GetTimerEvent(timerId);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (AutoRefresh && RefreshTimer != null)
        {
            Timer.Ins.Stop(RefreshTimer.Key);
            RefreshTimer = null;
        }
    }

    public virtual void Init()
    {
        Refresh();
    }

    public virtual void Refresh(bool immediately = false)
    {
        Interactable = CanInteractable();
        SetInteractable(Interactable);
    }

    public virtual bool CanInteractable()
    {
        return true;
    }

    public virtual void SetInteractable(bool interactable)
    {

    }

    public virtual void Show(params object[] args)
    {
        gameObject.SetActive(true);
        if (UIAnimation != null)
        {
            UIAnimation.Show(OnShow);
        }
        else
        {
            OnShow();
        }
    }

    // 当UI打开后，如果有动画，则在动画播放完成之后
    public virtual void OnShow()
    {

    }

    public virtual void Hide()
    {
        if (UIAnimation != null)
        {
            UIAnimation.Hide(() =>
            {
                gameObject.SetActive(false);
                OnHide();
            });
        }
        else
        {
            gameObject.SetActive(false);
            OnHide();
        }
    }

    // 当UI关闭，如果有动画，则在动画播放完成之后
    public virtual void OnHide()
    {

    }
}