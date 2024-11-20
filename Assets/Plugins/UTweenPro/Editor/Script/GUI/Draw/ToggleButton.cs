#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIUtil
    {
        public static void DrawToggleButton(SerializedProperty property)
        {
            DrawToggleButton(property, property.displayName);
        }

        public static void DrawToggleButton(SerializedProperty property, string propertyName)
        {
            property.boolValue = DrawToggleButton(propertyName, property.boolValue);
        }

        public static bool DrawToggleButton(string propertyName, bool value)
        {
            return DrawToggleButton(propertyName, value, UTweenEditorSetting.Ins.EnableColor, UTweenEditorSetting.Ins.DisableColor);
        }

        public static bool DrawToggleButton(string propertyName, bool value, Color enableColor, Color disableColor)
        {
            using (GUIHorizontal.Create())
            {
                var btnStyle = EditorStyles.miniButton;
                // btnStyle.margin = new RectOffset();
                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.miniButton);
                using (GUIColorArea.Create(enableColor, disableColor, value))
                {
                    var result = GUI.Toggle(rect, value, propertyName, btnStyle);
                    return result;
                }
            }
        }
    }
}
#endif