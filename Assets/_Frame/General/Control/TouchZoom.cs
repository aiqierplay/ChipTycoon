using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TouchZoomCamMode
{
    LensOrthoSize = 0,
    FramingTransposerCameraDistance = 1,
}

public class TouchZoom : EntityBase
{
    public TouchZoomCamMode Mode;
    public Vector2 ValueRange;
    public bool BlockWithUi = true;

    public Func<float> ValueGetter;
    public Action<float> ValuerSetter;

    private bool _isTouching;
    private float _startTouchDis;
    private float _startValue;

    protected override void OnEnable()
    {
        base.OnEnable();
        _isTouching = false;
    }

    public void SetDefaultImpl()
    {
        ValueGetter = GetCameraZoomValue;
        ValuerSetter = SetCameraZoomValue;
    }

    public float GetCameraZoomValue()
    {
        switch (Mode)
        {
            case TouchZoomCamMode.LensOrthoSize:
                return Camera.Current.Camera.m_Lens.OrthographicSize;
            case TouchZoomCamMode.FramingTransposerCameraDistance:
                return Camera.Current.Camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        }

        return 0f;
    }

    public void SetCameraZoomValue(float value)
    {
        switch (Mode)
        {
            case TouchZoomCamMode.LensOrthoSize:
                Camera.Current.Camera.m_Lens.OrthographicSize = value;
                break;
            case TouchZoomCamMode.FramingTransposerCameraDistance:
                Camera.Current.Camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = value;
                break;
        }
    }

    public void Update()
    {
        if (Input.touchCount != 2) return;
        var touch1 = Input.GetTouch(0);
        var touch2 = Input.GetTouch(1);
        if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
        {
            if (BlockWithUi)
            {
                if (EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1))
                {
                    return;
                }
            }

            var currentDistance = Vector2.Distance(touch1.position, touch2.position);
            _isTouching = true;
            _startTouchDis = currentDistance;
            _startValue = ValueGetter();
        }

        if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
        {
            if (!_isTouching) return;
            var currentDistance = Vector2.Distance(touch1.position, touch2.position);
            var factor = currentDistance / _startTouchDis;
            if (ValueRange.y > ValueRange.x)
            {
                var value = _startValue * factor;
                value = Mathf.Clamp(value, ValueRange.x, ValueRange.y);
                ValuerSetter(value);
            }
            else
            {
                var value = _startValue / factor;
                value = Mathf.Clamp(value, ValueRange.y, ValueRange.x);
                ValuerSetter(value);
            }
        }

        if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
        {
            _isTouching = false;
        }
    }
}