#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIGroup : IDisposable
    {
        public static GUIGroup Create(params GUILayoutOption[] options)
        {
            return new GUIGroup(options);
        }

        public static GUIGroup Create(string title, params GUILayoutOption[] options)
        {
            return new GUIGroup(title, options);
        }

        private GUIGroup(params GUILayoutOption[] options) : this(null, options)
        {

        }

        private GUIGroup(string title, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, options);
            if (!string.IsNullOrEmpty(title))
            {
                GUILayout.Button(title, EditorStyles.boldLabel);
            }
        }

        public void Dispose()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
#endif