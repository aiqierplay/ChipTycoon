#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Aya.TweenPro
{
    public struct GUIWideMode : IDisposable
    {
        public bool OriginalMode;

        public static GUIWideMode Create(bool enable)
        {
            return new GUIWideMode(enable);
        }

        private GUIWideMode(bool enable)
        {
            OriginalMode = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = enable;
        }

        public void Dispose()
        {
            EditorGUIUtility.wideMode = OriginalMode;
        }
    }
}
#endif