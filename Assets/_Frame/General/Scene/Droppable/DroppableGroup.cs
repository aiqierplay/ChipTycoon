using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DroppableGroup : EntityBase
{
    [BoxGroup("Droppable")] public bool AutoInit = true;
    [BoxGroup("Droppable")] public DraggablePlaceHolderMode PlaceHolder = DraggablePlaceHolderMode.Shape;
    [BoxGroup("Droppable"), ShowIf(nameof(PlaceHolder), DraggablePlaceHolderMode.Shape)] public Vector2Int Grid;

    [GetComponentInChildren, NonSerialized] public DroppableArea[] DraggableAreaList;
    [NonSerialized] public DroppableArea[,] DraggableAreaGrid;

    public DroppableArea this[int x, int y] => GetDraggableArea(x, y);

    protected override void Awake()
    {
        base.Awake();
        if (AutoInit) Init();
    }

    public virtual void Init()
    {
        foreach (var draggableArea in DraggableAreaList)
        {
            draggableArea.Init(this);
        }

        if (PlaceHolder == DraggablePlaceHolderMode.Shape)
        {
            DraggableAreaGrid = new DroppableArea[Grid.x, Grid.y];
            for (var i = 0; i < Grid.x; i++)
            {
                for (var j = 0; j < Grid.y; j++)
                {
                    var draggableArea = GetDraggableArea(i, j);
                    var index = i + j * Grid.x;
                    draggableArea.GroupIndex = index;
                    draggableArea.GroupGridIndex = new Vector2Int(i, j);
                    DraggableAreaGrid[i, j] = draggableArea;
                }
            }
        }
    }

    public virtual DroppableArea GetDraggableArea(int x, int y)
    {
        if (x < 0 || x >= Grid.x || y < 0 || y >= Grid.y) return default;
        var index = x + y * Grid.x;
        return DraggableAreaList[index];
    }

    public virtual bool CheckCanDrop(DraggableItem dropItem, DroppableArea startArea)
    {
        switch (dropItem.PlaceHolder)
        {
            case DraggablePlaceHolderMode.Single:
                return startArea.CheckCanDrop(dropItem);
            case DraggablePlaceHolderMode.Shape:
                var canDrop = true;
                ForeachCoverDraggableArea(dropItem, startArea, (x, y, draggableArea) =>
                {
                    if (canDrop == false) return;
                    if (draggableArea == null)
                    {
                        canDrop = false;
                        return;
                    }

                    if (!draggableArea.IsEmpty) canDrop = false;
                });

                return canDrop;
        }

        return false;
    }

    public virtual void ForeachDraggableArea(Action<int, int, DroppableArea> action)
    {
        for (var i = 0; i < Grid.x; i++)
        {
            for (var j = 0; j < Grid.y; j++)
            {
                var draggableArea = GetDraggableArea(i, j);
                if (draggableArea == null) continue;
                action(i, j, draggableArea);
            }
        }
    }

    public virtual void ForeachCoverDraggableArea(DraggableItem dropItem, DroppableArea startArea, Action<int,int, DroppableArea> action)
    {
        for (var i = 0; i < dropItem.PlaceHolderWidth; i++)
        {
            for (var j = 0; j < dropItem.PlaceHolderHeight; j++)
            {
                if (!dropItem.PlaceHolderMatrix[i, j]) continue;
                var x = startArea.GroupGridIndex.x + i;
                var y = startArea.GroupGridIndex.y + j;
                var draggableArea = GetDraggableArea(x, y);
                action(x, y, draggableArea);
            }
        }
    }
}
