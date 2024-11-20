using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Maths;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PathRoute
{
    [TableList] public List<PathNode> NodeList = new List<PathNode>();
    public int Count => NodeList.Count;
    public float Distance = 0f;

    public float DefaultWidth = 10f;
    public Vector3 DefaultUp = Vector3.up;

    public float MaxDistance = float.MaxValue;

    public PathRoute()
    {

    }

    public PathRoute(IEnumerable<Vector3> posList)
    {
        Init(posList);
    }

    public void Init(IEnumerable<Vector3> posList)
    {
        Clear();
        AddRange(posList);
    }

    #region Add

    public void AddRange(IEnumerable<Vector3> posList)
    {
        foreach (var pos in posList)
        {
            Add(pos, DefaultUp, DefaultWidth, false);
        }

        Refresh();
    }

    public void Add(Vector3 pos)
    {
        Add(pos, DefaultUp, DefaultWidth);
    }

    public void Add(Vector3 pos, Vector3 up, float width, bool refresh = true)
    {
        var lastNode = NodeList.Last();
        if (lastNode != null)
        {
            var dis = pos - lastNode.Position;
            if (dis.sqrMagnitude < 0.001f) return;
        }

        var newNode = new PathNode()
        {
            Index = NodeList.Count,
            Position = pos,
            Forward = Vector3.forward,
            Up = up,
            Width = width,
        };

        if (lastNode != null)
        {
            var segmentDistance = Vector3.Distance(lastNode.Position, newNode.Position);
            newNode.Forward = (pos - lastNode.Position).normalized;
            newNode.SegmentDistance = segmentDistance;
            newNode.Distance = lastNode.Distance + segmentDistance;
            Distance += segmentDistance;

            lastNode.Forward = newNode.Forward;
            var lastLeft = Vector3.Cross(lastNode.Forward, lastNode.Up).normalized;
            lastNode.Left = lastNode.Position + lastLeft * lastNode.Width / 2f;
            lastNode.Right = lastNode.Position - lastLeft * lastNode.Width / 2f;
        }

        var left = Vector3.Cross(newNode.Forward, newNode.Up).normalized;
        newNode.Left = newNode.Position + left * newNode.Width / 2f;
        newNode.Right = newNode.Position - left * newNode.Width / 2f;

        NodeList.Add(newNode);

        if (refresh)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        var remove = false;
        while (Distance > MaxDistance)
        {
            var first = NodeList.Count > 0 ? NodeList[0] : null;
            if (first == null) break;
            var segmentDistance = first.SegmentDistance;
            Distance -= segmentDistance;
            NodeList.RemoveAt(0);
            remove = true;
        }

        if (remove)
        {
            var totalDistance = 0f;
            var count = NodeList.Count;
            for (var i = 0; i < count; i++)
            {
                var node = NodeList[i];
                node.Index = i;
                var segmentDistance = i == 0 ? 0f : Vector3.Distance(node.Position, NodeList[i - 1].Position);
                totalDistance += segmentDistance;
                node.Distance = totalDistance;
                node.SegmentDistance = segmentDistance;
            }

            Distance = totalDistance;
        }
    }

    #endregion

    #region Get Pos

    public Vector3 GetPos(float distance)
    {
        return GetInfo(distance).Item1;
    }

    public Vector3 GetPosByFactor(float factor)
    {
        return GetInfoByFactor(factor).Item1;
    }

    public Vector3 GetPosByRemainDistance(float remainDistance)
    {
        return GetInfoByRemainDistance(remainDistance).Item1;
    }

    #endregion

    #region Get Info

    public (Vector3, Vector3) GetInfo(float distance)
    {
        var count = Count;
        while (distance > Distance)
        {
            distance -= Distance;
        }

        if (distance > 1e-6f)
        {
            var dis = 0f;
            for (var i = 0; i < count; i++)
            {
                var current = NodeList[i];
                var next = i < count - 1 ? NodeList[i + 1] : null;
                if (next == null) return (current.Position, current.Forward);
                if (dis + next.SegmentDistance >= distance)
                {
                    dis += next.SegmentDistance;
                    var diff = dis - distance;
                    var factor = ((next.SegmentDistance - diff) / next.SegmentDistance);
                    var pos = Vector3.Lerp(current.Position, next.Position, factor);
                    var forward = Vector3.Lerp(current.Forward, next.Forward, factor);
                    return (pos, forward);
                }
                else
                {
                    dis += next.SegmentDistance;
                }
            }
        }

        var firstNode = NodeList.Count > 0 ? NodeList[0] : null; ;
        if (firstNode == null) return (Vector3.zero, Vector3.forward);
        return (firstNode.Position, firstNode.Forward);
    }

    public (Vector3, Vector3) GetInfoByFactor(float factor)
    {
        var distance = factor * Distance;
        var result = GetInfo(distance);
        return result;
    }

    public (Vector3, Vector3) GetInfoByRemainDistance(float remainDistance)
    {
        var distance = Distance - remainDistance;
        if (distance < 0f) distance = 0f;
        return GetInfo(distance);
    }

    #endregion

    #region Get Factor By Pos

    public float GetFactorByPos(Vector3 position)
    {
        var start = 0;
        var end = 0;
        var count = NodeList.Count;
        var nearestNode = NodeList.Min(n => (n.Position - position).sqrMagnitude);
        if (nearestNode.Index == 0) end = 1;
        else if (nearestNode.Index == count - 1)
        {
            start = count - 2;
            end = count - 1;
        }
        else
        {
            var after = NodeList[nearestNode.Index + 1];
            var before = NodeList[nearestNode.Index - 1];
            var afterDis = (after.Position - position).sqrMagnitude;
            var beforeDis = (before.Position - position).sqrMagnitude;
            if (afterDis < beforeDis)
            {
                start = nearestNode.Index;
                end = nearestNode.Index + 1;
            }
            else
            {
                start = nearestNode.Index - 1;
                end = nearestNode.Index;
            }
        }

        var startNode = NodeList[start];
        var endNode = NodeList[end];
        var startPos = startNode.Position;
        var endPos = endNode.Position;
        var point = MathUtil.ClosestPointOnLine(position, startPos, endPos);
        var dis = startNode.Distance;
        dis += (point - startPos).magnitude;
        var factor = dis / Distance;

        return factor;
    }

    #endregion

    #region Serch Start With Index

    public (Vector3, Vector3, int) GetInfoStartWithIndex(float distance, int index)
    {
        var count = Count;
        if (distance > 1e-6f)
        {
            var dis = NodeList[index].Distance;
            for (var i = index; i < count; i++)
            {
                var current = NodeList[i];
                var next = i < count - 1 ? NodeList[i + 1] : null;
                if (next == null) return (current.Position, current.Forward, i);
                if (dis + next.SegmentDistance >= distance)
                {
                    dis += next.SegmentDistance;
                    var diff = dis - distance;
                    var factor = ((next.SegmentDistance - diff) / next.SegmentDistance);
                    var pos = Vector3.Lerp(current.Position, next.Position, factor);
                    var forward = Vector3.Lerp(current.Forward, next.Forward, factor);
                    return (pos, forward, i);
                }
                else
                {
                    dis += next.SegmentDistance;
                }
            }
        }

        var firstNode = NodeList.Count > 0 ? NodeList[0] : null; ;
        if (firstNode == null) return (Vector3.zero, Vector3.forward, 0);
        return (firstNode.Position, firstNode.Forward, 0);
    }

    #endregion

    public void Clear()
    {
        NodeList.Clear();
        Distance = 0f;
    }
}