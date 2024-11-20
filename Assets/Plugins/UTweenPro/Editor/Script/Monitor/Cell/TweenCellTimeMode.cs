#if UNITY_EDITOR
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellTimeMode : GUITableCell<TweenRowData, Tweener, TimeMode>
    {
        public override TimeMode GetValue()
        {
            return Data.Animation.TimeMode;
        }

        public override void DrawValue()
        {
            EditorGUILayout.LabelField(Data.Animation.TimeMode.ToString(), EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Animation.TimeMode;
            var value2 = data2.Animation.TimeMode;
            return value1.CompareTo(value2);
        }
    }
}
#endif