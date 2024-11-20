#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIErrorColorArea : IDisposable
    {
        public Color OriginalColor;

        public static GUIErrorColorArea Create(bool check = true)
        {
            return new GUIErrorColorArea(check);
        }

        public GUIErrorColorArea(bool check = true)
        {
            OriginalColor = GUI.color;
            if (check)
            {
                GUI.color = UTweenEditorSetting.Ins.ErrorColor;
            }
        }

        public void Dispose()
        {
            GUI.color = OriginalColor;
        }
    }
}
#endif