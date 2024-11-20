using System;
using System.Collections.Generic;
using Aya.Extension;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerControlThrow : PlayerControl
{
    public override PlayerControlMode ControlMode => PlayerControlMode.Throw;

    [Title("Param")]
    public float CacheTimeStep = 0.1f;
    public float DropHeight = 0f;
    public RangeMapFloat ThrowSpeedMap;
    [NonSerialized] public float ThrowSpeed = 1f;
    public float Range = 1f;
    public LayerMask TouchLayer;
    [Title("Render")]
    public LineRenderer LineRenderer;
    public SplineComputer SplineComputer;
    public Transform Thrower;
    public Transform StartPos;
    public Transform DropPoint;
    public GameObject TouchObj;

    // public Vector3 MinShootDirection;
    // public Vector3 MaxShootDirection;

    public Action OnThrowStart = delegate { };
    public Action<Vector3> OnThrow = delegate { };
    public Action<Vector3> OnThrowEnd = delegate { };

    public Vector3 StartPosition => StartPos != null ? StartPos.position : Position;
    [NonSerialized] public Vector3 ThrowDirection;
    [NonSerialized] public Vector3 TouchPosition;
    [NonSerialized] public bool IsMouseDown;
    [NonSerialized] public bool IsThrowing;

    public override void InitComponent()
    {
        base.InitComponent();
        IsMouseDown = false;
        IsThrowing = false;
    }

    public override void Update()
    {
        if (State == null) return;
        if (!State.EnableInput)
        {
            IsMouseDown = false;
            IsThrowing = false;
        }

        TouchObj.SetActive(IsThrowing);

        if (!IsGaming) return;
        if (!State.EnableInput) return;

        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public override void UpdateImpl(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var touchStart = Physics.Raycast(Camera.MainCamera.ScreenPointToRay(Input.mousePosition), out var _, 1000f, TouchLayer.value);
            if (touchStart)
            {
                IsMouseDown = true;
            }

            IsThrowing = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!IsMouseDown) return;
            IsMouseDown = false;
            if (IsThrowing)
            {
                OnThrowEnd?.Invoke(ThrowDirection);
                IsThrowing = false;
            }
        }

        if (IsMouseDown)
        {
            var hit = Physics.Raycast(Camera.MainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo, 1000f, TouchLayer.value);
            if (hit)
            {
                TouchPosition = hitInfo.point;
                if (!IsThrowing)
                {
                    IsThrowing = true;
                    OnThrowStart?.Invoke();
                }
            }
        }

        if (IsThrowing)
        {
            var direction = StartPosition - TouchPosition;
            direction.y = 0f;
            direction = direction.normalized;
            direction.y = 0.5f;
            direction = direction.normalized;

            var touchDistance = (StartPosition - TouchPosition).magnitude;
            var throwSpeed = ThrowSpeedMap.Lerp(touchDistance);
            ThrowSpeed = throwSpeed;
            ThrowDirection = direction;

            UpdateLineRenderer();
            OnThrow?.Invoke(ThrowDirection);
        }

        if (Thrower != null)
        {
            if (IsThrowing)
            {
                Thrower.forward = ThrowDirection;
            }
            else
            {
                Thrower.forward = Vector3.forward;
            }
        }
    }

    public void UpdateLineRenderer()
    {
        var linePosList = CalculateParabola(StartPosition, ThrowDirection, ThrowSpeed, Physics.gravity);
        if (LineRenderer != null)
        {
            LineRenderer.Clear();
            LineRenderer.SetPath(linePosList);
        }

        if (SplineComputer != null)
        {
            var points = linePosList.Select(p => new SplinePoint(p)).ToArray();
            SplineComputer.SetPoints(points);
        }

        if (DropPoint != null)
        {
            DropPoint.position = linePosList.Last();
            DropPoint.localScale = Vector3.one * Range;
        }
    }

    public override void ClearInput()
    {
        IsMouseDown = false;
    }

    public List<Vector3> CalculateParabola(Vector3 startPos, Vector3 direction, float speed, Vector3 gravity)
    {
        var path = new List<Vector3>();
        var velocity = direction * speed;
        var position = startPos;
        path.Add(position);
        while (true)
        {
            position += velocity * CacheTimeStep;
            velocity += Physics.gravity * CacheTimeStep;
            if (position.y < DropHeight)
            {
                position.y = DropHeight;
                break;
            }

            path.Add(position);
        }

        return path;
    }
}
