using System;
using UnityEngine;

public enum MeshOperatorVertexMode
{
    Offset = 0,
    SetValue = 1,
}

[RequireComponent(typeof(MeshFilter))]
public class MeshOperator : EntityBase
{
    public AnimationCurve ChangeNormalHeightCurve = new AnimationCurve(new[] { new Keyframe(0f, 1f), new Keyframe(1f, 0f) });

    [GetComponent, NonSerialized] public MeshFilter MeshFilter;

    [NonSerialized] public Vector3[] CacheVertices;
    [NonSerialized] public Vector3[] CacheNormals;
    [NonSerialized] public int Length;

    [NonSerialized] public Vector3[] OperateVertices;
    [NonSerialized] public Vector3[] OperateNormals;

    public virtual void Init()
    {
        CacheVertices = MeshFilter.mesh.vertices;
        CacheNormals = MeshFilter.mesh.normals;
        Length = CacheVertices.Length;

        OperateVertices = new Vector3[Length];
        OperateNormals = new Vector3[Length];

        Array.Copy(CacheVertices, OperateVertices, Length);
        Array.Copy(CacheNormals, OperateNormals, Length);

        ApplyMesh();
    }

    public virtual void OffsetVertices(int index, Vector3 offset)
    {
        OperateVertices[index] += offset;
    }

    public virtual void ChangeVertices(int index, Vector3 position, Vector3? normal = null)
    {
        OperateVertices[index] = position;
        if (normal != null)
        {
            OperateNormals[index] = normal.Value;
        }
    }

    public virtual void ChangeVerticesHeight(Vector3 center, float radius, float height, MeshOperatorVertexMode operateMode = MeshOperatorVertexMode.Offset)
    {
        var localCenter = MeshFilter.transform.InverseTransformPoint(center);
        var maxDistance = radius * radius;
        for (var i = 0; i < Length; i++)
        {
            var position = CacheVertices[i];
            var normal = CacheNormals[i];
            var distance = (position - localCenter).sqrMagnitude;
            if (distance > maxDistance) continue;
            var factor = distance / maxDistance;
            if (operateMode == MeshOperatorVertexMode.Offset)
            {
                var changeHeight = ChangeNormalHeightCurve.Evaluate(factor) * height;
                var currentPos = OperateVertices[i];
                var currentHeight = currentPos.y - position.y;
                if (currentHeight < changeHeight)
                {
                    var newPosition = position + normal * changeHeight;
                    ChangeVertices(i, newPosition);
                }
            }
            else if (operateMode == MeshOperatorVertexMode.SetValue)
            {
                var currentPos = OperateVertices[i];
                currentPos.y = height;
                ChangeVertices(i, currentPos);
            }
        }
    }

    public virtual void ApplyMesh()
    {
        MeshFilter.mesh.vertices = OperateVertices;
        MeshFilter.mesh.normals = OperateNormals;
    }
}
