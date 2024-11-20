#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIArea : IDisposable
    {
        public static GUIArea Create(Rect rect)
        {
            return new GUIArea(rect);
        }

        public static GUIArea Create(Rect rect, GUIContent content)
        {
            return new GUIArea(rect, content);
        }

        public static GUIArea Create(Rect rect, GUIContent content, GUIStyle style)
        {
            return new GUIArea(rect, content, style);
        }

        private GUIArea(Rect rect)
        {
            GUILayout.BeginArea(rect);
        }

        private GUIArea(Rect rect, GUIContent content)
        {
            GUILayout.BeginArea(rect, content);
        }

        private GUIArea(Rect rect, GUIContent content, GUIStyle style)
        {
            GUILayout.BeginArea(rect, content, style);
        }

        public void Dispose()
        {
            GUILayout.EndArea();
        }
    }
}
#endif