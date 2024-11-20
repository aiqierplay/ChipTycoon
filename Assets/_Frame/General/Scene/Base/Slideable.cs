using System;
using UnityEngine;
using UnityEngine.Events;

public class Slideable : Raycastable
{
    public UnityEvent OnSlideEvent;
    public Action OnSlideAction;

    public virtual void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var result = CheckRaycastTrans();
            if (result.Item1 == Trans)
            {
                OnSlide();
            }
        }
    }

    public virtual void OnSlide()
    {
        OnSlideEvent.Invoke();
        OnSlideAction?.Invoke();
    }
}
