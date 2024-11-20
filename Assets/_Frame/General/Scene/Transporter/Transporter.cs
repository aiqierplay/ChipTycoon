using System;
using System.Collections.Generic;
using System.Net;
using Aya.Extension;
using Dreamteck.Splines.Primitives;
using UnityEngine;

public class Transporter : EntityBase
{
    public Transform From;
    public Transform To;
    public Transform StartPos;
    public float MoveSpeed;
    public float PointInterval = 1.5f;
    public UTweenPlayerReference TweenTransfer;

    [NonSerialized] public List<EntityBase> EntityList = new List<EntityBase>();
    [NonSerialized] public List<TransporterPoint> PointList = new List<TransporterPoint>();

    [NonSerialized] public Vector3 Direction;
    [NonSerialized] public float Distance;

    public void Init()
    {
        InitPoint();
        TweenTransfer.Stop();
        EntityList.Clear();
    }

    public void InitPoint()
    {
        Direction = (To.position - From.position).normalized;
        Distance = Vector3.Distance(From.position, To.position);
        var count = Mathf.FloorToInt(Distance / PointInterval);

        PointList.Clear();
        var pos = From.position;
        for (var i = 0; i < count; i++)
        {
            var point = new TransporterPoint();
            point.Init(this, pos);
            pos += Direction * PointInterval;
            PointList.Add(point);
        }
    }

    public bool HasEmptyPoint(int count = 1)
    {
        var counter = 0;
        foreach (var point in PointList)
        {
            if (point.IsEmpty) counter++;
        }

        return counter >= count;
    }

    public void AddEntity(EntityBase entity)
    {
        EntityList.Add(entity);
    }

    public void RemoveEntity(EntityBase entity)
    {
        EntityList.Remove(entity);
    }

    [NonSerialized] public bool IsRunning;

    public void StartTransfer()
    {
        if (EntityList.Count > 0)
        {
            TweenTransfer.Play();
            IsRunning = true;
        }
    }

    public void StopTransfer()
    {
        if (EntityList.Count == 0)
        {
            TweenTransfer.Pause();
            IsRunning = false;
        }
    }

    public TransporterPoint GetEmptyPoint()
    {
        var points = GetEmptyPoint(1);
        if (points.Count > 0) return points[0];
        return null;
    }

    public List<TransporterPoint> GetEmptyPoint(int count)
    {
        var point = PointList.FindAll(p =>
            {
                var dis = Vector3.Distance(p.Position, To.position);
                var startDis = Vector3.Distance(StartPos.position, To.position);
                return dis >= startDis;
            })
            .SortDesc(p => p.Position.x)
            .First(count, p => p.IsEmpty);
        return point;
    }

    public void Transfer<TEntity>(TEntity entity, Action onDone = null)
        where TEntity : EntityBase
    {
        var point = GetEmptyPoint();
        if (point == null) return;
        Transfer(entity, point, onDone);
    }

    public void Transfer<TEntity>(TEntity entity, TransporterPoint point, Action onDone = null)
        where TEntity : EntityBase
    {
        AddEntity(entity);
        point.Set(entity, true, onDone);
    }

    public void Update()
    {
        if (EntityList.Count > 0) StartTransfer();
        if (EntityList.Count == 0) StopTransfer();

        if (IsRunning)
        {
            var deltaTime = DeltaTime;
            foreach (var point in PointList)
            {
                point.Update(deltaTime);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var point in PointList)
        {
            Gizmos.DrawCube(point.Position, Vector3.one * 0.5f);
        }
    }
}
