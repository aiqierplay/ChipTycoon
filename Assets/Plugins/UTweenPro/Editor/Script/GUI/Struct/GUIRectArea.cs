#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIRectArea : IDisposable
    {
        public static GUIRectArea Create(Rect rect)
        {
            return new GUIRectArea(rect);
        }

        private GUIRectArea(Rect rect)
        {
            GUILayout.BeginArea(rect);
        }

        public void Dispose()
        {
            GUILayout.EndArea();
        }
    }
}
#endif