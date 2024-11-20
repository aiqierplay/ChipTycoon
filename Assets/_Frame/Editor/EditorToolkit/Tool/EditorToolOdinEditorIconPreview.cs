#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Utilities.Editor;
using UnityEngine;
using UnityEditor;

[EditorTool]
public class EditorToolOdinEditorIconPreview : EditorToolBase
{
    public class IconData
    {
        public string Name;
        public string Type;
        public Texture2D Icon;
    }

    public override string GetTitle() => "Build-In Res/Odin EditorIcon Preview";
    public override SdfIconType GetIcon() => SdfIconType.Inbox;

    [NonSerialized]
    public List<IconData> IconList;

    public void CacheIcons()
    {
        if (IconList != null) return;
        IconList = new List<IconData>();
        var type = typeof(EditorIcons);
        var propertyList = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
        foreach (var property in propertyList)
        {
            if (property.PropertyType == typeof(Texture2D))
            {
                var texture = property.GetValue(null) as Texture2D;
                var iconData = new IconData()
                {
                    Name = property.Name,
                    Type = nameof(Texture2D),
                    Icon = texture,
                };

                IconList.Add(iconData);
            }


            if (property.PropertyType == typeof(EditorIcon))
            {
                var editorIcon = property.GetValue(null) as EditorIcon;
                if (editorIcon == null) continue;
                var texture = editorIcon.Raw;
                var iconData = new IconData()
                {
                    Name = property.Name,
                    Type = nameof(EditorIcon),
                    Icon = texture,
                };

                IconList.Add(iconData);
            }
        }
    }

    [OnInspectorGUI]
    public void DrawGui()
    {
        CacheIcons();
        DrawIcon();
    }

    public void DrawIcon()
    {
        var ppp = EditorGUIUtility.pixelsPerPoint;
        var iconSize = 66f;
        var column = Mathf.FloorToInt((Screen.width / ppp - EditorToolkit.ToolkitMenuSize) / iconSize) - 1;
        if (column < 1) column = 1;
        var index = 0;
        using (new GUILayout.VerticalScope())
        {
            for (var y = 0;; y++)
            {
                using (new GUILayout.HorizontalScope())
                {
                    for (var x = 0; x < column; x++)
                    {
                        var iconData = IconList[index];
                        DrawIcon(iconData, iconSize);
                        index++;
                        if (index >= IconList.Count)
                        {
                            break;
                        }
                    }

                    GUILayout.FlexibleSpace();
                }

                if (index >= IconList.Count) break;
            }
        }
    }

    public void DrawIcon(IconData iconData, float iconSize)
    {
        using (new GUILayout.VerticalScope())
        {
            var content = new GUIContent(iconData.Icon, iconData.Name);
            GUILayout.Button(content, GUILayout.Width(iconSize), GUILayout.Height(iconSize));
            // GUILayout.Button(iconData.Name);
            // GUILayout.Button(iconData.Type);
        }
    }
}
#endif