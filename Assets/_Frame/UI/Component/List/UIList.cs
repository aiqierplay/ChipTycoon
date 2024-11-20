using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class UIList : UIBase
{
    [BoxGroup("List")] public Transform ItemTrans;

    [NonSerialized] public int CurrentIndex;

    public override void Refresh(bool immediately = false)
    {
        
    }
}

public abstract class UIList<TItem, TData> : UIList where TItem : UIListItem<TData>
{
    [BoxGroup("List")] public TItem ItemPrefab;

    [NonSerialized] public List<TItem> ItemList = new List<TItem>();
    public TItem Current => ItemList[CurrentIndex];

    public virtual void Init(IList<TData> dataList, bool deSpawnRemainItem = true)
    {
        if (deSpawnRemainItem)
        {
            foreach (var item in ItemList)
            {
                UIPool.DeSpawn(item);
            }
        }

        if (dataList == null) return;
        ItemList.Clear();
        for (var index = 0; index < dataList.Count; index++)
        {
            var data = dataList[index];
            var item = UIPool.Spawn(ItemPrefab, ItemTrans);
            ItemList.Add(item);
            item.Init(this, index, data);
        }

        Select(0);
        Refresh();
    }

    public virtual void Select(int index)
    {
        if (index >= ItemList.Count) return;
        index = Mathf.Clamp(index, 0, ItemList.Count - 1);
        ItemList[index].Select();
    }

    public virtual void Select(TItem item)
    {
        CurrentIndex = ItemList.IndexOf(item);
        if (CurrentIndex >= 0) item.Select();
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        foreach (var item in ItemList)
        {
            item.Refresh(immediately);
        }
    }
}