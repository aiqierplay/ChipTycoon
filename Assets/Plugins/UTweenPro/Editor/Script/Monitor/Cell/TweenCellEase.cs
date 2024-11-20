#if UNITY_EDITOR
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellEase : GUITableCell<TweenRowData, Tweener, int>
    {
        public override int GetValue()
        {
            return Data.Ease;
        }

        public override void DrawValue()
        {
            EditorGUILayout.LabelField(EaseType.FunctionInfoDic[Data.Ease].DisplayName, EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Ease;
            var value2 = data2.Ease;
            return value1.CompareTo(value2);
        }
    }
}
#endif