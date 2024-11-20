#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIEnableArea : IDisposable
    {
        public bool OriginalEnable;

        public static GUIEnableArea Create(bool enable, bool inheritParent = true)
        {
            return new GUIEnableArea(enable, inheritParent);
        }

        private GUIEnableArea(bool enable, bool inheritParent = true)
        {
            OriginalEnable = GUI.enabled;
            if (!GUI.enabled && inheritParent) enable = false;
            GUI.enabled = enable;
        }

        public void Dispose()
        {
            GUI.enabled = OriginalEnable;
        }
    }
}
#endif