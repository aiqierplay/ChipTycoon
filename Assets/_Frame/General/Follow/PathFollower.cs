using System;
using UnityEngine;

public enum PathFollowMode
{
    Distance = 0,
    RemainDistance = 1,
}

public class PathFollower : EntityBase
{
    public Transform Target;
    public PathFollowMode Mode;
    public float FollowDistance;
    public float MaxDistance;
    public bool KeepForward;

    [NonSerialized] public PathRoute FollowPath;

    protected override void Awake()
    {
        base.Awake();
        FollowPath = new PathRoute()
        {
            MaxDistance = MaxDistance
        };
    }

    public void Init()
    {
        Target = null;
        FollowPath.Clear();
    }

    public void LateUpdate()
    {
        if (Target == null) return;
        FollowPath.Add(Target.position);
        var lastPos = transform.position;
        var pos = Vector3.zero;
        switch (Mode)
        {
            case PathFollowMode.Distance:
                pos = FollowPath.GetPos(FollowDistance);
                break;
            case PathFollowMode.RemainDistance:
                pos = FollowPath.GetPosByRemainDistance(FollowDistance);
                break;
        }

        transform.position = pos;
        if (KeepForward)
        {
            if (pos != lastPos)
            {
                transform.forward = pos - lastPos;
            }
            else
            {
                transform.forward = Target.forward;
            }
        }
    }
}
