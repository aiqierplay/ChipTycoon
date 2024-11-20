using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.TweenPro;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;

public class StackList : EntityBase
{
    [FoldoutGroup("Render")] public EntityBase Prefab;
    [FoldoutGroup("Render")] public Vector3 Size;
    [FoldoutGroup("Render")] public Vector2Int Grid = new Vector2Int(1, 1);
    [FoldoutGroup("Render")] public Vector3 Scale = Vector3.one;
    [FoldoutGroup("Render")] public Vector3 Rotate = Vector3.zero;
    [FoldoutGroup("Render")] public StackListItemOffsetData OffsetInterval;

    [FoldoutGroup("Rand")] public Vector3 RandPosFrom;
    [FoldoutGroup("Rand")] public Vector3 RandPosTo;
    [FoldoutGroup("Rand")] public Vector3 RandAngleFrom;
    [FoldoutGroup("Rand")] public Vector3 RandAngleTo;
    [FoldoutGroup("Rand")] public Vector2 RandScale;

    [FoldoutGroup("Animation")] public float ParabolaHeight = 2f;
    [FoldoutGroup("Animation")] public float ParabolaDuration = 0.5f;

    #region Inertia

    [FoldoutGroup("Inertia")] public bool EnableInertia;
    [FoldoutGroup("Inertia")] public AnimationCurve OffsetCurve;
    [FoldoutGroup("Inertia")] public float MaxOffset;
    [FoldoutGroup("Inertia")] public AnimationCurve HeightCurve;
    [FoldoutGroup("Inertia")] public float MaxHeight;
    [FoldoutGroup("Inertia")] public AnimationCurve RotateCurve;
    [FoldoutGroup("Inertia")] public float MaxRotate;
    [FoldoutGroup("Inertia")] public float MaxDistance;
    [FoldoutGroup("Inertia")] public int EffectFrame;
    [FoldoutGroup("Inertia")] public float EffectSpeed;
    [FoldoutGroup("Inertia")] public float SpreadForce;

    private readonly List<Vector3> _positionList = new List<Vector3>();
    private Vector3 _currentDirection;
    private Vector3 _centerOffset;

    #endregion

    public bool IsEmpty => Count == 0;

    private float _sizeX;
    private float _sizeY;
    private float _sizeZ;
    private int _gridX;
    private int _gridY;

    [NonSerialized] public List<EntityBase> List = new List<EntityBase>();
    [NonSerialized] public List<StackListItemData> PointDataList = new List<StackListItemData>();

    public int Count => List.Count;

    public bool Active { get; set; }
    public float Height { get; set; }

    public void Init()
    {
        foreach (var entity in List)
        {
            GamePool.DeSpawn(entity);
        }

        List.Clear();
        PointDataList.Clear();

        Active = true;

        CacheSizeInfo();
    }

    private bool _cachedSizeInfo = false;
    private void CacheSizeInfo()
    {
        if (_cachedSizeInfo) return;
        _cachedSizeInfo = true;

        _gridX = Grid.x;
        _gridY = Grid.y;

        _sizeX = Size.x;
        _sizeY = Size.y;
        _sizeZ = Size.z;

        var offsetX = (-_gridY / 2f + 0.5f) * _sizeZ;
        var offsetZ = (-_gridX / 2f + 0.5f) * _sizeX;
        _centerOffset = new Vector3(offsetX, 0f, offsetZ);
    }

    public void OnValidate()
    {
        _cachedSizeInfo = false;
        CacheSizeInfo();
    }

    public Vector3 GetItemPosition(int index)
    {
        CacheSizeInfo();
        var position = GetItemPosition(index, Grid, Size) + _centerOffset;
        return position;
    }

    public static Vector3 GetItemPosition(int index, Vector2 gridSize, Vector3 cellSize)
    {
        var layerCount = Mathf.RoundToInt(gridSize.x * gridSize.y);
        var layer = index / layerCount;
        var num = index % layerCount;

        var row = Mathf.FloorToInt(num / gridSize.x);
        var column = Mathf.FloorToInt(num - row * gridSize.x);
        var position = new Vector3(row * cellSize.x, layer * cellSize.y, column * cellSize.z);
        return position;
    }

    public StackListItemData GetItemPointData(int index)
    {
        while (index >= PointDataList.Count)
        {
            var data = new StackListItemData();
            var position = GetItemPosition(PointDataList.Count) + RandUtil.RandVector3(RandPosFrom, RandPosTo);
            var eulerAngle = Rotate + RandUtil.RandVector3(RandAngleFrom, RandAngleTo);
            var scale = Scale + Vector3.one * RandUtil.RandFloat(RandScale.x, RandScale.y);
            data.Position = position + OffsetInterval.Position * index;
            data.EulerAngle = eulerAngle + OffsetInterval.EulerAngles * index;
            data.Scale = scale + OffsetInterval.Scale * index;
            PointDataList.Add(data);
        }

        var result = PointDataList[index];
        return result;
    }

    #region Add

    public void AddParabola(EntityBase instance, Action onDone = null)
    {
        AddParabola(instance, ParabolaHeight, ParabolaDuration, onDone);
    }

    public void AddParabola(EntityBase instance, float height, float duration, Action onDone = null)
    {
        var startPos = instance.Position;
        var startEulerAngle = instance.LocalEulerAngles;
        var startScale = instance.LocalScale;
        Add(instance, true);
        var data = GetItemPointData(List.Count - 1);
        // startPos = WorldToLocalPosition(startPos);
        var endEulerAngle = data.EulerAngle;
        var endScale = data.Scale;
        var tweenDuration = duration * RandUtil.RandFloat(0.8f, 1.1f);
        var tweenParabola = UTween.Value(0f, 1f, duration, value =>
        {
            var endPos = LocalToWorldPosition(data.Position);
            var localPos = TweenParabola.GetPositionByFactor(startPos, endPos, height, value);
            instance.Position = localPos;
        })
            .SetUpdateMode(UpdateMode.LateUpdate)
            .SetOnStop(() =>
        {
            onDone?.Invoke();
        });

        var tweenScale = UTween.Scale(instance.Trans, startScale, endScale, tweenDuration);
        var tweenRotate = UTween.LocalEulerAngles(instance.Trans, startEulerAngle, endEulerAngle, tweenDuration);
    }

    public void Add(int count, bool refreshPos)
    {
        for (var i = 0; i < count; i++)
        {
            var instance = GamePool.Spawn(Prefab, Trans);
            Add(instance, refreshPos);
        }
    }

    public void Add(EntityBase instance, bool refreshPos = true)
    {
        instance.Parent = Trans;
        instance.LocalScale = Scale;
        if (instance.Rigidbody != null) instance.Rigidbody.isKinematic = true;
        List.Add(instance);
        if (refreshPos) RefreshPos();
    }

    #endregion

    #region Remove

    public void Remove(int count = 1, bool deSpawn = true)
    {
        for (var i = 0; i < count; i++)
        {
            var instance = List.Last();
            if (instance == null) return;
            List.Remove(instance);
            if (deSpawn) GamePool.DeSpawn(instance);
        }
    }

    public void Remove(EntityBase instance, bool deSpawn = true)
    {
        if (instance == null) return;
        if (!List.Contains(instance)) return;
        List.Remove(instance);
        if (deSpawn) GamePool.DeSpawn(instance);
    }

    #endregion

    public EntityBase Pop()
    {
        if (IsEmpty) return default;
        var index = List.Count - 1;
        var result = List[index];
        List.Remove(result);
        return result;
    }

    public void Clear(bool deSpawn = true)
    {
        var count = List.Count;
        for (var i = 0; i < count; i++)
        {
            var item = List[i];
            if (deSpawn) GamePool.DeSpawn(item);
        }

        List.Clear();
    }

    public void Update()
    {
        if (!Active) return;
        var position = Position;
        _positionList.Add(position);

        while (_positionList.Count > EffectFrame)
        {
            _positionList.RemoveAt(0);
        }

        RefreshPos();
    }

    public void RefreshPos()
    {
        var direction = _positionList.Count > 0 ? (_positionList[0] - _positionList[_positionList.Count - 1]) : Forward;
        _currentDirection = Vector3.Lerp(_currentDirection, direction, DeltaTime * EffectSpeed);
        var normalizedDirection = _currentDirection.normalized;
        var factor = _currentDirection.magnitude * 1f / MaxDistance;
        factor = Mathf.Clamp01(factor);

        var count = List.Count;
        var index = 0;

        for (var i = 0; i < count; i++)
        {
            var item = List[i];
            var data = GetItemPointData(index);
            if (EnableInertia)
            {
                var heightFactor = index * 1f / count;
                var curveFactor = heightFactor * factor;
                var height = HeightCurve.Evaluate(curveFactor) * MaxHeight;
                var offset = OffsetCurve.Evaluate(curveFactor) * MaxOffset * normalizedDirection + Vector3.down * height;

                var angle = new Vector3(normalizedDirection.z, 0f, -normalizedDirection.x);
                var rotate = RotateCurve.Evaluate(curveFactor) * MaxRotate * angle;

                item.Trans.position = Trans.TransformPoint(data.Position) + offset;
                item.Trans.localEulerAngles = data.EulerAngle + rotate;
            }
            else
            {
                item.Trans.localPosition = data.Position;
                item.Trans.localEulerAngles = data.EulerAngle;
            }

            index++;
        }

        Height = count > 0 ? (List.Last().Trans.position.y - Trans.position.y) : 0f;
    }

    public void Spread()
    {
        Active = false;
        foreach (var item in List)
        {
            item.Rigidbody.isKinematic = false;
            item.Rigidbody.AddExplosionForce(RandUtil.RandFloat(Mathf.Sqrt(SpreadForce), SpreadForce), item.Position + RandUtil.RandVector3(-1, 1f), 5f);
        }
    }

    [FoldoutGroup("Gizmos")] public bool ShowGizmos = true;
    [FoldoutGroup("Gizmos")] public Color GizmosColor = Color.green;
    [FoldoutGroup("Gizmos")] public int GizmosPreviewCount = 10;
    // [FoldoutGroup("Gizmos")] public Vector3 GizmosCellSize;

    public virtual void OnDrawGizmos()
    {
        if (!ShowGizmos) return;
        for (var index = 0; index < GizmosPreviewCount; index++)
        {
            var position = GetItemPosition(index) + OffsetInterval.Position * index;
            var eulerAngles = Rotate +OffsetInterval.EulerAngles * index;
            var scale = Scale + OffsetInterval.Scale * index;

            Gizmos.matrix = CalculateLocalToWorldMatrix(position, eulerAngles, scale);
            Gizmos.color = GizmosColor * 0.25f;
            Gizmos.DrawCube(position, Size);

            Gizmos.color = GizmosColor;
            Gizmos.DrawWireCube(position, Size);
        }
    }

    public  Matrix4x4 CalculateLocalToWorldMatrix(Vector3 position, Vector3 eulerAngles, Vector3 scale)
    {
        var translationMatrix = transform.localToWorldMatrix;
        var rotation = Quaternion.Euler(eulerAngles);
        var rotationMatrix = Matrix4x4.Rotate(rotation);
        var scaleMatrix = Matrix4x4.Scale(scale);
        var localToWorldMatrix = translationMatrix * rotationMatrix * scaleMatrix;
        return localToWorldMatrix;
    }
}
