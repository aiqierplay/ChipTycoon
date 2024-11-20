#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Aya.TweenPro
{
    public abstract class GUITableView<TRowData, TData> : TreeView 
        where TRowData : GUITableRowData<TRowData, TData>
    {
        public List<TRowData> DataList;
        public Dictionary<int, TRowData> DataDic;
        public MultiColumnHeader MultiColumnHeader;

        protected GUITableView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
            MultiColumnHeader = multiColumnHeader;
            MultiColumnHeader.sortingChanged += OnSortingChanged;
            MultiColumnHeader.ResizeToFit();
            MultiColumnHeader.SetSorting(0, true);

            Reload();
        }

        public virtual void CreateDataList()
        {
            DataList = new List<TRowData>();
        }

        protected override TreeViewItem BuildRoot()
        {
            CreateDataList();
            DataDic = new Dictionary<int, TRowData>();
            foreach (var data in DataList)
            {
                DataDic.Add(data.ID, data);
            }

            var columnIndex = multiColumnHeader.sortedColumnIndex;
            if (columnIndex >= 0)
            {
                var ascending = multiColumnHeader.IsSortedAscending(columnIndex);
                DataList.Sort((data1, data2) =>
                {
                    var compare = CompareCell(columnIndex, data1, data2, ascending);
                    return compare;
                });
            }

            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            foreach (var data in DataList)
            {
                var item = new TreeViewItem(data.ID, 0, $"Row_{data.ID}");
                root.AddChild(item);
            }

            return root;
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            var data = DataDic[item.id];
            return data.GetRowHeight();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            if (args.row % 2 == 0)
            {
                EditorGUI.DrawRect(args.rowRect, Color.gray * 0.65f);
            }

            for (var i = 0; i < args.GetNumVisibleColumns(); i++)
            {
                var cellRect = args.GetCellRect(i);
                CenterRectUsingSingleLineHeight(ref cellRect);
                using (GUIArea.Create(cellRect))
                {
                    var item = args.item;
                    var column = args.GetColumn(i);
                    DrawCell(cellRect, item, column, ref args);
                }
            }
        }

        protected virtual void DrawCell(Rect cellRect, TreeViewItem item, int column, ref RowGUIArgs args)
        {
            var data = DataDic[item.id];
            using (GUIHorizontal.Create())
            {
                data.DrawCell(column);
            }
        }

        public virtual void OnSortingChanged(MultiColumnHeader header)
        {
            Reload();
        }

        public virtual int CompareCell(int columnIndex, TRowData data1, TRowData data2, bool ascending)
        {
            var compare = data1.CompareCell(columnIndex, data1, data2) * (ascending ? 1 : -1);
            return compare;
        }
    }
}
#endif