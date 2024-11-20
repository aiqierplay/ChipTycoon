using System;

[Flags]
public enum GamePhaseType
{
    None = -1,
    Ready = 1,
    Start = 2,
    Gaming = 4,
    Pause = 8,
    Win = 16,
    Lose = 32,
    Endless = 64,
    Reward = 128,
}

public abstract class GamePhaseHandler : EntityBase<GamePhaseHandler>
{
    public abstract GamePhaseType Type { get; }

    public virtual void Enter(params object[] args)
    {

    }

    public virtual void UpdateImpl()
    {

    }

    public virtual void Exit()
    {

    }
}