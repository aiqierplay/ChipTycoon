using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Reflection;
using UnityEngine;

public class AppManager : EntityBase<AppManager>
{
    [TypeReference(typeof(GamePhaseHandler))] public TypeReference LaunchPhase;
    public Transform Handler;

    [NonSerialized] public new Player Player;
    [NonSerialized] public new List<Player> PlayerList = new List<Player>();

    [NonSerialized] public GamePhaseType GamePhase;
    [NonSerialized] public GamePhaseHandler CurrentPhase;

    [NonSerialized] public Dictionary<GamePhaseType, GamePhaseHandler> PhaseDic;
    [NonSerialized] public Dictionary<Type, GamePhaseHandler> PhaseTypeDic;
    [GetComponentInChildren, NonSerialized] public List<GamePhaseHandler> PhaseList;
    [GetComponentInChildren, NonSerialized] public new GameState State;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        Time.timeScale = 1f;
        GamePhase = GamePhaseType.None;
        CurrentPhase = null;

        PhaseDic = PhaseList.ToDictionary(p => p.Type);
        PhaseTypeDic = PhaseList.ToDictionary(p => p.GetType());

#if !SuperSonic
        OnLoadComplete();
#else
        UILoading.Ins.Show();
#endif
    }

    public virtual void OnLoadComplete()
    {
        SDKUtil.Init();
        StartApp();
        UILoading.Ins.Hide(() =>
        {
            Dispatch(GameEvent.Launch);
        });
    }

    public virtual void StartApp()
    {
        Level.LevelStart();
    }

    public void Init()
    {
        State.Init();
    }

    #region Game Phase
 
    public T Get<T>() where T : GamePhaseHandler
    {
        var type = typeof(T);
        return Get(type) as T;
    }

    public GamePhaseHandler Get(Type type)
    {
        if (PhaseTypeDic.TryGetValue(type, out var handler)) return handler;
        handler = Handler.GetOrAddComponent(type) as GamePhaseHandler;
        if (handler == null) return default;
        PhaseTypeDic.Add(type, handler);
        PhaseDic.Add(handler.Type, handler);
        return handler;
    }

    public void Enter<T>(params object[] args) where T : GamePhaseHandler
    {
        Enter(typeof(T), args);
    }

    public void Enter(Type phaseType, params object[] args)
    {
        var nextPhase = Get(phaseType);
        Enter(nextPhase, args);
    }

    public void Enter(GamePhaseType gamePhaseType, params object[] args)
    {
        var nextPhase = PhaseDic[gamePhaseType];
        Enter(nextPhase, args);
    }

    public void Enter(GamePhaseHandler nextPhase, params object[] args)
    {
        if (CurrentPhase != null) CurrentPhase.Exit();
        GamePhase = nextPhase.Type;
        CurrentPhase = nextPhase;
        nextPhase.Enter(args);
    } 

    #endregion

    public void Update()
    {
        if (CurrentPhase == null) return;
        CurrentPhase.UpdateImpl();
    }

    public void OnApplicationFocus(bool isFocusOn)
    {
        if (CurrentLevel == null) return;
        World.SaveState();
    }

    public void OnApplicationPause()
    {
        if (CurrentLevel == null) return;
        World.SaveState();
    }

    public void OnApplicationQuit()
    {
        if (CurrentLevel == null) return;
        World.SaveState();
    }
}