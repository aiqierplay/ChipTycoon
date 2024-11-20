public abstract class MergeableArea<TItem, TData> : DroppableArea
    where TItem : MergeableItem<TItem, TData>
    where TData : MergeableData<TItem>
{
    public TItem DstItem => CurrentItem as TItem;

    public override bool CheckCanDropCustom(DraggableItem dropItem, DroppableArea dropStartArea = null)
    {
        if (DstItem == null) return true;
        var item = dropItem as TItem;
        if (item == null) return false;
        if (CheckCanMerge(item, dropStartArea)) return true;
        return false;
    }

    public virtual bool CheckCanMerge(TItem srcItem, DroppableArea dropStartArea = null)
    {
        if (srcItem.Data.IsMaxLevel) return false;
        if (DstItem.Data.ID == srcItem.Data.ID) return true;
        return false;
    }

    public override bool DropCustom(DraggableItem srcItem, DraggableItem dstItem, DroppableArea dropStartArea = null)
    {
        return DropCustom(srcItem as TItem, dstItem as TItem, dropStartArea);
    }

    public bool DropCustom(TItem srcItem, TItem dstItem, DroppableArea dropStartArea = null)
    {
        if (dstItem == null)
        {
            return DropImpl(srcItem, dropStartArea);
        }

        if (!CheckCanMerge(srcItem, dropStartArea))
        {
            ReplaceImpl(srcItem, dropStartArea);
            DropImpl(srcItem, dropStartArea);
            return true;
        }

        if (dropStartArea == null) dropStartArea = this;
        IsPlaceHolder = dropStartArea != this;
        var nextIndex = srcItem.Data.Index + 1;
        GamePool.DeSpawn(srcItem);
        Clear();
        var mergeItem = SpawnMergeableItem(nextIndex);
        mergeItem.CurrentArea = dropStartArea;
        mergeItem.CurrentGroup = dropStartArea.DroppableGroup;
        mergeItem.FromArea = dropStartArea;
        OnMerge(srcItem, dstItem, mergeItem);
        mergeItem.SpawnFx(mergeItem.MergeFx);

        return true;
    }

    public virtual TItem SpawnMergeableItem(int index = 0)
    {
        var data = Config.GetData<TData>(index);
        var mergeItem = GamePool.Spawn(data.Prefab, CurrentLevel.Trans);
        mergeItem.Init(data, this);
        return mergeItem;
    }

    public abstract void OnMerge(TItem srcItem, TItem dstItem, TItem mergeItem);
}