using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Maths;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public enum CollectPlaceType
{
    Circle = 0,
    Stack = 1,
    Custom = 2,
}

public abstract class CollectEntityHandler<TEntity> : EntityBase
    where TEntity : EntityBase
{
    public TEntity DefaultPrefab;
    public Transform CollectTrans;
    public CollectPlaceType PlaceType = CollectPlaceType.Stack;

    [ShowIf(nameof(PlaceType), CollectPlaceType.Circle)]
    public float CircleRadius = 5f;

    [ShowIf(nameof(PlaceType), CollectPlaceType.Stack)]
    public Vector2Int StackGird = new Vector2Int(1, 1);

    [ShowIf(nameof(PlaceType), CollectPlaceType.Stack)]
    public float CollectFlyHeight = 3f;

    [ShowIf(nameof(PlaceType), CollectPlaceType.Stack)]
    public Vector3 StackCellSize = new Vector3(1f, 1f, 1f);

    public float CollectDuration = 0.5f;
    public int CachePointCount = 100;
    public float Scale = 1f;

    [NonSerialized] public List<TEntity> EntityList = new List<TEntity>();
    public int Count => EntityList.Count;
    public TEntity this[int index] => EntityList[index];
    [NonSerialized] public List<Vector3> CachePositionList = new List<Vector3>();

    public virtual void Init()
    {
        EntityList.Clear();
        CacheEntityPositionList();
    }

    #region Add

    public virtual List<TEntity> Add(int value, bool immediately = false)
    {
        return AddWithPrefab(DefaultPrefab, value, immediately);
    }

    public virtual TEntity AddWithInstance(TEntity entity, bool immediately = false)
    {
        OnBeforeAdd(entity, immediately);
        AddImpl(entity, immediately);
        RefreshEntityPlacement();
        OnAfterAdd(entity, immediately);

        return entity;
    }

    public virtual List<TEntity> AddWithPrefab(TEntity entityPrefab, int value, bool immediately = false)
    {
        var result = new List<TEntity>();
        if (value == 0) return result;
        for (var i = 0; i < value; i++)
        {
            var entity = SpawnEntity(entityPrefab);
            OnBeforeAdd(entity);
            AddImpl(entity, immediately);
            result.Add(entity);
        }

        RefreshEntityPlacement();

        foreach (var entity in result)
        {
            OnAfterAdd(entity, immediately);
        }

        return result;
    }

    protected virtual TEntity AddImpl(TEntity entity, bool immediately = false)
    {
        entity.Parent = CollectTrans;
        EntityList.Add(entity);
        return entity;
    }

    public virtual void OnBeforeAdd(TEntity entity, bool immediately = false)
    {
        if (immediately) return;
        if (PlaceType == CollectPlaceType.Stack)
        {
            entity.SetParam("FlyStart", TransformData.Create(entity.Trans));
        }

        if (PlaceType == CollectPlaceType.Circle)
        {
            entity.SetParam("FollowStart", TransformData.Create(entity.Trans));
        }
    }

    public virtual void OnAfterAdd(TEntity entity, bool immediately = false)
    {
        if (!immediately)
        {
            if (PlaceType == CollectPlaceType.Stack)
            {
                var start = entity.GetAndRemoveParam<TransformData>("FlyStart");
                var end = TransformData.Create(entity.Trans);
                UTween.Value(0f, 1f, CollectDuration, value =>
                {
                    if (!EntityList.Contains(entity)) return;
                    var endPos = Trans.TransformPoint(end.LocalPosition);
                    var pos = TweenParabola.GetPositionByFactor(start.Position, endPos, CollectFlyHeight, value);
                    entity.Position = pos;
                    var angle = Vector3.Lerp(entity.EulerAngles, EulerAngles, value);
                    entity.EulerAngles = angle;

                })
                    .SetUpdateMode(UpdateMode.LateUpdate)
                    .SetOnStop(() =>
                    {
                        if (EntityList.Contains(entity))
                        {
                            OnAddComplete(entity);
                        }
                    });
            }

            if (PlaceType == CollectPlaceType.Circle)
            {
                var start = entity.GetAndRemoveParam<TransformData>("FollowStart");
                var end = TransformData.Create(entity.Trans);
                var startPos = start.Position;
                UTween.Value(0f, 1f, CollectDuration, value =>
                {
                    if (!EntityList.Contains(entity)) return;
                    var endPos = Trans.TransformPoint(end.LocalPosition);
                    var pos = Vector3.Lerp(startPos, endPos, value);
                    entity.Position = pos;
                })
                    .SetUpdateMode(UpdateMode.LateUpdate)
                    .SetOnStop(() =>
                    {
                        entity.LocalPosition = end.LocalPosition;
                        if (EntityList.Contains(entity))
                        {
                            OnAddComplete(entity);
                        }
                    });
            }
        }
        else
        {
            OnAddComplete(entity);
        }
    }

    public virtual void OnAddComplete(TEntity entity)
    {

    }

    public virtual TEntity SpawnEntity(TEntity entityPrefab)
    {
        var entity = GamePool.Spawn(entityPrefab, CollectTrans);
        OnEntitySpawn(entity);
        return entity;
    }

    public virtual void OnEntitySpawn(TEntity entity)
    {

    }

    #endregion

    #region Remove

    public virtual List<TEntity> Remove(int value, bool immediately = false)
    {
        var result = new List<TEntity>();
        if (value == 0) return result;
        if (value > Count) value = Count;
        for (var i = 0; i < value; i++)
        {
            var entity = EntityList.Last();
            RemoveImpl(entity);
            result.Add(entity);
        }

        RefreshEntityPlacement();
        return result;
    }

    public virtual TEntity Remove(bool immediately = false)
    {
        if (Count == 0) return default;
        var entity = EntityList.Last();
        RemoveImpl(entity);
        RefreshEntityPlacement();
        return entity;
    }


    public virtual TEntity Remove(TEntity entity, bool immediately = false)
    {
        RemoveImpl(entity);
        RefreshEntityPlacement();
        return entity;
    }

    protected virtual TEntity RemoveImpl(TEntity entity, bool immediately = false)
    {
        EntityList.Remove(entity);
        OnRemove(entity, immediately);
        if (immediately)
        {
            GamePool.DeSpawn(entity);
        }

        return entity;
    }

    public virtual void OnRemove(TEntity entity, bool immediately = false)
    {

    }

    public virtual void Clear(bool immediately = false)
    {
        for (var i = EntityList.Count - 1; i >= 0; i--)
        {
            var entity = EntityList[i];
            RemoveImpl(entity, immediately);
        }

        RefreshEntityPlacement();
    }

    #endregion

    #region Refresh

    public virtual void RefreshEntityPlacement()
    {
        if (EntityList.Count > CachePositionList.Count)
        {
            CachePointCount = EntityList.Count * 2;
            CacheEntityPositionList();
        }

        for (var i = 0; i < EntityList.Count; i++)
        {
            var entity = EntityList[i];
            var position = CachePositionList[i];
            entity.LocalPosition = position;
            entity.LocalScaleValue = Scale;
        }
    }

    #endregion

    #region Cache Position

    public virtual void CacheEntityPositionList()
    {
        CachePositionList.Clear();
        switch (PlaceType)
        {
            case CollectPlaceType.Circle:
                CachePositionList = MathUtil.GetPointsInCircle(Vector3.zero, CircleRadius, CachePointCount);
                break;
            case CollectPlaceType.Stack:
                for (var i = 0; i < CachePointCount; i++)
                {
                    var point = StackList.GetItemPosition(i, StackGird, StackCellSize);
                    CachePositionList.Add(point);
                }

                break;
            case CollectPlaceType.Custom:
                for (var i = 0; i < CachePointCount; i++)
                {
                    var point = GetCustomEntityPosition(i);
                    CachePositionList.Add(point);
                }

                break;
        }
    }

    public virtual Vector3 GetCustomEntityPosition(int index)
    {
        return Vector3.zero;
    }

    #endregion
}
