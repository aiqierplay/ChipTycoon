using System;
using System.Collections.Generic;
using Aya.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract partial class EntityBase
{
    [GetComponent, NonSerialized] public RectTransform Rect;
    [GetComponent, NonSerialized] public Canvas Canvas;
    [GetComponent, NonSerialized] public CanvasGroup CanvasGroup;

    public UIEventListener UIListener
    {
        get
        {
            if(_uiEventListener == null) _uiEventListener = UIEventListener.Get(gameObject);
            return _uiEventListener;
        }
    }

    private UIEventListener _uiEventListener;

    public UIEventPass UIEventPass
    {
        get
        {
            if (_uiEventPass == null) _uiEventPass = UIEventPass.Get(gameObject);
            return _uiEventPass;
        }
    }

    private UIEventPass _uiEventPass;

    protected List<RaycastResult> RayCastUiResult = new List<RaycastResult>();

    public bool IsPointerOverUiObject()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };

        EventSystem.current.RaycastAll(eventDataCurrentPosition, RayCastUiResult);
        return RayCastUiResult.Count > 0;
    }
}