#if UNITY_EDITOR
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellAutoPlayMode : GUITableCell<TweenRowData, Tweener, AutoPlayMode>
    {
        public override AutoPlayMode GetValue()
        {
            return Data.Animation.AutoPlay;
        }

        public override void DrawValue()
        {
            EditorGUILayout.LabelField(Data.Animation.AutoPlay.ToString(), EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Animation.AutoPlay;
            var value2 = data2.Animation.AutoPlay;
            return value1.CompareTo(value2);
        }
    }
}
#endif