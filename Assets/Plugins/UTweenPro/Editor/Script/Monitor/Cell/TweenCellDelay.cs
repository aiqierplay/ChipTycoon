#if UNITY_EDITOR
using System.Globalization;
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellDelay : GUITableCell<TweenRowData, Tweener, float>
    {
        public override float GetValue()
        {
            return Data.Delay;
        }

        public override void DrawValue()
        {
            EditorGUILayout.LabelField(Data.Delay.ToString(CultureInfo.InvariantCulture), EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Delay;
            var value2 = data2.Delay;
            return value1.CompareTo(value2);
        }
    }
}
#endif