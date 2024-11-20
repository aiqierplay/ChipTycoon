#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Aya.TweenPro
{
    public struct GUILabelWidthArea : IDisposable
    {
        public float OriginalLabelWidth;

        public static GUILabelWidthArea Create(float labelWidth)
        {
            return new GUILabelWidthArea(labelWidth);
        }

        private GUILabelWidthArea(float labelWidth)
        {
            OriginalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
        }

        public void Dispose()
        {
            EditorGUIUtility.labelWidth = OriginalLabelWidth;
        }
    }
}
#endif