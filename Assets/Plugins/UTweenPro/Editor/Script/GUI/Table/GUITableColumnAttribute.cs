#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Aya.TweenPro
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GUITableColumnAttribute : Attribute
    {
        public int Index = 999;
        public string Name = "";
        public string Icon = "";
        public string ToolTip = "";
        public string ContextMenuText = "";
        public float Width = 60;
        public float MinWidth = 30;
        public float MaxWidth = 160;
        public TextAlignment Alignment = TextAlignment.Center;
        public bool AutoResize = true;
        public bool AllowToggle = true;
        public bool CanSort = true;

        // Cache
        public FieldInfo FieldInfo;
        // public PropertyInfo PropertyInfo;

        // public MethodInfo OnDraw;
        // public MethodInfo OnCompare;

        // public string GetPropertyName()
        // {
        //     if (FieldInfo != null) return FieldInfo.Name;
        //     return PropertyInfo.Name;
        // }

        public MultiColumnHeaderState.Column GetColumn()
        {
            var state = new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent(Name, EditorGUIUtility.FindTexture(Icon), ToolTip),
                contextMenuText = ContextMenuText,
                width = Width,
                minWidth = MinWidth,
                maxWidth = MaxWidth,
                headerTextAlignment = Alignment,
                autoResize = AutoResize,
                allowToggleVisibility = AllowToggle,
                canSort = CanSort,
            };

            return state;
        }
    }
}
#endif