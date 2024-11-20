#if UNITY_EDITOR
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class GUIUtil
    {
        public static void DrawBox(Rect rect, Color color, float width = 1)
        {
            DrawBox(rect.position, rect.size, color, width);
        }

        public static void DrawBox(Vector2 position, Vector2 size, Color color, float width = 1)
        {
            DrawLine(position, new Vector3(position.x + size.x, position.y), color, width);
            DrawLine(new Vector3(position.x + size.x, position.y), new Vector3(position.x + size.x, position.y + size.y), color, width);
            DrawLine(new Vector3(position.x, position.y + size.y), new Vector3(position.x + size.x, position.y + size.y), color, width);
            DrawLine(position, new Vector3(position.x, position.y + size.y), color, width);
        }
    }
}
#endif