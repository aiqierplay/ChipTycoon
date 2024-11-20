using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class Touchable : EntityBase
{
    [Title("Touchable")] 
    public bool AutoFillGap = true;
    [ShowIf(nameof(AutoFillGap))] public float MaxMoveDistance = 1f;

    [NonSerialized] public bool IsTouching;
    [NonSerialized] public Vector3 LastTouchPos;
    [NonSerialized] public int TouchPointCount;

    [FoldoutGroup("Callback"), PropertyOrder(99999)] public UnityEvent<Vector3> OnTouchStartEvent;
    [FoldoutGroup("Callback"), PropertyOrder(99999)] public UnityEvent<Vector3> OnTouchEndEvent;
    [FoldoutGroup("Callback"), PropertyOrder(99999)] public UnityEvent<Vector3> OnTouchEvent;

    public Action<Vector3> OnTouchStart = delegate { };
    public Action<Vector3> OnTouchEnd = delegate { };
    public Action<Vector3> OnTouching = delegate { };

    public virtual void Init(Action<Vector3> touchStart, Action<Vector3> touchEnd, Action<Vector3> touching)
    {
        OnTouchStart = touchStart;
        OnTouchEnd = touchEnd;
        OnTouching = touching;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        IsTouching = false;
    }

    protected virtual void TouchStart(Vector3 touchPoint)
    {
        if (!CanTouch()) return;
        if (IsTouching) return;
        IsTouching = true;
        TouchPointCount = 0;
        OnTouchStartEvent.Invoke(touchPoint);
        OnTouchStart?.Invoke(touchPoint);
    }

    protected virtual void TouchEnd(Vector3 touchPoint)
    {
        if (!IsTouching) return;
        IsTouching = false;
        OnTouchEndEvent.Invoke(touchPoint);
        OnTouchEnd?.Invoke(touchPoint);
    }

    protected virtual void Touching(Vector3 touchPoint)
    {
        if (!IsTouching) return;
        if (TouchPointCount == 0)
        {
            TouchPointCount++;
            TouchingImpl(touchPoint);
        }
        else
        {
            if (AutoFillGap)
            {
                var distance = (touchPoint - LastTouchPos).magnitude;
                if (distance > MaxMoveDistance)
                {
                    var step = 1f / (distance / MaxMoveDistance);
                    for (var i = step; i <= 1f; i += step)
                    {
                        TouchPointCount++;
                        var lerpPos = Vector3.Lerp(LastTouchPos, touchPoint, i);
                        TouchingImpl(lerpPos);
                    }
                }
            }
            
            TouchPointCount++;
            TouchingImpl(touchPoint);
        }

        LastTouchPos = touchPoint;
    }

    protected virtual void TouchingImpl(Vector3 touchPoint)
    {
        OnTouchEvent.Invoke(touchPoint);
        OnTouching?.Invoke(touchPoint);
    }


    public virtual bool CanTouch()
    {
        return true;
    }
}
