using System;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public enum UIAnimationControlMode
{
    None = 0,
    Show = 1,
    Hide = 2,
    ShowHide = 3,
}

public enum UIAnimationTriggerMode
{
    None = 0,
    Enter = 1,
    Leave = 2,
    Hover = 3,
    Down = 4,
    Up = 5,
    Click = 6,
    DoubleClick = 7
}

[RequireComponent(typeof(UTweenPlayer))]
[HideMonoScript]
[Serializable]
[LabelWidth(85)]
public class UITweenPlayerHandler : EntityBase
{
    public UTweenPlayer TweenPlayer;
    public GameObject Target;

    public UIAnimationControlMode Control;
    public UIAnimationTriggerMode Trigger;

    public float Duration => TweenPlayer.Animation.Delay + TweenPlayer.Animation.Duration;

    public GameObject GameObject => Target != null ? Target : TweenPlayer.gameObject;

    public void Init()
    {
        TweenPlayer.Animation.AutoPlay = AutoPlayMode.None;
        TweenPlayer.Animation.PrepareSampleMode = PrepareSampleMode.None;
    }

    public void Play(bool forward = true)
    {
        TweenPlayer.Play(forward);
    }

    public void Pause()
    {
        TweenPlayer.Animation.Pause();
    }

    public void Stop()
    {
        TweenPlayer.Stop();
    }

    public void Restart()
    {
        TweenPlayer.Stop();
        TweenPlayer.Play();
    }

    public void Sample(float value)
    {
        TweenPlayer.Sample(value);
    }

    public void Reset()
    {
        TweenPlayer = GetComponent<UTweenPlayer>();
        Target = gameObject;
    }
}