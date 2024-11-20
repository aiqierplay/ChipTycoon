using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public enum CollectEntityValueMode
{
    None = 0,
    SetValue = 1,
    Add = 2,
    Reduce = 3,
}

public enum CollectEntityAddMode
{
    DefaultPrefab = 0,
    CustomPrefab = 1,
    SubInstance = 2,
}

public abstract class ItemCollectEntity<TEntity, TTarget> : ItemBase<TTarget>
    where TEntity : EntityBase
    where TTarget : EntityBase
{
    [Title("Collect")]
    public CollectEntityValueMode ValueMode = CollectEntityValueMode.Add;
    public int Value = 1;
    public TMP_Text TextValue;
    public bool IsDoor;

    public CollectEntityAddMode AddMode = CollectEntityAddMode.DefaultPrefab;
    [ShowIf(nameof(AddMode), CollectEntityAddMode.CustomPrefab)] public TEntity EntityPrefab;

    [NonSerialized] public bool Immediately = false;

    [GetComponentInChildren, NonSerialized] public List<TEntity> EntityInstancePrefab;
    [NonSerialized] public List<TEntity> EntityInstance = new List<TEntity>();

    public bool EnableCustomPrefab => AddMode == CollectEntityAddMode.CustomPrefab && EntityPrefab != null;
    public bool EnableSubInstance => AddMode == CollectEntityAddMode.SubInstance && EntityInstancePrefab != null && EntityInstancePrefab.Count > 0;

    public override void Init()
    {
        base.Init();
        Refresh();
        RefreshSubEntityInstance();
    }

    public virtual void RefreshSubEntityInstance()
    {
        EntityInstance.Clear();
        if (EnableSubInstance)
        {
            foreach (var prefab in EntityInstancePrefab)
            {
                prefab.gameObject.SetActive(true);
                var instance = GamePool.Spawn(prefab, prefab.Parent);
                instance.gameObject.SetActive(true);
                instance.Trans.CopyTrans(prefab.Trans, true);
                OnEntitySpawn(instance);
                EntityInstance.Add(instance);
                prefab.gameObject.SetActive(false);
            }
        }
    }

    public virtual void OnEntitySpawn(TEntity entity)
    {
    }


    public override void Refresh()
    {
        if (TextValue != null)
        {
            if (ValueMode != CollectEntityValueMode.None)
            {
                TextValue.text = Value.ToString();
            }
        }
    }

    public override void OnTargetEffect(TTarget target)
    {
        Collect(target);
    }

    public virtual void Collect(TTarget target)
    {
        switch (ValueMode)
        {
            case CollectEntityValueMode.None:
                break;
            case CollectEntityValueMode.Add:
            {
                Add(target, Value, Immediately);
                break;
            }
            case CollectEntityValueMode.SetValue:
                SetValue(target, Value, Immediately);
                break;
            case CollectEntityValueMode.Reduce:
            {
                Reduce(target, Value, Immediately);
                break;
            }
        }
    }

    public abstract CollectEntityHandler<TEntity> GetCollectEntityHandler(TTarget target);

    public virtual void Add(TTarget target, int value, bool immediately)
    {
        var handler = GetCollectEntityHandler(target);
        if (EnableSubInstance)
        {
            foreach (var instance in EntityInstance)
            {
                handler.AddWithInstance(instance, immediately);
            }

            EntityInstance.Clear();
        }
        else if (EnableCustomPrefab)
        {
            handler.AddWithPrefab(EntityPrefab, value, immediately);
        }
        else
        {
            handler.Add(value, immediately);
        }
    }

    public virtual void Reduce(TTarget target, int value, bool immediately)
    {
        var handler = GetCollectEntityHandler(target);
        handler.Remove(value, immediately);
    }

    public virtual void SetValue(TTarget target, int value, bool immediately)
    {
        var handler = GetCollectEntityHandler(target);
        if (handler.Count == value) return;
        if (handler.Count < value)
        {
            Add(target, value - handler.Count, immediately);
        }
        else if (handler.Count > value)
        {
            Reduce(target, handler.Count - value, immediately);
        }
    }
}
