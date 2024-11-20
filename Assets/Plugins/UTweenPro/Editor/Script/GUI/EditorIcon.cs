#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static class EditorIcon
    {
        public static Texture2D MoveToolOn => GetIcon("d_MoveTool on");
        public static Texture2D MoveTool => GetIcon("d_MoveTool");
        public static Texture2D RotateToolOn => GetIcon("d_RotateTool On");
        public static Texture2D RotateTool => GetIcon("d_RotateTool");
        public static Texture2D RectToolOn => GetIcon("d_RectTool On");
        public static Texture2D RectTool => GetIcon("d_RectTool");
        public static Texture2D DotFill => GetIcon("DotFill");
        public static Texture2D Script => GetIcon("cs Script Icon");

        #region Tweener Icon

        private static Dictionary<Type, Texture2D> _tweenerIconDic;

        public static Texture2D GetTweenerIcon(Type tweenerType)
        {
            if (_tweenerIconDic == null) _tweenerIconDic = new Dictionary<Type, Texture2D>();
            if (!_tweenerIconDic.TryGetValue(tweenerType, out var icon))
            {
                if (TypeCaches.TweenerEditorDataDic.TryGetValue(tweenerType, out var tweenerEditorData))
                {
                    if (tweenerEditorData == null) return Script;
                    if (string.IsNullOrEmpty(tweenerEditorData.Info.IconName))
                    {
                        var targetType = tweenerType.GetField("Target")?.FieldType;
                        icon = EditorGUIUtility.ObjectContent(null, targetType).image as Texture2D;
                    }
                    else
                    {
                        icon = GetIcon(tweenerEditorData.Info.IconName);
                    }

                    _tweenerIconDic.Add(tweenerType, icon);
                }
            }

            if (icon == null) return Script;
            return icon;
        }

        #endregion

        #region Icon Method

        public static Texture2D CreateIcon(Type type, int size = 24)
        {
            var srcIcon = GetIcon(type);
            if (srcIcon == null) return default;
            var icon = CreateIconWithSrc(srcIcon, size);
            return icon;
        }

        public static Texture2D CreateIcon(string name, int size = 24)
        {
            var srcIcon = GetIcon(name);
            if (srcIcon == null) return default;
            var icon = CreateIconWithSrc(srcIcon, size);
            return icon;
        }

        public static Texture2D GetIcon(Type type)
        {
            if (type != null) return EditorGUIUtility.ObjectContent(null, type).image as Texture2D;
            return default;
        }

        public static Texture2D GetIcon(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var icon = EditorGUIUtility.FindTexture(name);
                if (icon == null) icon = AssetDatabase.GetCachedIcon(name) as Texture2D;
                if (icon == null) icon = EditorGUIUtility.IconContent(name).image as Texture2D;
                return icon;
            }

            return default;
        }

        public static Texture2D CreateIconWithSrc(Texture2D srcIcon, int size = 24)
        {
            // Copy Built-in texture with RenderTexture
            var tempRenderTexture = RenderTexture.GetTemporary(size, size, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(srcIcon, tempRenderTexture);
            var previousRenderTexture = RenderTexture.active;
            RenderTexture.active = tempRenderTexture;
            var icon = new Texture2D(size, size);
            icon.ReadPixels(new Rect(0, 0, tempRenderTexture.width, tempRenderTexture.height), 0, 0);
            icon.Apply();
            RenderTexture.ReleaseTemporary(tempRenderTexture);
            RenderTexture.active = previousRenderTexture;
            return icon;
        }

        #endregion
    }

}
#endif