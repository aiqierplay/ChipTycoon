using System;
using Aya.Extension;
using Aya.Maths;
using UnityEngine;

public class Digger : EntityBase
{
    public Vector3 PosMin;
    public Vector3 PosMax;

    public Transform RootTrans;
    public Transform MoveTrans;
    public LineRenderer Line;
    public float Length = 3;

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
        RootTrans.ResetLocal();
        MoveTrans.ResetLocal();
        RefreshLine();
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
        var length = (pos - Position).magnitude;
        if (length > Length)
        {
            var targetPos = Position + Direction.normalized * Length;
            pos = Vector3.Lerp(MoveTrans.position, targetPos, MoveSpeed * DeltaTime * 5f);
        }

        pos = MathUtil.Clamp(pos, PosMin, PosMax);
        var rootPos = pos;
        rootPos.x = 0;
        RootTrans.position = rootPos;
        MoveTrans.position = pos;
        RefreshLine();
    }

    public void RefreshLine()
    {
        Line.SetPositions(new Vector3[]{Position, MoveTrans.position});
        Line.positionCount = 2;
    }
}