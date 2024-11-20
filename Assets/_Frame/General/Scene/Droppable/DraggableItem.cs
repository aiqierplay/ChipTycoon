using System;
using Aya.Physical;
using Sirenix.OdinInspector;
using UnityEngine;

public enum DraggableItemState
{
    Place,
    Dragging,
}

public enum DraggablePlaceHolderMode
{
    Single = 0,
    Shape = 1,
}

public enum DraggableUpdateMode
{
    Update = 0,
    LateUpdate = 1,
    FixedUpdate = 2,
}

public abstract class DraggableItem : EntityBase
{
    [BoxGroup("Draggable")] public LayerMask DragAreaLayer;
    [BoxGroup("Draggable")] public float DragCameraZOffset;
    [BoxGroup("Draggable")] public Vector3 DragMouseOffset;
    [BoxGroup("Draggable")] public Vector3 DragWorldOffset;
    [BoxGroup("Draggable")] public float PickupScale = 1f;
    [BoxGroup("Draggable")] public DraggableUpdateMode UpdateMode;
    [BoxGroup("Draggable")] public DraggablePlaceHolderMode PlaceHolder = DraggablePlaceHolderMode.Single;
    [BoxGroup("Draggable"), Multiline, ShowIf(nameof(PlaceHolder), DraggablePlaceHolderMode.Shape)] public string PlaceHolderShape;
    [BoxGroup("Draggable")] public GameObject DropFx;

    [NonSerialized] public DraggableItemState DragState;
    [NonSerialized] public DroppableArea FromArea;
    [NonSerialized] public DroppableArea CurrentArea;
    [NonSerialized] public DroppableGroup CurrentGroup;
    [NonSerialized] public static DraggableItem DraggingItem;
    [NonSerialized] public bool[,] PlaceHolderMatrix;
    [NonSerialized] public int PlaceHolderWidth;
    [NonSerialized] public int PlaceHolderHeight;

    protected bool IsMouseDown;
    protected DroppableArea lastHitDroppableArea;

    public virtual void Init(DroppableArea initArea = null)
    {
        IsMouseDown = false;
        PlaceHolderMatrix = GetPlaceHolderMatrix();
        CurrentArea = initArea;
        if (initArea != null)
        {
            Drop(CurrentArea);
        }
    }

    public virtual bool[,] GetPlaceHolderMatrix()
    {
        var lines = PlaceHolderShape.Split('\n');
        PlaceHolderWidth = lines[0].Length;
        PlaceHolderHeight = lines.Length;
        var matrix = new bool[PlaceHolderWidth, PlaceHolderHeight];
        for (var i = 0; i < PlaceHolderWidth; i++)
        {
            for (var j = 0; j < PlaceHolderHeight; j++)
            {
                matrix[i, j] = lines[j][i] == '1';
            }
        }

        return matrix;
    }

    public virtual void Update()
    {
        if (UpdateMode != DraggableUpdateMode.Update) return;
        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void LateUpdate()
    {
        if (UpdateMode != DraggableUpdateMode.LateUpdate) return;
        var deltaTime = DeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void FixedUpdate()
    {
        if (UpdateMode != DraggableUpdateMode.FixedUpdate) return;
        var deltaTime = FixedDeltaTime;
        UpdateImpl(deltaTime);
    }

    public virtual void UpdateImpl(float deltaTime)
    {
        if (DraggingItem != null && DraggingItem != this) return;
        if (Input.GetMouseButtonDown(0))
        {
            MouseDown();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUp();
        }

        if (IsMouseDown && DragState == DraggableItemState.Dragging)
        {
            Drag();
            var area = RayCheckArea();
            if (area == lastHitDroppableArea) return;
            lastHitDroppableArea = area;
            Exit();

            if (area != CurrentArea && area != null)
            {
                Enter(area);
            }

            if (area == CurrentArea && CurrentArea != null)
            {
                Move(area);
            }
        }
    }

    public virtual DroppableArea RayCheckArea()
    {
        var ray = Camera.Camera.ScreenPointToRay(Input.mousePosition);
        var area = PhysicsUtil.Raycast<DroppableArea>(ray.origin, ray.direction, 100f, DragAreaLayer).Item1;
        return area;
    }

    public virtual bool CheckCanPickup()
    {
        return true;
    }

    public virtual void Enter(DroppableArea enterArea)
    {
        enterArea.Enter(this, enterArea);
        if (CurrentGroup != null && CurrentGroup.PlaceHolder == DraggablePlaceHolderMode.Shape)
        {
            CurrentGroup.ForeachCoverDraggableArea(this, enterArea, (x, y, draggableArea) =>
            {
                if (draggableArea != null && draggableArea != enterArea) draggableArea.Enter(this, enterArea);
            });
        }
    }

    public virtual void Exit()
    {
        if (CurrentGroup != null && CurrentGroup.PlaceHolder == DraggablePlaceHolderMode.Shape)
        {
            CurrentGroup.ForeachDraggableArea((x, y, draggableArea) => draggableArea.Exit(this));
        }
        else if (CurrentArea != null)
        {
            CurrentArea.Exit(this);
        }
    }

    public virtual void Move(DroppableArea moveArea)
    {
        if (CurrentGroup != null && CurrentGroup.PlaceHolder == DraggablePlaceHolderMode.Shape)
        {
            CurrentGroup.ForeachCoverDraggableArea(this, moveArea, (x, y, draggableArea) =>
            {
                if (draggableArea != null && draggableArea != CurrentArea) draggableArea.Move(this);
            });
        }
        else
        {
            moveArea.Move(this);
        }
    }

    public virtual bool Pickup()
    {
        if (CurrentGroup != null && CurrentGroup.PlaceHolder == DraggablePlaceHolderMode.Shape)
        {
            CurrentGroup.ForeachDraggableArea((x, y, draggableArea) =>
            {
                if (draggableArea.CurrentItem == this) draggableArea.Pickup(this);
            });

            return true;
        }
        else
        {
            var pickup = CurrentArea.Pickup(this);
            return pickup;
        }
    }

    public virtual bool Drop(DroppableArea dropArea)
    {
        var canDrop = dropArea.CheckCanDrop(this, dropArea);
        if (!canDrop) return false;
        if (dropArea.DroppableGroup != null && dropArea.DroppableGroup.PlaceHolder == DraggablePlaceHolderMode.Shape)
        {
            dropArea.Drop(this);
            dropArea.DroppableGroup.ForeachCoverDraggableArea(this, dropArea, (x, y, draggableArea) =>
            {
                if (draggableArea != null && draggableArea != dropArea)
                {
                    draggableArea.Drop(this, dropArea);
                    SpawnFx(DropFx);
                }
            });

            return true;
        }
        else
        {
            var drop = dropArea.Drop(this);
            SpawnFx(DropFx);
            return drop;
        }
    }

    public virtual void MouseDown()
    {
        if (IsMouseDown) return;
        IsMouseDown = true;
        lastHitDroppableArea = null;
        if (DragState == DraggableItemState.Place)
        {
            var area = RayCheckArea();
            if (area != null && area.CurrentItem == this && CheckCanPickup())
            {
                if (!area.AllowPickup) return;
                Pickup();
                BeginDrag();
            }
        }

        OnMouseDown();
    }

    public virtual void MouseUp()
    {
        if (!IsMouseDown) return;
        IsMouseDown = false;
        if (DragState == DraggableItemState.Dragging)
        {
            var area = RayCheckArea();
            Exit();
            if (area != null)
            {
                if (!area.AllowDrop) return;
                var drop = Drop(area);
                if (!drop)
                {
                    Drop(FromArea);
                }
            }
            else
            {
                Drop(FromArea);
            }

            EndDrag();
        }

        OnMouseUp();
    }

    public virtual void BeginDrag()
    {
        DraggingItem = this;
        LocalScale = Vector3.one * PickupScale;
        OnBeginDrag();
    }

    public virtual void Drag()
    {
        Position = UiToWorldPosition(Input.mousePosition + DragMouseOffset, DragCameraZOffset) + DragWorldOffset;
        OnDrag();
    }

    public virtual void EndDrag()
    {
        DraggingItem = null;
        LocalScale = Vector3.one;
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
