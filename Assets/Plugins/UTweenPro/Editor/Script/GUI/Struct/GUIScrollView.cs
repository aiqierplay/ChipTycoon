#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIScrollView : IDisposable
    {
        public static GUIScrollView Create(ref Vector2 scrollPos, bool showHorizontal, bool showVertical, params GUILayoutOption[] options)
        {
            return new GUIScrollView(ref scrollPos, showHorizontal, showVertical, options);
        }

        private GUIScrollView(ref Vector2 scrollPos, bool showHorizontal, bool showVertical, params GUILayoutOption[] options)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, showHorizontal, showVertical, options);
        }

        public void Dispose()
        {
            GUILayout.EndScrollView();
        }
    }
}
#endif