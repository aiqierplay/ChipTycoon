using System;
using Aya.UI;
using UnityEngine;
using UnityEngine.Events;

public enum UITouchEvent
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
    Horizontal = 4,
    Vertical = 5,
}

public class UIFourWayControlPanel : UIBase
{
    [NonSerialized, GetOrAddComponent] public Empty4Raycast Raycast;

    public float MoveMinDis = 100;
    
    public UnityEvent OnUp;
    public UnityEvent OnDown;
    public UnityEvent OnLeft;
    public UnityEvent OnRight;

    [NonSerialized] public Vector2 StartPos;
    [NonSerialized] public Vector2 EndPos;

    protected override void Awake()
    {
        base.Awake();
        UIEventListener.Get(gameObject).onDown += (go, data) =>
        {
            StartPos = data.position;
        };

        UIEventListener.Get(gameObject).onUp += (go, data) =>
        {
            EndPos = data.position;
            Check();
        };
    }

    public virtual void Check()
    {
        var diff = EndPos - StartPos;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            if (diff.x > MoveMinDis)
            {
                OnRight?.Invoke();
                Dispatch(UITouchEvent.Right);
                Dispatch(UITouchEvent.Horizontal);
            }

            if (diff.x <= -MoveMinDis)
            {
                OnLeft?.Invoke();
                Dispatch(UITouchEvent.Left);
                Dispatch(UITouchEvent.Horizontal);
            }
        }
        else
        {
            if (diff.y >= MoveMinDis)
            {
                OnUp?.Invoke();
                Dispatch(UITouchEvent.Up);
                Dispatch(UITouchEvent.Vertical);
            }

            if (diff.y <= -MoveMinDis)
            {
                OnDown?.Invoke();
                Dispatch(UITouchEvent.Down);
                Dispatch(UITouchEvent.Vertical);
            }
        }
    }
}
