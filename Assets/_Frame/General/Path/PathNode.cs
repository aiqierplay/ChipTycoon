using System;
using UnityEngine;

[Serializable]
public class PathNode
{
    public int Index;

    public Vector3 Position;
    public Vector3 Forward;

    public float SegmentDistance;
    public float Distance;

    public float Width;
    public Vector3 Up;
    public Vector3 Left;
    public Vector3 Right;
}