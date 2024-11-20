using System;
using System.Collections.Generic;
using Aya.Maths;
using Sirenix.OdinInspector;
using UnityEngine;

public class Navigator : EntityBase
{
    [Title(nameof(Navigator))]
    public int DetectorCount = 8;
    public bool AutoRun = true;
    [SerializeReference] public List<Detector> DetectorList;

    [Title(nameof(Gizmos))]
    public Color DetectorColor = Color.green;
    public Color DirectionColor = Color.red;

    [NonSerialized] public List<DetectorGroup> DetectorGroupList = new List<DetectorGroup>();
    [NonSerialized] public Vector3 TargetValue;
    [NonSerialized] public Vector3 Direction;
    [NonSerialized] public bool Movable;
    [NonSerialized] public bool IsRunning;

    protected override void Awake()
    {
        base.Awake();

        for (var i = 0; i < DetectorCount; i++)
        {
            var direction = MathUtil.Rotate(Vector3.forward, Vector2.zero, Vector3.up, 360f / DetectorCount * i);
            var detectorGroup = new DetectorGroup();
            detectorGroup.Init(this, direction, DetectorList);
            DetectorGroupList.Add(detectorGroup);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Direction = Vector3.zero;
        IsRunning = false;
        if (AutoRun) Enable();
    }

    public void Enable()
    {
        IsRunning = true;
    }

    public void Disable()
    {
        IsRunning = false;
    }

    public virtual void Update()
    {
        if (!IsGaming) return;
        if (IsRunning)
        {
            UpdateImpl();
        }
    }

    public virtual void UpdateImpl()
    {
        var targetValue = Vector3.zero;
        var count = DetectorGroupList.Count;
        for (var i = 0; i < count; i++)
        {
            var detectorGroup = DetectorGroupList[i];
            targetValue += detectorGroup.Direction * detectorGroup.GetValue();
        }

        TargetValue = targetValue;
        Direction = targetValue.normalized;
        Movable = targetValue.sqrMagnitude > 0.1f;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = DetectorColor;
        var count = DetectorGroupList.Count;
        for (var i = 0; i < count; i++)
        {
            var detectorGroup = DetectorGroupList[i];
            Gizmos.DrawLine(Position, Position + detectorGroup.Direction * 5f);
        }

        if (Movable)
        {
            Gizmos.color = DirectionColor;
            Gizmos.DrawLine(Position, Position + TargetValue * 5f);
        }
    }
}
