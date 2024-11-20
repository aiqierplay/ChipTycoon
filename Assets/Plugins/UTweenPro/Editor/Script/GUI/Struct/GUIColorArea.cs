#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIColorArea : IDisposable
    {
        public Color OriginalColor;

        public static GUIColorArea Create(Color color, bool check = true)
        {
            return new GUIColorArea(color, Color.white, check);
        }

        public static GUIColorArea Create(Color enableColor, Color disableColor, bool check = true)
        {
            return new GUIColorArea(enableColor, disableColor, check);
        }

        public GUIColorArea(Color enableColor, Color disableColor, bool check = true)
        {
            OriginalColor = GUI.color;
            if (check)
            {
                GUI.color = enableColor;
            }
            else
            {
                GUI.color = disableColor;
            }
        }

        public void Dispose()
        {
            GUI.color = OriginalColor;
        }
    }
}
#endif