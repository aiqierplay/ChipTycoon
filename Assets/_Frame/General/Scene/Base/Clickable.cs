using System;
using UnityEngine;
using UnityEngine.Events;

public class Clickable : Raycastable
{
    public UnityEvent OnClickEvent;
    public Action OnClickAction;
    
    [NonSerialized] public float ClickDuration = 0.25f;

    private (Transform, Vector3) _downRayResult;
    private float _downTime;
    private (Transform, Vector3) _upRayResult;
    private float _upTime;

    public virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _downRayResult = CheckRaycastTrans();
            _downTime = Time.realtimeSinceStartup;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _upRayResult = CheckRaycastTrans();
            _upTime = Time.realtimeSinceStartup;

            if (_upTime - _downTime > ClickDuration) return;
            if (_downRayResult.Item1 != _upRayResult.Item1)
            {
                return;
            }

            if (_upRayResult.Item1 == null)
            {
                return;
            }

            if (_upRayResult.Item1 != Trans)
            {
                return;
            }

            OnClick();
        }
    }

    public virtual void OnClick()
    {
        OnClickEvent.Invoke();
        OnClickAction?.Invoke();
    }
}