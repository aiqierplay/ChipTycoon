#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenCellID : GUITableCell<TweenRowData, Tweener, int>
    {
        public override int GetValue()
        {
            return Data.InstanceID;
        }

        public override void DrawValue()
        {
            GUILayout.Label(Data.InstanceID.ToString(), EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.InstanceID;
            var value2 = data2.InstanceID;
            return value1.CompareTo(value2);
        }
    }
}
#endif