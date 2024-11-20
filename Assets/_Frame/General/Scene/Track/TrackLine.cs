using System;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

public class TrackLine : EntityBase
{
    public bool IsCurve;

    [GetComponentInChildren, NonSerialized] public List<TrackPoint> PointList;

    [NonSerialized] public List<TrackPointData> PointDataList;

    public int Count => PointDataList.Count;
    [NonSerialized] public float Distance;

    public TrackPointData this[int index] => PointDataList[index];

    public void Init()
    {
        Distance = 0f;
        for (var i = 0; i < PointList.Count; i++)
        {
            TrackPoint p1;
            TrackPoint p2;
            if (i < PointList.Count - 1)
            {
                p1 = PointList[i];
                p2 = PointList[i + 1];
            }
            else
            {
                p1 = PointList[i];
                p2 = PointList[0];
            }

            p1.NextPoint = p2;
            p2.PreviewPoint = p1;
        }

        PointDataList = new List<TrackPointData>();
        if (IsCurve)
        {
            for (var i = 0; i < PointList.Count; i++)
            {
                var p1 = PointList[i];
                var p2 = p1.NextPoint;
                var p3 = p2.NextPoint;
                var p4 = p3.NextPoint;
                for (var t = 0f; t < 1f; t += 0.1f)
                {
                    var pos = EvaluateCatmullRomCurve(p1.transform.position, p2.transform.position, p3.transform.position, p4.transform.position, t);
                    var data = new TrackPointData(pos)
                    {
                        Type = p1.Type
                    };

                    PointDataList.Add(data);
                }
            }
        }
        else
        {
            for (var i = 0; i < PointList.Count; i++)
            {
                var point = PointList[i];
                point.Data = new TrackPointData(point.transform.position);
                PointDataList.Add(point.Data);
            }
        }

        for (var i = 0; i < PointDataList.Count; i++)
        {
            TrackPointData p1;
            TrackPointData p2;
            if (i < PointDataList.Count - 1)
            {
                p1 = PointDataList[i];
                p2 = PointDataList[i + 1];
            }
            else
            {
                p1 = PointDataList[i];
                p2 = PointDataList[0];
            }

            p1.NextPointData = p2;
            p2.PreviewPointData = p1;
            p1.Direction = p2.Position - p1.Position;
            p1.VerticalLine = Vector3.Cross(Vector3.up, p1.Direction).normalized;
            p1.Distance = Vector3.Distance(p1.Position, p2.Position);
            p1.TotalDistance = Distance;
            Distance += p1.Distance;
        }
    }

    public Vector3 EvaluateCatmullRomCurve(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        var t2 = t * t;
        var t3 = t2 * t;
        var point = 0.5f * ((2.0f * p2) +
                            (-p1 + p3) * t +
                            (2.0f * p1 - 5.0f * p2 + 4.0f * p3 - p4) * t2 +
                            (-p1 + 3.0f * p2 - 3.0f * p3 + p4) * t3);
        return point;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (PointList == null)
        {
            PointList = GetComponentsInChildren<TrackPoint>().ToList();
            Init();
        }

        if (PointDataList == null || PointDataList.Count < 2) return;

        Gizmos.color = Color.green;
        for (var i = 0; i < PointDataList.Count; i++)
        {
            var p = PointDataList[i];
            var p1 = p.Position;
            var p2 = p.NextPointData.Position;
            Gizmos.DrawLine(p1 + Vector3.up, p2 + Vector3.up);
            Gizmos.DrawLine(p1 + Vector3.up, p1 + Vector3.up + p.Direction * 5f);
            Gizmos.DrawLine(p1 + Vector3.up, p1 + Vector3.up + p.VerticalLine * 5f);
        }
    }
#endif
}
