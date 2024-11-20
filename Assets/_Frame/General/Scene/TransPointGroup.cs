using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Aya.Extension;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TransPointGroup : EntityBase
{
    [BoxGroup("Point")] public Transform PointTrans;
    [BoxGroup("Point")] public bool AutoRename = true;
    [BoxGroup("Point")] public List<Transform> PointList;

    [BoxGroup("Gizmos")] public bool DrawGizmos = true;
    [BoxGroup("Gizmos")] public Color GizmosColor = Color.green;
    [BoxGroup("Gizmos")] public float GizmosSize = 1f;


    [BoxGroup("Point")]
    [ButtonGroup("Point/Operate")]
    [Button("Auto Cache")]
    public void AutoCache()
    {
        PointList = PointTrans.GetComponentsInChildren<Transform>().FindAll(t => t != PointTrans);
        for (var i = 0; i < PointList.Count; i++)
        {
            var point = PointList[i];
            if (AutoRename)
            {
                point.name = "Point_" + (i + 1).ToString("D2");
            }
        }
    }


    [BoxGroup("Point")]
    [ButtonGroup("Point/Operate")]
    [Button("Clear"), GUIColor(1f, 0.5f, 0.5f)]
    public void Clear()
    {
        for (var i = 0; i < PointList.Count; i++)
        {
            var point = PointList[i];
            if (point == null) continue;
            if (Application.isPlaying) Destroy(point.gameObject);
            else
            {
#if UNITY_EDITOR
                Undo.DestroyObjectImmediate(point.gameObject);
#endif
            }
        }

        PointList.Clear();
    }

    [BoxGroup("Copy")]
    public Transform CopyTrans;

    [BoxGroup("Copy")]
    [Button("Copy")]
    public void CopyFormOtherTrans()
    {
        AutoCache();
        CopyFormOtherTrans(CopyTrans);
    }

    public void CopyFormOtherTrans(Transform otherTrans)
    {
        if (otherTrans == null) return;
        if (PointList == null) return;
        var transList = CopyTrans.GetAllChild<Transform>();
        for (var i = 0; i < PointList.Count && i < transList.Count; i++)
        {
            var point = PointList[i];
            var trans = transList[i];
            point.CopyTrans(trans, true);
        }
    }


    public int Count => PointList.Count;

    public Vector3 this[int index]
    {
        get
        {
            if (Count == 0) return default;
            index = Mathf.Clamp(index, 0, Count - 1);
            return PointList[index].position;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (PointList == null || PointList.Count == 0)
        {
            AutoCache();
        }
    }

    #region Get Position

    public virtual Vector3 GetRandPosition()
    {
        return PointList.Random().position;
    }

    public virtual List<Vector3> GetRandPositions(int count)
    {
        count = Mathf.Clamp(count, 0, PointList.Count);
        return PointList.Random(count).ToList(t => t.position);
    }

    public virtual List<Vector3> GetPositionList()
    {
        return PointList.Select(t => t.position).ToList();
    }

    public virtual Vector3 GetNearestRandomPosition(Vector3 position, int count = 1)
    {
        return PointList.Min(t => (t.position - position).sqrMagnitude, count).Random().position;
    }

    #endregion

    #region Get Point

    public virtual Transform GetRandPoint()
    {
        return PointList.Random();
    }

    public virtual List<Transform> GetRandPoints(int count)
    {
        count = Mathf.Clamp(count, 0, PointList.Count);
        return PointList.Random(count).ToList();
    }

    public virtual List<Transform> GetPointList()
    {
        return PointList;
    }

    public virtual Transform GetNearestRandomPoint(Vector3 position, int count = 1)
    {
        return PointList.Min(t => (t.position - position).sqrMagnitude, count).Random();
    }

    #endregion

    public virtual void Reset()
    {
        PointTrans = transform;
    }

    public virtual void OnDrawGizmos()
    {
        if (!DrawGizmos) return;
        if (PointTrans == null) return;

        for (var i = 0; i < PointTrans.childCount; i++)
        {
            var point = PointTrans.GetChild(i);
            Gizmos.color = GizmosColor;
            Gizmos.DrawWireCube(point.position, Vector3.one * GizmosSize);

            Gizmos.color = GizmosColor * 0.35f;
            Gizmos.DrawCube(point.position, Vector3.one * GizmosSize);
        }
    }
}
