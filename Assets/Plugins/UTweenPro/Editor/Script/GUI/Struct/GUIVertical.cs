#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIVertical : IDisposable
    {
        public static GUIVertical Create(params GUILayoutOption[] options)
        {
            return new GUIVertical(options);
        }

        public static GUIVertical Create(GUIStyle style, params GUILayoutOption[] options)
        {
            return new GUIVertical(style, options);
        }

        private GUIVertical(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        private GUIVertical(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }
}
#endif