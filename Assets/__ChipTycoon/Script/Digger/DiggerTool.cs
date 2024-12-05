using System;
using System.Collections.Generic;
using Aya.Events;
using Aya.Extension;
using Aya.Maths;
using Aya.TweenPro;
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
    public Transform AbsorberPos;
    public LineRenderer Line;

    public GameObject MaxLengthTip;
    public UTweenPlayerReference TweenMaxTip;

    public float MoveSpeed = 5;
    public float RotateSpeed = 10;
    public float MaxTouchDis = 10;

    [NonSerialized] public DiggerToolMode Mode;
    [NonSerialized] public Vector3 StartPos;
    [NonSerialized] public Vector3 Direction;
    [NonSerialized] public DiggerToolData CurrentTool;

    [NonSerialized] public int Power;
    [NonSerialized] public float Length;
    [NonSerialized] public float AbsorberSpeed;

    public void Init()
    {
        Direction = Vector3.zero;
        Trans.ResetLocal();
        RootTrans.ResetLocal();
        MoveTrans.ResetLocal();
        RefreshData();
        RefreshLine(true);
        SwitchTool(DiggerToolMode.Digger);
    }

    [Listen(GameEvent.Upgrade)]
    public void RefreshData()
    {
        Power = Upgrade.GetInfo<DiggerPowerData>(CurrentLevel.SaveKey).Current.IntValue;
        Length = Upgrade.GetInfo<DiggerLengthData>(CurrentLevel.SaveKey).Current.Value;
        AbsorberSpeed = Upgrade.GetInfo<AbsorberSpeedData>(CurrentLevel.SaveKey).Current.Value;
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

        if (CurrentTool.Mode == DiggerToolMode.Absorber)
        {
            CurrentTool.Target.transform.SetLocalScale(AbsorberSpeed);
        }
    }

    public void OnTouchStart(Vector3 pos)
    {
        StartPos = pos;
        CurrentTool.Start();
        UIDigger.Ins.ControlObj.SetActive(false);
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
        UIDigger.Ins.ControlObj.SetActive(true);
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

    [NonSerialized] public bool IsMaxLength;

    public void RefreshLine(bool force = false)
    {
        if (World.Mode != GameMode.Digger)
        {
            Line.positionCount = 0;
            return;
        }

        Line.SetPositions(new Vector3[]{Position, MoveTrans.position});
        Line.positionCount = 2;

        var length = (MoveTrans.position - Position).magnitude;
        var max = length >= Length * 0.98f;

        if (IsMaxLength != max || force)
        {
            IsMaxLength = max;
            MaxLengthTip.SetActive(max);
            if (max) TweenMaxTip.Play();
            else TweenMaxTip.Value.PlayBackward();
        }
    }
}