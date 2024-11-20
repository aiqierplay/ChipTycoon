using System;
using Aya.Physical;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;

public enum MovableUpdateMode
{
    Update = 0,
    LateUpdate = 1,
    FixedUpdate = 2,
}

public abstract class Movable : EntityBase
{
    [BoxGroup("Movable")] public LayerMask MoveAreaLayer;
    [BoxGroup("Movable")] public float MoveCameraZOffset;
    [BoxGroup("Movable")] public Vector3 MoveMouseOffset;
    [BoxGroup("Movable")] public Vector3 MoveWorldOffset;
    [BoxGroup("Movable")] public float MoveScale = 1f;
    [BoxGroup("Movable")] public MovableUpdateMode UpdateMode;

    [NonSerialized] public LayerMask MoveItemLayer;
    [NonSerialized] public static Transform MovingItem;

    protected Vector3 MoveStartOffset;
    protected bool IsMouseDown;

    protected override void Awake()
    {
        base.Awake();
        MoveItemLayer = LayerUtil.GetLayerMaskByIndex(gameObject.layer);
    }

    public virtual void Init()
    {
    }

    public virtual bool CanMove()
    {
        return true;
    }

    public virtual void Update()
    {
        if (UpdateMode != MovableUpdateMode.Update) return;
        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void LateUpdate()
    {
        if (UpdateMode != MovableUpdateMode.LateUpdate) return;
        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void FixedUpdate()
    {
        if (UpdateMode != MovableUpdateMode.FixedUpdate) return;
        var deltaTime = FixedDeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void UpdateImpl(float deltaTime)
    {
        if (!CanMove()) return;
        if (MovingItem != null && MovingItem != Trans) return;
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }

        if (IsMouseDown && MovingItem == Trans)
        {
            var rayResult = RayMovableArea();
            if (rayResult.Item1 == null) return;
            Position = ClampPosition(rayResult.Item2 + MoveWorldOffset + MoveStartOffset);
            Drag();
        }
    }

    public virtual Vector3 ClampPosition(Vector3 position)
    {
        return position;
    }

    public virtual ValueTuple<Transform, Vector3> RayMovableArea()
    {
        var ray = Camera.Camera.ScreenPointToRay(Input.mousePosition);
        var result = PhysicsUtil.Raycast<Transform>(ray.origin, ray.direction, 100f, MoveAreaLayer);
        return result;
    }

    public virtual ValueTuple<Transform, Vector3> RayMovableItem()
    {
        var ray = Camera.Camera.ScreenPointToRay(Input.mousePosition);
        var result = PhysicsUtil.Raycast<Transform>(ray.origin, ray.direction, 100f, MoveItemLayer);
        return result;
    }

    public virtual void MouseDown()
    {
        if (IsMouseDown) return;
        if (MovingItem != null) return;
        var rayMoveItemResult = RayMovableItem();
        var rayTarget = rayMoveItemResult.Item1;
        while (rayTarget != null && rayTarget != Trans)
        {
            rayTarget = rayTarget.parent;
        }

        if (rayTarget != Trans) return;

        var rayMoveAreaResult = RayMovableArea();
        if (rayMoveAreaResult.Item1 == null) return;

        IsMouseDown = true;
        MovingItem = Trans;
        LocalScale = Vector3.one * MoveScale;
        Position = UiToWorldPosition(Input.mousePosition + MoveMouseOffset, MoveCameraZOffset) + MoveWorldOffset;
        BeginDrag();
        OnMouseDown();
    }

    public virtual void MouseUp()
    {
        if (!IsMouseDown) return;
        var rayResult = RayMovableArea();
        if (rayResult.Item1 != null)
        {
            Position = ClampPosition(rayResult.Item2 + MoveStartOffset);
        }
        else
        {
            Position -= MoveWorldOffset;
            Position = ClampPosition(Position);
        }

        MovingItem = null;
        IsMouseDown = false;
        LocalScale = Vector3.one;
        EndDrag();
        OnMouseUp();
    }

    public virtual void BeginDrag()
    {
        OnBeginDrag();
    }

    public virtual void Drag()
    {
        OnDrag();
    }

    public virtual void EndDrag()
    {
        OnEndDrag();
    }

    public virtual void OnMouseDown()
    {

    }

    public virtual void OnMouseUp()
    {

    }

    public abstract void OnBeginDrag();
    public abstract void OnDrag();
    public abstract void OnEndDrag();
}