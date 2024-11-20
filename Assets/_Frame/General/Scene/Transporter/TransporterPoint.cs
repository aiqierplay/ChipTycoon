using System;
using UnityEngine;

public class TransporterPoint
{
    public Transporter Transporter;
    public bool IsSyncPos;

    public Vector3 Position;
    public EntityBase Entity;
    public Action OnDone;

    public bool IsEmpty => Entity == null;


    public void Init(Transporter transporter, Vector3 position)
    {
        Transporter = transporter;
        Position = position;
        Set(null, false, null);
    }

    public void Set(EntityBase entity, bool isSync, Action onDone = null)
    {
        Entity = entity;
        OnDone = onDone;
        IsSyncPos = isSync;
    }

    public void Update(float deltaTime)
    {
        Position += Transporter.Direction * Transporter.MoveSpeed * deltaTime;

        var dis = Vector3.Distance(Position, Transporter.StartPos.position);
        if (dis >= Transporter.Distance)
        {
            var diff = dis - Transporter.Distance;
            Position = Transporter.StartPos.position + Transporter.Direction * diff;
            End();
        }

        if (Entity != null && IsSyncPos)
        {
            Entity.Position = Position;
        }
    }

    public void End()
    {
        if (Entity == null) return;
        OnDone?.Invoke();
        Transporter.RemoveEntity(Entity);
        Entity = null;
    }
}
