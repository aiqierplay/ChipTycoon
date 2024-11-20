#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public struct GUIEnableColorArea : IDisposable
    {
        public Color OriginalColor;

        public static GUIEnableColorArea Create(bool check = true)
        {
            return new GUIEnableColorArea(check);
        }

        public GUIEnableColorArea(bool check = true)
        {
            OriginalColor = GUI.color;
            if (check)
            {
                GUI.color = UTweenEditorSetting.Ins.EnableColor;
            }
            else
            {
                GUI.color = UTweenEditorSetting.Ins.DisableColor;
            }
        }

        public void Dispose()
        {
            GUI.color = OriginalColor;
        }
    }
}
#endif