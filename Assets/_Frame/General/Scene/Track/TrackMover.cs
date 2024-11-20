using System;
using UnityEngine;

public class TrackMover : EntityBase
{
    [NonSerialized] public TrackLine TrackLine;
    [NonSerialized] public int PointIndex;
    [NonSerialized] public float Distance;

    public Action OnCompleteTurn = delegate { };

    public TrackPointData CurrentPoint
    {
        get
        {
            if (TrackLine == null) return null;
            return TrackLine[PointIndex];
        }
    }

    public float Progress => Distance / TrackLine.Distance;

    public void Init(TrackLine track)
    {
        TrackLine = track;
        Distance = 0f;
        PointIndex = 0;
        Move(0f, 0f);
    }

    public void SetToPoint(TrackPointData point)
    {
        Distance = point.TotalDistance;
        PointIndex = TrackLine.PointDataList.IndexOf(point);
        Move(0f, 0f);
    }

    public void Move(float speed, float deltaTime)
    {
        if (TrackLine == null) return;
        Distance += speed * deltaTime;
        MoveDistance(Distance);
    }

    public void MoveDistance(float distance)
    {
        Distance = distance;
        while (Distance >= TrackLine.Distance)
        {
            CompleteTurn();
            Distance -= TrackLine.Distance;
        }

        var index = TrackLine.Count - 1;
        while (index >= 0)
        {
            var point = TrackLine[index];
            var totalDistance = point.TotalDistance;
            if (Distance >= totalDistance)
            {
                PointIndex = index;
                var remainDis = Distance - totalDistance;
                var p1 = TrackLine[index]; ;
                var p2 = p1.NextPointData;
                var factor = remainDis / p1.Distance;
                var forward = Vector3.Lerp(p1.Direction, p2.Direction, factor);
                Forward = forward;
                var pos = Vector3.Lerp(p1.Position, p2.Position, factor);
                Position = pos;
                break;
            }

            index--;
        }
    }

    public void CompleteTurn()
    {
        OnCompleteTurn?.Invoke();
    }
}
