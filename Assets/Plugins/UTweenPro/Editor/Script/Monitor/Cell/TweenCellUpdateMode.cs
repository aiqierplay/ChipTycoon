#if UNITY_EDITOR
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellUpdateMode : GUITableCell<TweenRowData, Tweener, UpdateMode>
    {
        public override UpdateMode GetValue()
        {
            return Data.Animation.UpdateMode;
        }

        public override void DrawValue()
        {
            EditorGUILayout.LabelField(Data.Animation.UpdateMode.ToString(), EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Animation.UpdateMode;
            var value2 = data2.Animation.UpdateMode;
            return value1.CompareTo(value2);
        }
    }
}
#endif