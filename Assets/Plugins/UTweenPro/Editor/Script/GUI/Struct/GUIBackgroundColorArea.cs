#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIBackgroundColorArea : IDisposable
    {
        public Color OriginalColor;

        public static GUIBackgroundColorArea Create(Color color, bool check = true)
        {
            return new GUIBackgroundColorArea(color, Color.white, check);
        }

        public static GUIBackgroundColorArea Create(Color enableColor, Color disableColor, bool check = true)
        {
            return new GUIBackgroundColorArea(enableColor, disableColor, check);
        }

        public GUIBackgroundColorArea(Color enableColor, Color disableColor, bool check = true)
        {
            OriginalColor = GUI.backgroundColor;
            if (check)
            {
                GUI.backgroundColor = enableColor;
            }
            else
            {
                GUI.backgroundColor = disableColor;
            }
        }

        public void Dispose()
        {
            GUI.backgroundColor = OriginalColor;
        }
    }
}
#endif