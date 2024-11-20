using System;
using System.Collections.Generic;
using Aya.Particle;
using Aya.Pool;
using UnityEngine;

public abstract partial class EntityBase
{
    public EntityPool AppPool => PoolManager.Ins?["App"];
    public EntityPool GamePool => PoolManager.Ins?["Game"];
    public EntityPool UIPool => PoolManager.Ins?["UI"];
    public EntityPool EffectPool => ParticleSpawner.EntityPool;

    public AppManager App => AppManager.Ins;
    public GameState State => App.State;
    public LevelManager Level => LevelManager.Ins;
    public TagManager Tag => TagManager.Ins;
    public LayerManager Layer => LayerManager.Ins;
    public UIController UI => UIController.Ins;
    public ConfigManager Config => ConfigManager.Ins;
    public UpgradeManager Upgrade => UpgradeManager.Ins;
    public SaveManager Save => SaveManager.Ins;
    public Level CurrentLevel => Level.Level;
    public LevelSaveInfo CurrentLevelInfo => CurrentLevel.Info;

    public Player Player => App.Player;
    public List<Player> PlayerList => App.PlayerList;

    public bool IsGaming => App.GamePhase == GamePhaseType.Gaming;

    [NonSerialized] public float SelfScale;
    public static float GlobalScale = 1f;
    public virtual float DeltaTime => Time.deltaTime * SelfScale * GlobalScale;
    public virtual float UnscaledDeltaTime => Time.unscaledDeltaTime * SelfScale * GlobalScale;
    public virtual float FixedDeltaTime => Time.fixedDeltaTime * SelfScale * GlobalScale;
}
