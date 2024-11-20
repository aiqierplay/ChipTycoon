using System;
using System.Collections.Generic;
using Aya.Async;
using Aya.Extension;
using Aya.Maths;
using Aya.Test;
using Aya.UI.Markup;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(-102)]
public class DynamicNavMeshBaker : EntityBase
{
    #region Entity Info

    [GetComponentInChildren(true), NonSerialized] public List<MeshFilter> MeshFilters;
    [GetComponentInChildren(true), NonSerialized] public List<Terrain> Terrains;

    protected override void OnEnable()
    {
        base.OnEnable();
        BakeMeshes.AddRange(MeshFilters);
        Terrains.AddRange(Terrains);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var meshFilter in MeshFilters)
        {
            BakeMeshes.Remove(meshFilter);
        }

        foreach (var terrain in Terrains)
        {
            Terrains.Remove(terrain);
        }
    }

    #endregion

    #region NavMeshBuildSource

    public static List<MeshFilter> BakeMeshes = new List<MeshFilter>();
    public static List<Terrain> BakeTerrains = new List<Terrain>();

    public static void Collect(ref List<NavMeshBuildSource> sources)
    {
        sources.Clear();
        var meshesCount = BakeMeshes.Count;
        for (var i = 0; i < meshesCount; ++i)
        {
            var meshFilter = BakeMeshes[i];
            if (meshFilter == null) continue;

            var m = meshFilter.sharedMesh;
            if (m == null) continue;

            var s = new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Mesh,
                sourceObject = m,
                transform = meshFilter.transform.localToWorldMatrix,
                area = 0
            };

            sources.Add(s);
        }

        var terrainsCount = BakeTerrains.Count;
        for (var i = 0; i < terrainsCount; ++i)
        {
            var terrain = BakeTerrains[i];
            if (terrain == null) continue;

            var s = new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Terrain,
                sourceObject = terrain.terrainData,
                transform = Matrix4x4.TRS(terrain.transform.position, Quaternion.identity, Vector3.one),
                area = 0
            };

            sources.Add(s);
        }
    }

    #endregion

    #region Bake

    public static Transform BakeCenter;
    public static Vector3 Size = new Vector3(1000.0f, 100.0f, 1000.0f);

    public static NavMeshData NavMeshData;
    public static NavMeshDataInstance NavMeshDataInstance;

    public static AsyncOperation AsyncBakeOperation;
    public static List<NavMeshBuildSource> NavMeshBakeSources = new List<NavMeshBuildSource>();

    public static NavMeshTriangulation NavMeshTriangulation;
    public static List<Vector3> Points;

    public static int PointCount => NavMeshTriangulation.indices.Length;
    public static bool IsBaking { private set; get; }

    public static void Bake(bool async = false, Action onDone = null)
    {
        NavMeshData = new NavMeshData();
        NavMeshDataInstance = NavMesh.AddNavMeshData(NavMeshData);
        BakeImpl(async, onDone);
    }

    protected static void BakeImpl(bool async = false, Action onDone = null)
    {
        Collect(ref NavMeshBakeSources);
        var defaultBuildSettings = NavMesh.GetSettingsByID(0);
        var bounds = QuantizedBounds();
        IsBaking = true;
        if (async)
        {
            var startTime = Time.realtimeSinceStartup;
            AsyncBakeOperation = NavMeshBuilder.UpdateNavMeshDataAsync(NavMeshData, defaultBuildSettings, NavMeshBakeSources, bounds);
            Delay.Ins.ExecuteWhen(() =>
            {
                OnBakeDone();
                onDone?.Invoke();
                IsBaking = false;

                var endTime = Time.realtimeSinceStartup;
                var time = (endTime - startTime) * 1000f;
                Debug.Log("[Bake Async]".ToMarkup(Color.cyan) + " Complete " + time + "ms");
            }, () => AsyncBakeOperation.isDone);
        }
        else
        {
            var time = StopwatchUtil.TestOnce(() =>
            {
                NavMeshBuilder.UpdateNavMeshData(NavMeshData, defaultBuildSettings, NavMeshBakeSources, bounds);
                OnBakeDone();
                onDone?.Invoke();
            });

            IsBaking = false;
            Debug.Log("[Bake]".ToMarkup(Color.cyan) + " Complete " + time + "ms");
        }
    }

    protected static void OnBakeDone()
    {
        NavMeshTriangulation = NavMesh.CalculateTriangulation();
        Points = new List<Vector3>();
        for (var i = 0; i < PointCount; i++)
        {
            var point = GetPointByIndex(i);
            Points.Add(point);
        }
    }

    public static Vector3 Quantize(Vector3 vector, Vector3 quantize)
    {
        var x = quantize.x * Mathf.Floor(vector.x / quantize.x);
        var y = quantize.y * Mathf.Floor(vector.y / quantize.y);
        var z = quantize.z * Mathf.Floor(vector.z / quantize.z);
        return new Vector3(x, y, z);
    }

    public static Bounds QuantizedBounds()
    {
        var center = BakeCenter ? BakeCenter.position : Vector3.zero;
        return new Bounds(Quantize(center, .001f * Size), Size);
    }

    #endregion

    #region Get Data

    public static Vector3 GetPointByIndex(int index)
    {
        index = Mathf.Clamp(index, 0, PointCount - 1);
        var result = NavMeshTriangulation.vertices[NavMeshTriangulation.indices[index]];
        return result;
    }

    public static Vector3 GetRandPosByTriangleIndex(int index)
    {
        var p1 = Points[index];
        var p2 = Points[index + 1];
        var p3 = Points[index + 2];
        var result = MathUtil.GetRandPosInTriangle(p1, p2, p3);
        return result;
    }

    public static Vector3 GetRandomPos()
    {
        var index = UnityEngine.Random.Range(0, NavMeshTriangulation.indices.Length - 3);
        var result = GetRandPosByTriangleIndex(index);
        return result;
    }

    public static Vector3 GetNearRandomPos(Vector3 position, int findCount = 20)
    {
        var minPoints = Points.Min(p => (p - position).sqrMagnitude, findCount);
        var index = UnityEngine.Random.Range(0, minPoints.Count - 3);
        var p1 = minPoints[index];
        var p2 = minPoints[index + 1];
        var p3 = minPoints[index + 2];
        var result = MathUtil.GetRandPosInTriangle(p1, p2, p3);
        return result;
    }

    #endregion
}
