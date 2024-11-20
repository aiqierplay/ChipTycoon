#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public class TweenCellType : GUITableCell<TweenRowData, Tweener, Type>
    {
        public override Type GetValue()
        {
            return Data.GetType();
        }

        public override void DrawValue()
        {
            var tweenerType = Data.GetType();
            var icon = EditorIcon.GetTweenerIcon(tweenerType);
            if (icon != null)
            {
                var size = EditorStyle.TweenerHeaderIconSize;
                GUILayout.Label("", EditorStyles.label, GUILayout.Width(size), GUILayout.Height(size));
                var rect = GUILayoutUtility.GetLastRect();
                GUI.DrawTexture(rect, icon);
            }

            string displayName;
            if (TypeCaches.TweenerEditorDataDic.TryGetValue(tweenerType, out var data))
            {
                displayName = data.Info.DisplayName;
            }
            else displayName = tweenerType.Name;

            EditorGUILayout.LabelField(displayName, EditorStyles.label);
        }

        public override int CompareValue(Tweener data1, Tweener data2)
        {
            var value1 = data1.GetType();
            var value2 = data2.GetType();
            return string.Compare(value1.Name, value2.Name, StringComparison.Ordinal);
        }
    }
}
#endif