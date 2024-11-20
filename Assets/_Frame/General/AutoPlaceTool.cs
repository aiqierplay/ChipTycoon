using System.Collections.Generic;
using Aya.Extension;
using Aya.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum AutoPlaceShapeMode
{
    [LabelText(" Square", SdfIconType.Square)]
    Square = 0,
    [LabelText(" Hex", SdfIconType.Hexagon)]
    Hex = 1,
}

public enum AutoPlaceCenterMode
{
    Position = 1,
    WorldZero = 2,
}

public class AutoPlaceTool : MonoBehaviour
{
    [BoxGroup("Place Param")]
    [TypeReference(typeof(EntityBase))]
    public TypeReference TypeFilter;

    [BoxGroup("Place Param")]
    [EnumToggleButtons]
    public AutoPlaceShapeMode ShapeMode = AutoPlaceShapeMode.Square;

    [BoxGroup("Place Param")]
    [EnumToggleButtons]
    public AutoPlaceCenterMode CenterMode = AutoPlaceCenterMode.WorldZero;

    [BoxGroup("Place Param")] public Transform CellTrans;
    [BoxGroup("Place Param")] public Vector2Int MapSize = new Vector2Int(10, 10);
    [BoxGroup("Place Param")] public Vector2 CellSize = Vector2Int.one;
    [BoxGroup("Place Param")] public string CellNameFormat = "{index} - ({x},{y})";

    [BoxGroup("Place Param")] public bool AutoSetParameter = true;
    [BoxGroup("Place Param"), ShowIf(nameof(AutoSetParameter))] public string CellXParameter = "X";
    [BoxGroup("Place Param"), ShowIf(nameof(AutoSetParameter))] public string CellYParameter = "Y";

    #region Border

    [TabGroup(TabPlaceGroup, TabBorder, SdfIconType.Square, TextColor = "white")]
    // [ShowIf(nameof(ShapeMode), AutoPlaceShapeMode.Square)]
    [Button(SdfIconType.Square, IconAlignment.LeftEdge)]
    public void PlaceBorder()
    {
        ActionWithUndo(() =>
        {
            var transList = GetChildList();
            var index = 0;
            for (var y = MapSize.y - 1; y >= 0; y--)
            {
                for (var x = 0; x < MapSize.x; x++)
                {
                    if (index >= transList.Count) return;
                    if (y > 0 && y < MapSize.y - 1 && x > 0 && x < MapSize.x - 1) continue;
                    var trans = transList[index];
                    SetCell(trans, index, x, y);
                    index++;
                }
            }
        });
    }

    [TabGroup(TabPlaceGroup, TabBorder)]
    // [ShowIf(nameof(ShapeMode), AutoPlaceShapeMode.Square)]
    [Button(SdfIconType.Arrow90degRight, IconAlignment.LeftEdge)]
    public void PlaceBorderClockwise()
    {
        ActionWithUndo(() =>
        {
            var transList = GetChildList();
            var index = 0;
            for (var x = 0; x < MapSize.x; x++)
            {
                if (index >= transList.Count) return;
                var trans = transList[index];
                SetCell(trans, index, x, MapSize.y - 1);
                index++;
            }

            for (var y = MapSize.y - 1 - 1; y >= 1; y--)
            {
                if (index >= transList.Count) return;
                var trans = transList[index];
                trans.name = GetCellName(index, MapSize.x - 1, y);
                trans.position = GetCellPosition(MapSize.x - 1, y);
                SetCell(trans, index, MapSize.x - 1, y);
                index++;
            }

            for (var x = MapSize.x - 1; x >= 0; x--)
            {
                if (index >= transList.Count) return;
                var trans = transList[index];
                SetCell(trans, index, x, 0);
                index++;
            }

            for (var y = 1; y < MapSize.y - 1; y++)
            {
                if (index >= transList.Count) return;
                var trans = transList[index];
                SetCell(trans, index, 0, y);
                index++;
            }
        });
    }

    [TabGroup(TabPlaceGroup, TabBorder)]
    // [ShowIf(nameof(ShapeMode), AutoPlaceShapeMode.Square)]
    [Button(SdfIconType.Arrow90degLeft, IconAlignment.LeftEdge)]
    public void PlaceBorderAntiClockwise()
    {
        ActionWithUndo(() =>
        {
            var transList = GetChildList();
            var index = 0;
            for (var x = 0; x < MapSize.x; x++)
            {
                if (index >= transList.Count) return;
                var trans = transList[index];
                SetCell(trans, index, x, 0);
                index++;
            }

            for (var y = 1; y < MapSize.y - 1; y++)
            {
                if (index >= transList.Count) return;
                var trans = transList[index];
                SetCell(trans, index, MapSize.x - 1, y);
                index++;
            }

            for (var x = MapSize.x - 1; x >= 0; x--)
            {
                if (index >= transList.Count) return;
                var trans = transList[index];
                SetCell(trans, index, x, MapSize.y - 1);
                index++;
            }

            for (var y = MapSize.y - 1 - 1; y >= 1; y--)
            {
                if (index >= transList.Count) return;
                var trans = transList[index];
                SetCell(trans, index, 0, y);
                index++;
            }
        });
    }

    #endregion

    #region Grid

    [TabGroup(TabPlaceGroup, TabGrid, SdfIconType.Grid3x3, TextColor = "white")]
    [Button(SdfIconType.GripHorizontal, IconAlignment.LeftEdge)]
    public void PlaceGridRow()
    {
        ActionWithUndo(() =>
        {
            var transList = GetChildList();
            var index = 0;
            for (var y = MapSize.y - 1; y >= 0; y--)
            {
                for (var x = 0; x < MapSize.x; x++)
                {
                    if (index >= transList.Count) return;
                    var trans = transList[index];
                    SetCell(trans, index, x, y);
                    index++;
                }
            }
        });
    }

    [TabGroup(TabPlaceGroup, TabGrid)]
    [Button(SdfIconType.GripVertical, IconAlignment.LeftEdge)]
    public void PlaceGridColumn()
    {
        ActionWithUndo(() =>
        {
            var transList = GetChildList();
            var index = 0;
            for (var x = 0; x < MapSize.x; x++)
            {
                for (var y = MapSize.y - 1; y >= 0; y--)
                {
                    if (index >= transList.Count) return;
                    var trans = transList[index];
                    SetCell(trans, index, x, y);
                    index++;
                }
            }
        });
    }

    #endregion

    #region Operate

    [TabGroup(TabOperateGroup, TabOperate, SdfIconType.Gear, TextColor = "white")]
    [Button(SdfIconType.Grid, IconAlignment.LeftEdge)]
    public void SnapToGrid()
    {
        ActionWithUndo(() =>
        {
            var transList = GetChildList();
            for (var i = 0; i < transList.Count; i++)
            {
                var trans = transList[i];
                var pos = trans.position;
                var (snapPos, gridPos) = SnapTo(pos);
                SetCell(trans, i, gridPos.x, gridPos.y);
            }
        });
    }

    #endregion

    #region Util

    public List<Transform> GetChildList()
    {
        if (CellTrans == null) CellTrans = transform;
        if (TypeFilter.Type != null)
        {
            var childList = CellTrans.GetComponentsInChildren(TypeFilter).Select(t => t.transform).ToList();
            return childList;
        }
        else
        {
            var childList = CellTrans.GetAllChild<Transform>();
            return childList;
        }
    }

    public void SetCell(Transform trans, int index, int x, int y)
    {
        trans.name = GetCellName(index, x, y);
        trans.position = GetCellPosition(x, y);
        if (AutoSetParameter)
        {
            var component = trans.GetComponent(TypeFilter);
            if (component == null) return;
            component.SetField(CellXParameter, x);
            component.SetField(CellYParameter, y);
        }
    }

    public string GetCellName(int index, int x, int y)
    {
        index += 1;
        var format = CellNameFormat;
        format = format.Replace("index", "0")
            .Replace("x", "1")
            .Replace("y", "2");
        var cellName = string.Format(format, index, x, y);
        return cellName;
    }

    public Vector3 GetCenterPosition()
    {
        var centerPos = Vector3.zero;
        switch (CenterMode)
        {
            case AutoPlaceCenterMode.Position:
                centerPos = transform.position;
                break;
            case AutoPlaceCenterMode.WorldZero:
                centerPos = Vector3.zero;
                break;
        }

        return centerPos;
    }

    public Vector3 GetCellPosition(int x, int y)
    {
        var centerPos = GetCenterPosition();
        var offsetX = -MapSize.x * 1f / 2 + 0.5f;
        var offsetY = -MapSize.y * 1f / 2 + 0.5f;
        switch (ShapeMode)
        {
            case AutoPlaceShapeMode.Square:
                break;
            case AutoPlaceShapeMode.Hex:
                var hexOuterDis = CellSize.y / 2f * Mathf.Tan(45 * Mathf.Deg2Rad);
                if (y % 2 == 0)
                {
                    offsetX += CellSize.x / 2f;
                }

                offsetX -= hexOuterDis;
                offsetY -= hexOuterDis * (y - MapSize.y * 1f / 2 + 0.5f) / 2f / 2f;
                break;
        }

        var posX = centerPos.x + (offsetX + x) * 1f * CellSize.x;
        var posZ = centerPos.z + (offsetY + y) * 1f * CellSize.y;
        var pos = new Vector3(posX, 0, posZ);
        return pos;
    }

    public (Vector3, Vector2Int) SnapTo(Vector3 position)
    {
        var result = (new Vector3(), new Vector2Int(0, 0));
        var minDis = float.MaxValue;
        for (var x = 0; x < MapSize.x; x++)
        {
            for (var y = 0; y < MapSize.y; y++)
            {
                var pos = GetCellPosition(x, y);
                var dis = (pos - position).sqrMagnitude;
                if (!(dis < minDis)) continue;
                result.Item1 = pos;
                result.Item2 = new Vector2Int(x, y);
                minDis = dis;
            }
        }

        return result;
    }

    #endregion

    public void Reset()
    {
        if (CellTrans == null)
        {
            CellTrans = transform;
        }
    }

    public const string TabPlaceGroup = "Place";
    public const string TabOperateGroup = "Operate";
    public const string TabBorder = " Border";
    public const string TabGrid = " Grid";
    public const string TabOperate = " Operate";

    public void ActionWithUndo(Action action)
    {
#if UNITY_EDITOR
        Undo.RecordObject(gameObject, "Place");
#endif
        action?.Invoke();
#if UNITY_EDITOR
        EditorUtility.SetDirty(gameObject);
#endif
    }

#if UNITY_EDITOR

    [BoxGroup("Gizmos")]
    [PropertyOrder(9999)]
    public Color GizmosColor = new Color(0f, 1f, 0f, 0.5f);

    public void OnDrawGizmos()
    {
        for (var x = 0; x < MapSize.x; x++)
        {
            for (var y = 0; y < MapSize.y; y++)
            {
                var pos = GetCellPosition(x, y);
                Gizmos.color = GizmosColor * 0.25f;
                Gizmos.DrawCube(pos, new Vector3(CellSize.x, 0, CellSize.y));

                Gizmos.color = GizmosColor;
                Gizmos.DrawWireCube(pos, new Vector3(CellSize.x, 0, CellSize.y));
            }
        }
    }
#endif
}