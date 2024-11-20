#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Aya.TweenPro
{
    public class TweenCellPlayer : GUITableCell<TweenRowData, Tweener, UTweenPlayer>
    {
        public override UTweenPlayer GetValue()
        {
            return Data.Animation.TweenPlayer;
        }

        public override void DrawValue()
        {
            if (Data.Animation.TweenPlayer != null)
            {
                EditorGUILayout.ObjectField(Data.Animation.TweenPlayer, typeof(UTweenPlayer), false);
            }
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.Animation.TweenPlayer;
            var value2 = data2.Animation.TweenPlayer;
            if (value1 != null && value2 != null)
            {
                return string.Compare(value1.name, value2.name, StringComparison.Ordinal);
            }

            return 0;
        }
    }
}
#endif