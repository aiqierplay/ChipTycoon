using System;
using Aya.Extension;
using Aya.Maths;
using UnityEngine;

public class Digger : EntityBase
{
    public Vector3 PosMin;
    public Vector3 PosMax;

    public Transform MoveTrans;

    public float MoveSpeed;
    public float MaxTouchDis = 10;
    public UTweenPlayerReference TweenWork;

    [NonSerialized] public Vector3 StartPos;
    [NonSerialized] public Vector3 Direction;

    public void Init()
    {
        TweenWork.Stop();
        Direction = Vector3.zero;
        Trans.ResetLocal();
        MoveTrans.ResetLocal();
    }

    public void OnTouchStart(Vector3 pos)
    {
        TweenWork.Play();
        StartPos = pos;
    }

    public void OnTouch(Vector3 pos)
    {
        Direction = pos - StartPos;
        var dis = Direction.magnitude;
        var factor = dis / MaxTouchDis;
        Direction = Direction.normalized * factor;
    }

    public void OnTouchEnd(Vector3 pos)
    {
        TweenWork.Stop();
        Direction = Vector3.zero;
    }

    public void LateUpdate()
    {
        if (World.Mode != GameMode.Digger) return;
        var pos = MoveTrans.position + Direction * MoveSpeed * DeltaTime;
        pos = MathUtil.Clamp(pos, PosMin, PosMax);
        
        var rootPos = pos;
        rootPos.x = 0;
        Position = rootPos;

        MoveTrans.position = pos;
    }
}