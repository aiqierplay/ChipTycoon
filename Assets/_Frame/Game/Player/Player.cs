using System;
using Aya.Extension;
using UnityEngine;

public enum PlayerGroup
{
    Player = 0,
    Enemy = 1,
}

public partial class Player : PlayerBase
{
    [NonSerialized] public PlayerData Data;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        InitAllComponent();

        Play(AnimatorDefine.Idle, true);
        Trans.position = Vector3.zero;
        Trans.forward = Vector3.forward;
    }
    public void Start()
    {
        
    }

    public void FixedUpdate()
    {


    }

    public void LateUpdate()
    {

    }

    public void Win()
    {
        if (!IsGaming) return;
        Stop(true);
        Play(AnimatorDefine.Win);
        if (IsPlayer)
        {
            App.Enter<GameWin>();
        }
    }

    public void Lose()
    {
        if (!IsGaming) return;
        Stop(false);
        Play(AnimatorDefine.Lose);
        if (IsPlayer)
        {
            App.Enter<GameLose>();
        }
    }

    public void Die()
    {
        if (!IsGaming) return;
        State.Hp = 0;
        Stop(false);
        Play(AnimatorDefine.Die);
        if (IsPlayer)
        {
            this.ExecuteDelay(() =>
            {
                App.Enter<GameLose>();
            }, GeneralSetting.Ins.LoseWaitDuration);
        }
    }

    public void Stop(bool win)
    {
        Move.DisableMove();
        Buff.StopAll();
        if (!win)
        {
            State.RestoreSave();
        }
    }
}
