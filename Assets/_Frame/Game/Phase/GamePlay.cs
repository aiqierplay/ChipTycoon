using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

public enum GameResult
{
    None = -1,
    Win = 1,
    Lose = 2,
}

public class GamePlay : GamePhaseHandler
{
    public override GamePhaseType Type => GamePhaseType.Gaming;
    private bool _isOver;

    public override void Enter(params object[] args)
    {
        _isOver = false;
    }

    public override void UpdateImpl()
    {
        if (_isOver) return;
        
        var result = CheckGameResult();
        if (result == GameResult.Lose)
        {
            _isOver = true;
            if (Player != null) Player.Die();
            else App.Enter<GameLose>();
        }
        else if (result == GameResult.Win)
        {
            _isOver = true;
            if (Player != null) Player.Win();
            else App.Enter<GameWin>();
        }
    }

    public virtual GameResult CheckGameResult()
    {
        if (Player != null && Player.State.Point == 0 && Player.State.PointChanged)
        {
            return GameResult.Lose;
        }

        return GameResult.None;
    }

    public override void Exit()
    {

    }
}
