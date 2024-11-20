using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Physical;
using Dreamteck.Splines;
using UnityEngine;

public class PlayerControlShoot : PlayerControl
{
    public override PlayerControlMode ControlMode => PlayerControlMode.Shoot;

    public GameObject TouchObj;
    public LineRenderer LineRenderer;
    public SplineComputer SplineComputer;
    public Transform Arrow;
    public LayerMask TouchLayer;
    public LayerMask ReflexLayer;
    public float PreviewLength;
    public Vector3 PreviewOffset;

    public Action OnShootStart = delegate { };
    public Action<Vector3> OnShoot = delegate { };
    public Action<Vector3> OnShootEnd = delegate { };

    [NonSerialized] public Vector3 StartPosition;
    [NonSerialized] public Vector3 TouchPosition;
    [NonSerialized] public bool IsMouseDown;

    public Vector3 Direction => (StartPosition - TouchPosition).normalized;

    public override void InitComponent()
    {
        base.InitComponent();
        IsMouseDown = false;
    }

    public override void Update()
    {
        if (!State.EnableInput) IsMouseDown = false;
        TouchObj.SetActive(State.EnableInput && IsMouseDown);

        if (!IsGaming) return;
        if (!State.EnableInput) return;

        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public override void UpdateImpl(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsMouseDown = true;
            OnShootStart?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!IsMouseDown) return;
            IsMouseDown = false;
            OnShootEnd?.Invoke(Direction);
        }

        if (IsMouseDown)
        {
            var touch = Physics.Raycast(Camera.MainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo, 1000f, TouchLayer.value);
            if (touch)
            {
                StartPosition = Position;
                StartPosition.y = hitInfo.point.y;
                TouchPosition = hitInfo.point;
            }

            UpdateLineRenderer();
            OnShoot?.Invoke(Direction);
        }
    }

    public void UpdateLineRenderer()
    {
        var length = PreviewLength;
        var startPos = StartPosition;
        var direction = Direction;
        var linePosList = GetShootPath(startPos, direction, length, ReflexLayer, PreviewOffset);
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

        if (Arrow != null)
        {
            Arrow.position = linePosList.Last();
            if (direction.sqrMagnitude > 0)
            {
                Arrow.forward = direction;
            }
        }
    }

    public List<Vector3> GetShootPath(Vector3 startPos, Vector3 direction, float distance, LayerMask reflexLayer)
    {
        return GetShootPath(startPos, direction, distance, reflexLayer, Vector3.zero);
    }

    public List<Vector3> GetShootPath(Vector3 startPos, Vector3 direction, float distance, LayerMask reflexLayer, Vector3 offset)
    {
        return PhysicsUtil.GetReflectionPath(startPos, direction, distance, reflexLayer, offset);
    }

    public override void ClearInput()
    {
        IsMouseDown = false;
    }
}
