using System;
using Aya.Maths;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class MovableOnPlane : TouchArea
{
    public float MoveSpeed = 1f;
    public bool ClampPos;
    [ShowIf(nameof(ClampPos))] public Vector3 MinPos = new Vector3(-1, 0, -1);
    [ShowIf(nameof(ClampPos))] public Vector3 MaxPos = new Vector3(1, 0, 1);

    [NonSerialized] public Vector3 TargetStartPos;
    [NonSerialized] public Vector3 TouchStartPos;
    [NonSerialized] public Vector3 TouchEndPos;
    [NonSerialized] public Vector3 TouchLastPos;

    protected override void TouchStart(Vector3 touchPoint)
    {
        base.TouchStart(touchPoint);
        TouchStartPos = CurrentTouchPos;
        TouchLastPos = TouchStartPos;
        TargetStartPos = TargetObject.transform.position;

    }
    protected override void TouchEnd(Vector3 touchPoint)
    {
        base.TouchEnd(touchPoint);
        TouchEndPos = CurrentTouchPos;
        TouchLastPos = TouchEndPos;
    }

    protected override void Touching(Vector3 touchPoint)
    {
        base.Touching(touchPoint);
        var from = TargetObject.transform.position;
        var to = TargetStartPos + (TouchStartPos - touchPoint);
        var position = Vector3.MoveTowards(from, to, MoveSpeed * DeltaTime);
        TouchLastPos = touchPoint;

        if (ClampPos)
        {
            position = MathUtil.Clamp(position, MinPos, MaxPos);
        }

        TargetObject.transform.position = position;
    }
}