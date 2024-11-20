#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Aya.TweenPro
{
    public abstract class GUITableRowData<TRowData, TData>
        where TRowData : GUITableRowData<TRowData, TData>
    {
        public static List<GUITableColumnAttribute> ColumnAttributeList;
        public List<GUITableCell<TRowData, TData>> CellList;

        public TData Data;
        public int ID;

        protected GUITableRowData(int id, TData data)
        {
            ID = id;
            Data = data;
            CacheCellList();
        }

        public virtual void CacheCellList()
        {
            CellList = new List<GUITableCell<TRowData, TData>>();
            foreach (var columnAttribute in ColumnAttributeList)
            {
                var cellType = columnAttribute.FieldInfo.FieldType;
                var cell = Activator.CreateInstance(cellType) as GUITableCell<TRowData, TData>;
                var method = cellType.GetMethod("Init");
                method?.Invoke(cell, new object[] {this});
                columnAttribute.FieldInfo.SetValue(this, cell);
                CellList.Add(cell);
            }
        }

        public virtual void DrawCell(int columnIndex)
        {
            var cell = CellList[columnIndex];
            cell.DrawValue();
        }

        public virtual int CompareCell(int columnIndex, TRowData rowData1, TRowData rowData2)
        {
            var cell = CellList[columnIndex];
            var compare = cell.CompareValue(rowData1.Data, rowData2.Data);
            return compare;
        }

        public static MultiColumnHeaderState CreateMultiColumnHeaderState()
        {
            var dataType = typeof(TRowData);
            if (ColumnAttributeList == null)
            {
                var fields = dataType.GetFields();
                ColumnAttributeList = new List<GUITableColumnAttribute>();
                foreach (var fieldInfo in fields)
                {
                    var columnAttribute = fieldInfo.GetCustomAttribute<GUITableColumnAttribute>();
                    if (columnAttribute != null)
                    {
                        columnAttribute.FieldInfo = fieldInfo;
                        ColumnAttributeList.Add(columnAttribute);
                    }
                }

                ColumnAttributeList.Sort((c1, c2) => c1.Index.CompareTo(c2.Index));
            }

            var columns = new MultiColumnHeaderState.Column[ColumnAttributeList.Count];
            for (var i = 0; i < ColumnAttributeList.Count; i++)
            {
                var columnAttribute = ColumnAttributeList[i];
                columns[i] = columnAttribute.GetColumn();
            }

            var state = new MultiColumnHeaderState(columns);
            return state;
        }

        public virtual float GetRowHeight()
        {
            return EditorGUIUtility.singleLineHeight * 1.2f;
        }
    }
}
#endif