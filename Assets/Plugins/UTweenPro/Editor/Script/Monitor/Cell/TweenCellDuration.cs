#if UNITY_EDITOR
using System.Globalization;
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellDuration : GUITableCell<TweenRowData, Tweener, float>
    {
        public override float GetValue()
        {
            return Data.Duration;
        }

        public override void DrawValue()
        {
            EditorGUILayout.LabelField(Data.Duration.ToString(CultureInfo.InvariantCulture), EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Duration;
            var value2 = data2.Duration;
            return value1.CompareTo(value2);
        }
    }
}
#endif