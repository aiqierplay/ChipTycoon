using Aya.Extension;
using System;

public abstract class UISingletonMaskPage : UIBase
{
    public UTweenPlayerReference TweenShow;
    public UTweenPlayerReference TweenHide;
    public float WaitDuration = 0.2f;

    public override void Init()
    {
        base.Init();
        Awake();
        gameObject.SetActive(false);
    }

    public virtual void Show(Action onShow = null)
    {
        gameObject.SetActive(true);
        TweenShow.Play();
        UI.ExecuteDelay(() =>
        {
            onShow?.Invoke();
        }, TweenShow.Value.Animation.Duration + WaitDuration);
    }

    public virtual void ShowHide(Action onShow = null, Action onHide = null)
    {
        Show(() =>
        {
            onShow?.Invoke();
            Hide(onHide);
        });
    }

    public virtual void Hide(Action onHide = null)
    {
        TweenHide.Play();
        UI.ExecuteDelay(() =>
        {
            gameObject.SetActive(false);
            onHide?.Invoke();
        }, TweenHide.Value.Animation.Duration);
    }
}
