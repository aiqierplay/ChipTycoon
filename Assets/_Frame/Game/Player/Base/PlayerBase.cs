using System;
using UnityEngine;

public abstract class PlayerBase : ComponentBase<PlayerBase>
{
    public int Index => State.Index;
    public bool IsPlayer => State.IsPlayer;
    public bool IsAI => !State.IsPlayer;
    public bool IsAlive => State.Hp > 0;
    public bool IsDie => State.Hp <= 0;
    public PlayerGroup Group => IsPlayer ? PlayerGroup.Player : PlayerGroup.Enemy;

    public Vector3 RenderPosition
    {
        get => RendererTrans.position;
        set => RendererTrans.position = value;
    }

    [GetComponent, NonSerialized] public Player Self;
    [GetComponent, NonSerialized] public PlayerControl Control;
    [GetComponent, NonSerialized] public PlayerMove Move;
    [GetComponent, NonSerialized] public PlayerBuff Buff;
    [GetComponent, NonSerialized] public new PlayerState State;
    [GetComponent, NonSerialized] public PlayerAttack Attack;
    [GetComponent, NonSerialized] public PlayerAI AI;
    [GetComponent, NonSerialized] public PlayerHealth Health;
    [GetComponent, NonSerialized] public PlayerRender Render;
    [GetComponent, NonSerialized] public PlayerFx Fx;
}
