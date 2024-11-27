using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Maths;
using Sirenix.OdinInspector;
using UnityEngine;

public enum DiggerToolMode
{
    Digger = 0,
    Absorber = 1,
}

public class DiggerTool : EntityBase
{
    public Vector3 PosMin;
    public Vector3 PosMax;

    [TableList] public List<DiggerToolData> ToolList;

    public Transform RootTrans;
    public Transform MoveTrans;
    public LineRenderer Line;
    public float Length = 3;

    public float MoveSpeed = 5;
    public float RotateSpeed = 10;
    public float MaxTouchDis = 10;

    [NonSerialized] public DiggerToolMode Mode;
    [NonSerialized] public Vector3 StartPos;
    [NonSerialized] public Vector3 Direction;
    [NonSerialized] public DiggerToolData CurrentTool;
    
    public void Init()
    {
        Direction = Vector3.zero;
        Trans.ResetLocal();
        RootTrans.ResetLocal();
        MoveTrans.ResetLocal();
        RefreshLine();
        SwitchTool(DiggerToolMode.Digger);
    }

    public void SwitchTool(DiggerToolMode mode)
    {
        Mode = mode;
        foreach (var data in ToolList)
        {
            if (data.Mode == mode)
            {
                data.Active();
                CurrentTool = data;
            }
            else
            {
                data.DeActive();
            }
        }
    }

    public void OnTouchStart(Vector3 pos)
    {
        StartPos = pos;
        CurrentTool.Start();
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
        Direction = Vector3.zero;
        CurrentTool.Stop();
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

        if (Mode == DiggerToolMode.Absorber)
        {
            var targetDirection = -Direction;
            var toolDirection = Vector3.Lerp(CurrentTool.Target.transform.up, targetDirection, RotateSpeed * DeltaTime);
            CurrentTool.Target.transform.up = toolDirection;
        }

        RefreshLine();
    }

    public void RefreshLine()
    {
        Line.SetPositions(new Vector3[]{Position, MoveTrans.position});
        Line.positionCount = 2;
    }
}