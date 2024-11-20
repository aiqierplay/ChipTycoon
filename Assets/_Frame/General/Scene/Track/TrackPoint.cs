using System;
using UnityEngine;

public enum TrackPointType
{
    Normal = 0,
    Shift = 1,
}

[Serializable]
public class TrackPointData
{
    public TrackPointType Type;

    public Vector3 Position;
    public Vector3 Direction;
    public Vector3 VerticalLine;

    public float Distance;
    public float TotalDistance;

    public TrackPointData PreviewPointData;
    public TrackPointData NextPointData;

    public TrackPointData(Vector3 position)
    {
        Position = position;
    }
}

public class TrackPoint : EntityBase
{
    public TrackPointType Type = TrackPointType.Normal;

    [NonSerialized] public TrackPointData Data;
    [NonSerialized] public TrackPoint NextPoint;
    [NonSerialized] public TrackPoint PreviewPoint;

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (Type  == TrackPointType.Normal)
        {
            Gizmos.color = Color.red * 0.75f;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.75f;
        }

        Gizmos.DrawSphere(transform.position, 1f);
    }
#endif
}
