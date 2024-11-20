#if UNITY_EDITOR
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIUtil
    {
        public static void DrawTipArea(Color color, string tip)
        {
            using (GUIColorArea.Create(color))
            {
                using (GUIGroup.Create())
                {
                    GUILayout.Label(tip, EditorStyle.MultiLineLabel);
                }
            }
        }
        public static void DrawTipArea(string tip)
        {
            using (GUIGroup.Create())
            {
                GUILayout.Label(tip, EditorStyle.MultiLineLabel);
            }
        }
    }
}
#endif