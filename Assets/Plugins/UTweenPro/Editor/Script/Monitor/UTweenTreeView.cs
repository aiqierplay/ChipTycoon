#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Aya.TweenPro
{
    public class UTweenTreeView : GUITableView<TweenRowData, Tweener>
    {
        public UTweenTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
        }
        
        public override void CreateDataList()
        {
            DataList = new List<TweenRowData>();
            if (Application.isPlaying)
            {
                foreach (var tweenAnimation in UTweenManager.Ins.PlayingList)
                {
                    if (tweenAnimation.ControlMode != TweenControlMode.Component) continue;
                    foreach (var tweener in tweenAnimation.TweenerList)
                    {
                        var id = tweener.InstanceID;
                        var data = new TweenRowData(id, tweener, true);
                        DataList.Add(data);
                    }
                }
            }

            foreach (var kv in UTweenPool.PoolListDic)
            {
                var type = kv.Key;
                if (!type.IsSubclassOf(typeof(Tweener))) continue;
                var pool = kv.Value;
                foreach (var value in pool.SpawnList)
                {
                    var tweener = value as Tweener;
                    if (tweener == null) continue;
                    var id = tweener.InstanceID;
                    var data = new TweenRowData(id, tweener, true);
                    DataList.Add(data);
                }
            }

            foreach (var kv in UTweenPool.PoolListDic)
            {
                var type = kv.Key;
                if (!type.IsSubclassOf(typeof(Tweener))) continue;
                var pool = kv.Value;
                foreach (var value in pool.DeSpawnList)
                {
                    var tweener = value as Tweener;
                    if (tweener == null) continue;
                    var id = tweener.InstanceID;
                    var data = new TweenRowData(id, tweener, false);
                    DataList.Add(data);
                }
            }
        }

        public override int CompareCell(int columnIndex, TweenRowData data1, TweenRowData data2, bool ascending)
        {
            var compareResult = data2.Active.CompareTo(data1.Active);
            if (compareResult == 0) compareResult = data1.CompareCell(columnIndex, data1, data2) * (ascending ? 1 : -1);
            var compare = compareResult;
            return compare;
        }
    }
}
#endif