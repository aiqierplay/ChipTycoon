using System;
using UnityEngine;

[RequireComponent(typeof(CanvasRenderer))]
public class UITouchSlideArea : UITouchArea
{
    public float TouchCheckDis = 100f;

    public virtual void Init()
    {
        base.Init(null, null, null);
        OnTouchStart = TouchStartImpl;
        OnTouchEnd = TouchEndImpl;
        OnTouching = TouchMoveImpl;
    }

    [NonSerialized] public Vector3 StartPosition;
    [NonSerialized] public Vector3 EndPosition;
    [NonSerialized] public Vector3 TouchingPosition;

    public virtual void TouchStartImpl(Vector3 touchPoint)
    {
        StartPosition = touchPoint;
    }

    public virtual void TouchEndImpl(Vector3 touchPoint)
    {
        EndPosition = touchPoint;
        var offset = EndPosition - StartPosition;
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
        {
            if (Mathf.Abs(offset.x) < TouchCheckDis) return;
            if (offset.x < 0) OnSlideLeft();
            if (offset.x > 0) OnSlideRight();
        }
        else
        {
            if (Mathf.Abs(offset.y) < TouchCheckDis) return;
            if (offset.y < 0) OnSlideDown();
            if (offset.y > 0) OnSlideUp();
        }
    }

    public virtual void TouchMoveImpl(Vector3 touchPoint)
    {
        TouchingPosition = touchPoint;
        var offset = TouchingPosition - StartPosition;
        OnSliding(offset);
    }

    public virtual void OnSlideUp()
    {

    }

    public virtual void OnSlideDown()
    {

    }

    public virtual void OnSlideLeft()
    {

    }

    public virtual void OnSlideRight()
    {

    }

    public virtual void OnSliding(Vector3 offset)
    {

    }
}