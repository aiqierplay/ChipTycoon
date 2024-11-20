using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Aya.Extension;

public enum GameEffectState
{
    None = 0,
    Playing = 1
}

public class GameEffect : EntityBase
{
    public const bool ShowIndexLabels = false;

    public EntityBase OtherTarget;

    [TableList(ShowIndexLabels = ShowIndexLabels)] public List<GameEffectAnimator> AnimatorList;
    [TableList(ShowIndexLabels = ShowIndexLabels)] public List<GameEffectFx> FxList;
    [TableList(ShowIndexLabels = ShowIndexLabels)] public List<GameEffectActive> ActiveList;
    [TableList(ShowIndexLabels = ShowIndexLabels)] public List<GameEffectTweenPlayer> TweenPlayerList;
    [TableList(ShowIndexLabels = ShowIndexLabels)] public List<GameEffectAudio> AudioList;
    [TableList(ShowIndexLabels = ShowIndexLabels)] public List<GameEffectVibration> VibrationList;
    [TableList(ShowIndexLabels = ShowIndexLabels)] public List<GameEffectSubEffect> SubEffectList;
    [TableList(ShowIndexLabels = ShowIndexLabels)] public List<GameEffectCallback> CallbackList;

    [NonSerialized] public List<GameEffectBase> RuntimeEffectList = new List<GameEffectBase>();
    [NonSerialized] public float Duration;
    [NonSerialized] public GameEffectState EffectState;

    protected override void Awake()
    {
        base.Awake();
        AutoCacheSubClassList(RuntimeEffectList);
        Duration = RuntimeEffectList.ToList(effect => effect.GetDuration()).Max();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public virtual void Init()
    {
        EffectState = GameEffectState.None;
        RuntimeEffectList.ForEach(effect => effect.Init(this, OtherTarget));
    }

    public virtual void Play()
    {
        Play(null);
    }

    public virtual void Play(Action onDone)
    {
        if (EffectState == GameEffectState.Playing) return;
        RuntimeEffectList.ForEach(effect => effect.Play(this, OtherTarget));
        this.ExecuteDelay(() =>
        {
            if (onDone != null) onDone.Invoke();
            EffectState = GameEffectState.None;
        }, Duration);
    }
}