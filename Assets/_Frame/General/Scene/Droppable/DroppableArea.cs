using System;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public enum DraggableDropMode
{
    EmptyToPlace = 0,
    Replace = 1,
    Custom = 2,
}

public abstract class DroppableArea : EntityBase
{
    [BoxGroup("Droppable")] public DraggableDropMode DropMode = DraggableDropMode.EmptyToPlace;
    [BoxGroup("Droppable")] public bool AllowPickup = true;
    [BoxGroup("Droppable")] public bool AllowDrop = true;
    [BoxGroup("Droppable")] public Transform PlaceTrans;
    [BoxGroup("Droppable")] public GameObject CanDropTip;
    [BoxGroup("Droppable")] public GameObject CanNotDropTip;

    [GetComponentInChildren, NonSerialized] public Collider DropCollider;
    [NonSerialized] public DraggableItem CurrentItem;
    [NonSerialized] public bool IsPlaceHolder;
    [NonSerialized] public DraggableItem HoverItem;

    [NonSerialized] public DroppableGroup DroppableGroup;
    [NonSerialized] public int GroupIndex;
    [NonSerialized] public Vector2Int GroupGridIndex;

    public bool IsEmpty => CurrentItem == null;

    public virtual void Init(DroppableGroup group = null)
    {
        DroppableGroup = group;
        CurrentItem = null;
        IsPlaceHolder = false;
        if (CanDropTip != null) CanDropTip.SetActive(false);
        if (CanNotDropTip != null) CanNotDropTip.SetActive(false);
    }

    public virtual void Enter(DraggableItem item, DroppableArea enterStartArea = null)
    {
        if (enterStartArea == null) enterStartArea = this;
        item.CurrentArea = enterStartArea;
        item.CurrentGroup = DroppableGroup;
        HoverItem = item;
        OnEnter(item);
        var canDrop = CheckCanDrop(item, enterStartArea);
        if (CanDropTip != null) CanDropTip.SetActive(canDrop);
        if (CanNotDropTip != null) CanNotDropTip.SetActive(!canDrop);
    }

    public virtual void Move(DraggableItem item)
    {
        OnMove(item);
    }

    public virtual void Exit(DraggableItem item)
    {
        item.CurrentArea = null;
        item.CurrentGroup = null;
        HoverItem = null;
        OnExit(item);
        if (CanDropTip != null) CanDropTip.SetActive(false);
        if (CanNotDropTip != null) CanNotDropTip.SetActive(false);
    }

    public abstract void OnEnter(DraggableItem item);
    public abstract void OnMove(DraggableItem item);
    public abstract void OnExit(DraggableItem item);

    public virtual bool Pickup(DraggableItem item)
    {
        if (CurrentItem == null) return false;
        if (!AllowPickup) return false;
        item.DragState = DraggableItemState.Dragging;
        item.Parent = null;
        item.CurrentArea = null;
        item.Trans.ResetLocal();
        CurrentItem = null;
        OnPickup(item);
        return true;
    }

    public virtual bool Drop(DraggableItem item, DroppableArea dropStartArea = null)
    {
        if (dropStartArea == null) dropStartArea = this;
        var canDrop = this != dropStartArea || CheckCanDrop(item, dropStartArea);
        if (!canDrop) return false;
        switch (DropMode)
        {
            case DraggableDropMode.EmptyToPlace:
                return DropImpl(item, dropStartArea);
            case DraggableDropMode.Replace:
                ReplaceImpl(item, dropStartArea);
                return DropImpl(item, dropStartArea);
            case DraggableDropMode.Custom:
                return DropCustom(item, CurrentItem, dropStartArea);
        }

        return false;
    }

    public virtual bool CheckCanDrop(DraggableItem dropItem, DroppableArea dropStartArea = null)
    {
        if (!AllowDrop) return false;
        if (DroppableGroup != null && DroppableGroup.PlaceHolder != DraggablePlaceHolderMode.Single)
        {
            if (dropStartArea == null) dropStartArea = this;
            var canDropGroup = DroppableGroup.CheckCanDrop(dropItem, dropStartArea);
            if (!canDropGroup) return false;
        }

        switch (DropMode)
        {
            case DraggableDropMode.EmptyToPlace: return IsEmpty;
            case DraggableDropMode.Replace: return true;
            case DraggableDropMode.Custom: return CheckCanDropCustom(dropItem, dropStartArea);
        }

        return false;
    }

    protected virtual bool DropImpl(DraggableItem item, DroppableArea dropStartArea = null)
    {
        CurrentItem = item;
        IsPlaceHolder = dropStartArea != this;

        item.DragState = DraggableItemState.Place;
        item.CurrentArea = dropStartArea;
        item.CurrentGroup = DroppableGroup;
        item.FromArea = dropStartArea;
        if (!IsPlaceHolder)
        {
            item.Parent = PlaceTrans == null ? Trans : PlaceTrans;
            item.Trans.ResetLocal();
        }

        OnDrop(item);
        return true;
    }

    public virtual void ReplaceImpl(DraggableItem item, DroppableArea dropStartArea = null)
    {
        if (this != dropStartArea) return;
        if (CurrentItem != null)
        {
            var currentItem = CurrentItem;
            currentItem.Pickup();
            currentItem.Drop(item.FromArea);
            if (this.DropMode != DraggableDropMode.Replace) item.Drop(this);
        }
    }

    public virtual bool DropCustom(DraggableItem srcItem, DraggableItem dstItem, DroppableArea dropStartArea = null)
    {
        return false;
    }

    public virtual bool CheckCanDropCustom(DraggableItem dropItem, DroppableArea dropStartArea = null)
    {
        return true;
    }

    public virtual void Clear()
    {
        if (CurrentItem != null)
        {
            GamePool.DeSpawn(CurrentItem);
            CurrentItem = null;
        }
    }

    public abstract void OnPickup(DraggableItem item);
    public abstract void OnDrop(DraggableItem item);
}