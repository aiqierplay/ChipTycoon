#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIUtil
    {
        public static void DrawToolbarEnum(SerializedProperty property, Type enumType)
        {
            DrawToolbarEnum(property, property.displayName, enumType);
        }

        public static void DrawToolbarEnum(SerializedProperty property, string propertyName, Type enumType)
        {
            property.intValue = DrawToolbarEnum(property.intValue, propertyName, enumType);
        }

        public static int DrawToolbarEnum(int value, string propertyName, Type enumType)
        {
            using (GUIHorizontal.Create())
            {
                GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                var buttons = Enum.GetNames(enumType);
                var style = EditorStyles.miniButton;
                style.margin = new RectOffset();
                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, style);
                var btnWidth = rect.width / buttons.Length;
                for (var i = 0; i < buttons.Length; i++)
                {
                    var button = buttons[i];
                    var index = i;
                    var btnRect = rect;
                    btnRect.x += i * btnWidth;
                    btnRect.width = btnWidth;
                    using (GUIColorArea.Create(UTweenEditorSetting.Ins.SelectedColor, UTweenEditorSetting.Ins.DisableColor, value == index))
                    {
                        var btn = GUI.Button(btnRect, button, style);
                        if (btn)
                        {
                            return index;
                        }
                    }
                }
            }

            return value;
        }
    }
}
#endif