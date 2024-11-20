#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIUtil
    {
        public static void DrawLine(Vector2 start, Vector2 end, Color color, float width = 1)
        {
            Handles.color = color;
            // Handles.DrawLine(start, end);
            Handles.DrawAAPolyLine(width, start, end);
        }
    }
}
#endif