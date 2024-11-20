#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIFoldOut : IDisposable
    {
        public static GUIFoldOut Create(SerializedProperty foldOutProperty, string title = null, params GUILayoutOption[] options)
        {
            return new GUIFoldOut(foldOutProperty, () =>
            {
                var btnTitle = GUILayout.Button(title, EditorStyles.boldLabel);
                if (btnTitle)
                {
                    foldOutProperty.boolValue = !foldOutProperty.boolValue;
                }
            }, null, options);
        }

        public static GUIFoldOut Create(SerializedProperty foldOutProperty, Action drawTitleAction = null, params GUILayoutOption[] options)
        {
            return new GUIFoldOut(foldOutProperty, drawTitleAction, null, options);
        }

        public static GUIFoldOut Create(SerializedProperty foldOutProperty, Action drawTitleAction = null, Action drawAppendAction = null, params GUILayoutOption[] options)
        {
            return new GUIFoldOut(foldOutProperty, drawTitleAction, drawAppendAction, options);
        }

        private GUIFoldOut(SerializedProperty foldOutProperty, Action drawTitleAction, Action drawAppendAction = null, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, options);
            using (GUIHorizontal.Create())
            {
                foldOutProperty.boolValue = GUIUtil.DrawFoldOutButton(foldOutProperty.boolValue);
                drawTitleAction?.Invoke();
            }

            drawAppendAction?.Invoke();
        }

        public void Dispose()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
#endif