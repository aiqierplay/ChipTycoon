#if UNITY_EDITOR
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellPreSampleMode : GUITableCell<TweenRowData, Tweener, PrepareSampleMode>
    {
        public override PrepareSampleMode GetValue()
        {
            return Data.Animation.PrepareSampleMode;
        }

        public override void DrawValue()
        {
            EditorGUILayout.LabelField(Data.Animation.PrepareSampleMode.ToString(), EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Animation.PrepareSampleMode;
            var value2 = data2.Animation.PrepareSampleMode;
            return value1.CompareTo(value2);
        }
    }
}
#endif