#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIUtil
    {
        public static bool DrawHoldProgressToggleButton(SerializedProperty property, string propertyName, string toolTip, float width, float height)
        {
            var btnStyle = EditorStyle.ProgressHoldButton;
            btnStyle.fixedWidth = width;
            btnStyle.fixedHeight = height;
            var enableColor = UTweenEditorSetting.Ins.EnableColor;
            var disableColor = GUI.enabled ? UTweenEditorSetting.Ins.SelectedColor : UTweenEditorSetting.Ins.DisableColor;
            var rect = EditorGUILayout.GetControlRect(false, GUILayout.Width(width), GUILayout.Height(height));
            using (GUIColorArea.Create(enableColor, disableColor, property.boolValue))
            {
                var content = new GUIContent(propertyName, toolTip);
                var result = GUI.Toggle(rect, property.boolValue, content, btnStyle);
                property.boolValue = result;
                return result;
            }
        }

        #region Button

        public static bool DrawFoldOutButton(bool foldOut)
        {
            foldOut = EditorGUILayout.Toggle(GUIContent.none, foldOut, EditorStyles.foldout, GUILayout.Width(EditorStyle.CharacterWidth));
            return foldOut;
        }

        public static bool DrawHeaderDocumentationButton()
        {
            var button = DrawIconButton("_Help", "Help");
            return button;
        }

        public static bool DrawOptionMenuButton(string toolTip = "")
        {
            var button = DrawIconButton("pane options", toolTip);
            return button;
        }

        public static bool DrawSearchMenuButton()
        {
            var button = DrawIconButton("Search Icon", "Search");
            return button;
        }

        public static bool DrawDeleteButton()
        {
            var button = DrawIconButton("winbtn_win_close", " Delete");
            return button;
        }

        public static bool DrawImportPresetButton()
        {
            var button = DrawIconButton("Preset.Context", " Import Preset");
            return button;
        }

        public static bool DrawIconButton(string iconName, string toolTip = "")
        {
            var content = new GUIContent(EditorGUIUtility.IconContent(iconName))
            {
                tooltip = toolTip
            };

            var button = GUILayout.Button(content, EditorStyles.miniButtonMid, GUILayout.Width(EditorStyle.SingleButtonWidth));
            return button;
        }

        public static bool DrawHeaderIdentifierButton(SerializedProperty property)
        {
            return DrawIconToggleButton(property, "FilterByLabel", "Identifier");
        }

        public static bool DrawHeaderAxisButton(SerializedProperty property)
        {
            return DrawIconToggleButton(property, "AvatarPivot", "Independent Axis");
        }

        public static bool DrawCallbackButton(SerializedProperty property)
        {
            return DrawIconToggleButton(property, "Animation.AddEvent", "Callback");
        }

        public static bool DrawIconToggleButton(SerializedProperty property, string iconName, string toolTip = "")
        {
            using (GUIColorArea.Create(UTweenEditorSetting.Ins.EnableColor, property.boolValue))
            {
                var content = new GUIContent(EditorGUIUtility.IconContent(iconName))
                {
                    tooltip = toolTip
                };

                var button = GUILayout.Button(content, EditorStyles.miniButtonMid, GUILayout.Width(EditorStyle.SingleButtonWidth));
                if (button)
                {
                    property.boolValue = !property.boolValue;
                }

                return button;
            }
        }

        public static bool DrawSelectEnumButton(SerializedProperty modeProperty, Type enumType, string displayName = "")
        {
            var button = string.IsNullOrEmpty(displayName) ? DrawSelectModeButton() : DrawAutoSizeSelectModeButton(displayName);
            if (button)
            {
                var menu = new GenericMenu();
                var showNames = modeProperty.enumDisplayNames;
                var names = Enum.GetNames(enumType);
                var values = Enum.GetValues(enumType);
                for (var i = 0; i < names.Length; i++)
                {
                    var showName = showNames[i];
                    var name = names[i];
                    var value = (int)values.GetValue(i);
                    var hideAttribute = enumType.GetField(name).GetCustomAttribute<HideInInspector>(false);
                    if (hideAttribute != null) continue;
                    menu.AddItem(showName, value == modeProperty.intValue, () =>
                    {
                        modeProperty.intValue = value;
                        modeProperty.serializedObject.ApplyModifiedProperties();
                    });
                }
                
                menu.ShowAsContext();
            }

            return button;
        }

        public static bool DrawAutoSizeSelectModeButton(string name)
        {
            var width = name.Length == 1 ? (EditorStyle.SingleButtonWidth + 3) : (EditorStyle.CharacterWidth + 2) * name.Length;
            width += EditorStyle.CharacterWidth;
            var button = GUILayout.Button(new GUIContent(name), EditorStyles.popup, GUILayout.Width(width));
            return button;
        }

        public static bool DrawSelectModeButton()
        {
            var button = GUILayout.Button(GUIContent.none, EditorStyles.popup, GUILayout.Width(EditorStyle.SingleButtonWidth));
            return button;
        }

        #endregion

        #region From To Property

        public static void DrawLabel(string label)
        {
            GUILayout.Label(label, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
        }

        public static void DrawMinLabel(string label)
        {
            GUILayout.Label(label, EditorStyles.label, GUILayout.Width(EditorStyle.MinWidth));
        }

        public static float DrawFloatProperty(string name, float value, bool enable)
        {
            using (GUIEnableArea.Create(enable))
            {
                value = EditorGUILayout.FloatField(name, value, GUILayout.MinWidth(EditorStyle.MinWidth));
                return value;
            }
        }

        public static int DrawIntProperty(string name, int value, bool enable)
        {
            using (GUIEnableArea.Create(enable))
            {
                value = EditorGUILayout.IntField(name, value, GUILayout.MinWidth(EditorStyle.MinWidth));
                return value;
            }
        }

        public static void DrawProperty(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property);
        }

        public static void DrawProperty(SerializedProperty property, string name)
        {
            EditorGUILayout.PropertyField(property, new GUIContent(name));
        }

        public static void DrawVector2Property(SerializedProperty property, string name,
            string axis1Name, string axis2Name,
            bool enableAxis1, bool enableAxis2)
        {
            using (GUIHorizontal.Create())
            {
                var value = property.vector2Value;
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                }

                property.vector2Value = value;
            }
        }


        public static void DrawVector2IntProperty(SerializedProperty property, string name,
            string axis1Name, string axis2Name,
            bool enableAxis1, bool enableAxis2)
        {
            using (GUIHorizontal.Create())
            {
                var value = property.vector2IntValue;
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawIntProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawIntProperty(axis2Name, value.y, enableAxis2);
                }

                property.vector2IntValue = value;
            }
        }

        public static void DrawVector3Property(SerializedProperty valueProperty, string name,
            string axis1Name, string axis2Name, string axis3Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3)
        {
            valueProperty.vector3Value = DrawVector3Property(valueProperty.vector3Value, name,
                axis1Name, axis2Name, axis3Name,
                enableAxis1, enableAxis2, enableAxis3);
        }

        public static Vector3 DrawVector3Property(Vector3 value, string name,
            string axis1Name, string axis2Name, string axis3Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3)
        {
            using (GUIHorizontal.Create())
            {
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                    value.z = DrawFloatProperty(axis3Name, value.z, enableAxis3);
                }

                return value;
            }
        }

        public static void DrawVector3IntProperty(SerializedProperty valueProperty, string name,
            string axis1Name, string axis2Name, string axis3Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3)
        {
            valueProperty.vector3IntValue = DrawVector3IntProperty(valueProperty.vector3IntValue, name,
                axis1Name, axis2Name, axis3Name,
                enableAxis1, enableAxis2, enableAxis3);
        }

        public static Vector3Int DrawVector3IntProperty(Vector3Int value, string name,
            string axis1Name, string axis2Name, string axis3Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3)
        {
            using (GUIHorizontal.Create())
            {
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawIntProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawIntProperty(axis2Name, value.y, enableAxis2);
                    value.z = DrawIntProperty(axis3Name, value.z, enableAxis3);
                }

                return value;
            }
        }

        public static void DrawVector4Property(SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            using (GUIHorizontal.Create())
            {
                var value = property.vector4Value;
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                    value.z = DrawFloatProperty(axis3Name, value.z, enableAxis3);
                    value.w = DrawFloatProperty(axis4Name, value.w, enableAxis4);
                }

                property.vector4Value = value;
            }
        }

        public static void DrawBoundsProperty(SerializedProperty property, string name, bool enableAxis1, bool enableAxis2)
        {
            using (GUIHorizontal.Create())
            {
                var value = property.boundsValue;
                using (GUIVertical.Create(GUILayout.Width(EditorGUIUtility.labelWidth)))
                {
                    GUILayout.Label(name, EditorStyles.label);
                    GUILayout.Label("");
                }
              
                using (GUIVertical.Create())
                {
                    value.center = DrawVector3Property(value.center, "Center", "X", "Y", "Z", enableAxis1, enableAxis1, enableAxis1);
                    value.size = DrawVector3Property(value.size, "Size", "X", "Y", "Z", enableAxis2, enableAxis2, enableAxis2);
                }

                property.boundsValue = value;
            }
            
            GUILayout.Space(2);
        }

        public static void DrawBoundsIntProperty(SerializedProperty property, string name, bool enableAxis1, bool enableAxis2)
        {
            using (GUIHorizontal.Create())
            {
                var value = property.boundsIntValue;
                using (GUIVertical.Create(GUILayout.Width(EditorGUIUtility.labelWidth)))
                {
                    GUILayout.Label(name, EditorStyles.label);
                    GUILayout.Label("");
                }

                using (GUIVertical.Create())
                {
                    value.position = DrawVector3IntProperty(value.position, "Position", "X", "Y", "Z", enableAxis1, enableAxis1, enableAxis1);
                    value.size = DrawVector3IntProperty(value.size, "Size", "X", "Y", "Z", enableAxis2, enableAxis2, enableAxis2);
                }

                property.boundsIntValue = value;
            }

            GUILayout.Space(2);
        }

        public static void DrawRectProperty(SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            using (GUIHorizontal.Create())
            {
                var value = property.rectValue;
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                    value.width = DrawFloatProperty(axis3Name, value.width, enableAxis3);
                    value.height = DrawFloatProperty(axis4Name, value.height, enableAxis4);
                }

                property.rectValue = value;
            }
        }

        public static void DrawRectIntProperty(SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            using (GUIHorizontal.Create())
            {
                var value = property.rectIntValue;
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawIntProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawIntProperty(axis2Name, value.y, enableAxis2);
                    value.width = DrawIntProperty(axis3Name, value.width, enableAxis3);
                    value.height = DrawIntProperty(axis4Name, value.height, enableAxis4);
                }

                property.rectIntValue = value;
            }
        }

        public static void DrawRectOffsetProperty(SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            var leftProperty = property.FindPropertyRelative("m_Left");
            var rightProperty = property.FindPropertyRelative("m_Right");
            var topProperty = property.FindPropertyRelative("m_Top");
            var bottomProperty = property.FindPropertyRelative("m_Bottom");
            using (GUIHorizontal.Create())
            {
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    leftProperty.intValue = DrawIntProperty(axis1Name, leftProperty.intValue, enableAxis1);
                    rightProperty.intValue = DrawIntProperty(axis2Name, rightProperty.intValue, enableAxis2);
                    topProperty.intValue = DrawIntProperty(axis3Name, topProperty.intValue, enableAxis3);
                    bottomProperty.intValue = DrawIntProperty(axis4Name, bottomProperty.intValue, enableAxis4);
                }
            }
        }

        public static void DrawQuaternionProperty(SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            using (GUIHorizontal.Create())
            {
                var value = property.quaternionValue;
                DrawLabel(name);
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                    value.z = DrawFloatProperty(axis3Name, value.z, enableAxis3);
                    value.w = DrawFloatProperty(axis4Name, value.w, enableAxis4);
                }

                property.quaternionValue = value;
            }
        } 

        #endregion

        #region Draggable ProgressBar

        public static void DrawDraggableProgressBar(UnityEngine.Object target, float height, float currentValue, 
            Action<float> onDown,
            Action<float> onUp,
            Action<float> onValueChanged)
        {
            var disableColor = UTweenEditorSetting.Ins.ProgressDisableColor;
            var backColor = UTweenEditorSetting.Ins.ProgressBackColor;
            var rangeColor = UTweenEditorSetting.Ins.ProgressColor;
            var rect = EditorGUILayout.GetControlRect(false, height);
            var valuePos = Mathf.Round(rect.width * currentValue);

            // Back
            EditorGUI.DrawRect(rect, backColor);

            // Progress
            var rectProgress = rect;
            rectProgress.width = valuePos;
            EditorGUI.DrawRect(rectProgress, rangeColor);

            if (onValueChanged != null)
            {
                var id = GUIUtility.GetControlID(FocusType.Passive);
                Draggable(target, id, ref _progressState1, rect, rect,
                    value =>
                    {
                        currentValue = value;
                        onDown?.Invoke(value);
                    },
                    value =>
                    {
                        currentValue = value;
                        onUp?.Invoke(value);
                    },
                    value =>
                    {
                        currentValue = value;
                        onValueChanged?.Invoke(currentValue);
                    }, true);
            }
            else
            {
                EditorGUI.DrawRect(rect, disableColor);
            }

            if (!GUI.enabled)
            {
                EditorGUI.DrawRect(rect, disableColor);
            }
        }

        private static int _progressState1;
        private static int _progressState2;
        private static int _progressState3;

        public static void DrawFromToDraggableProgressBar(UnityEngine.Object target, float height, float fromValue, float toValue, float currentValue, bool holdStart, bool holdEnd,
            Action<float, float> onValueChanged = null)
        {
            var holdColor = UTweenEditorSetting.Ins.SubProgressHoldColor;
            var disableColor = UTweenEditorSetting.Ins.ProgressDisableColor;
            var backColor = UTweenEditorSetting.Ins.ProgressBackColor;
            var rangeColor = UTweenEditorSetting.Ins.SubProgressColor;
            var inRangeColor = UTweenEditorSetting.Ins.ProgressColor;
            var outOfRangeColor = UTweenEditorSetting.Ins.ProgressColor * 0.55f;

            var rect = EditorGUILayout.GetControlRect(false, height);
            var progressWidth = Mathf.Round(rect.width * (toValue - fromValue));
            var fromValuePos = Mathf.Round(rect.width * fromValue);
            var toValuePos = Mathf.Round(rect.width * toValue);
            var currentValuePos = Mathf.Round(rect.width * currentValue);

            // Handle
            var fromRect = new Rect(rect.x, rect.y, fromValuePos + 1f, rect.height);
            var toRect = new Rect(rect.x + toValuePos, rect.y, rect.width - toValuePos + 1f, rect.height);
            var rangeRect = new Rect(rect.x + fromValuePos + 2f, rect.y, progressWidth - 4f, rect.height);

            // Back
            EditorGUI.DrawRect(rect, backColor);

            // Range
            var rectRange = rect;
            rectRange.x = rect.x + Mathf.Round(rect.width * fromValue);
            rectRange.width = progressWidth;
            EditorGUI.DrawRect(rectRange, rangeColor);

            // Hold Start
            if (holdStart)
            {
                var holdStartRect = rect;
                holdStartRect.width = fromValuePos;
                EditorGUI.DrawRect(holdStartRect, holdColor);
            }
            
            // Hold End
            if (holdEnd)
            {
                var holdEndRect = rect;
                holdEndRect.x += toValuePos;
                holdEndRect.width = rect.width - toValuePos;
                EditorGUI.DrawRect(holdEndRect, holdColor);
            }
            
            // Progress
            if (currentValue > 0)
            {
                var delayRect = rect;
                delayRect.width = currentValuePos > fromValuePos ? fromValuePos : currentValuePos;
                EditorGUI.DrawRect(delayRect, outOfRangeColor);

                if (currentValue > fromValue)
                {
                    var playingRect = rect;
                    playingRect.x = rect.x + fromValuePos;
                    playingRect.width = (currentValuePos > toValuePos ? toValuePos : currentValuePos) - fromValuePos;
                    EditorGUI.DrawRect(playingRect, inRangeColor);
                }

                if (currentValue > toValue)
                {
                    var afterRect = rect;
                    afterRect.x = rect.x + toValuePos;
                    afterRect.width = currentValuePos - toValuePos;
                    EditorGUI.DrawRect(afterRect, outOfRangeColor);
                }
            }

            if (onValueChanged != null)
            {
                var id = GUIUtility.GetControlID(FocusType.Passive);
                Draggable(target, id, ref _progressState1, rect, fromRect,
                    value => { },
                    value => { },
                    value =>
                    {
                        fromValue = value;
                        if (fromValue > toValue)
                        {
                            toValue = fromValue;
                        }

                        onValueChanged(fromValue, toValue);
                    }, true);

                Draggable(target, id, ref _progressState2, rect, toRect,
                    value => { },
                    value => { },
                    value =>
                    {
                        toValue = value;
                        if (toValue < fromValue)
                        {
                            fromValue = toValue;
                        }

                        onValueChanged(fromValue, toValue);
                    }, true);
            }
            else
            {
                EditorGUI.DrawRect(rect, disableColor);
            }

            if (!GUI.enabled)
            {
                EditorGUI.DrawRect(rect, disableColor);
            }
        }

        public static void Draggable(UnityEngine.Object target, int controlId, ref int state, Rect progressRect, Rect handleRect, 
            Action<float> onDown, 
            Action<float> onUp, 
            Action<float> onValueChanged, bool allowClick = false)
        {
            var current = Event.current;
            var offset = current.mousePosition.x - progressRect.x + 1f;
            var value = Mathf.Clamp01(offset / progressRect.width);
            switch (current.GetTypeForControl(controlId))
            {
                case EventType.MouseDown:
                    if (handleRect.Contains(current.mousePosition) && current.button == 0)
                    {
                        EditorGUIUtility.editingTextField = false;
                        GUIUtility.hotControl = controlId;
                        Undo.RegisterCompleteObjectUndo(target, "Drag Progress");
                        state = 1;
                        if (allowClick)
                        {
                            value = (float)Math.Round(value, 3);
                            onDown?.Invoke(value);
                            onValueChanged?.Invoke(value);
                            GUI.changed = true;
                        }

                        current.Use();
                    }

                    break;

                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId && state != 0)
                    {
                        GUIUtility.hotControl = 0;
                        state = 0;
                        onUp?.Invoke(value);
                        current.Use();
                    }

                    break;

                case EventType.MouseDrag:
                    if (GUIUtility.hotControl != controlId)
                    {
                        break;
                    }

                    if (state != 0)
                    {
                        value = (float)Math.Round(value, 3);
                        onValueChanged?.Invoke(value);
                        GUI.changed = true;
                        current.Use();
                    }

                    break;

                case UnityEngine.EventType.Repaint:
                    EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.SlideArrow);
                    break;
            }
        }

        #endregion

        #region Rect

        public static void DrawEmptyRect(Rect rect, Color color)
        {
            var left = rect;
            left.width = 1;
            var right = left;
            right.x = rect.x + rect.width;
            var top = rect;
            top.height = 1;
            var bottom = top;
            bottom.y = rect.y + rect.height;

            EditorGUI.DrawRect(left, color);
            EditorGUI.DrawRect(right, color);
            EditorGUI.DrawRect(top, color);
            EditorGUI.DrawRect(bottom, color);
        }

        #endregion
    }
}
#endif