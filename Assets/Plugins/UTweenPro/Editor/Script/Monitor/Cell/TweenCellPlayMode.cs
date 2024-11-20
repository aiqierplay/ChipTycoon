#if UNITY_EDITOR
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellPlayMode : GUITableCell<TweenRowData, Tweener, PlayMode>
    {
        public override PlayMode GetValue()
        {
            return Data.Animation.PlayMode;
        }

        public override void DrawValue()
        {
            EditorGUILayout.LabelField(Data.Animation.PlayMode.ToString(), EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Animation.PlayMode;
            var value2 = data2.Animation.PlayMode;
            return value1.CompareTo(value2);
        }
    }
}
#endif