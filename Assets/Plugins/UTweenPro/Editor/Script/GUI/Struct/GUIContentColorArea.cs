#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIContentColorArea : IDisposable
    {
        public Color OriginalColor;

        public static GUIContentColorArea Create(Color color, bool check = true)
        {
            return new GUIContentColorArea(color, Color.white, check);
        }

        public static GUIContentColorArea Create(Color enableColor, Color disableColor, bool check = true)
        {
            return new GUIContentColorArea(enableColor, disableColor, check);
        }

        public GUIContentColorArea(Color enableColor, Color disableColor, bool check = true)
        {
            OriginalColor = GUI.contentColor;
            if (check)
            {
                GUI.contentColor = enableColor;
            }
            else
            {
                GUI.contentColor = disableColor;
            }
        }

        public void Dispose()
        {
            GUI.contentColor = OriginalColor;
        }
    }
}
#endif