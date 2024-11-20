#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static class HandlesUtil
    {
        public static void DrawTexture(Vector3 position, Texture2D texture)
        {
            DrawTexture(position, texture, new Vector2(texture.width, texture.height));
        }

        public static void DrawTexture(Vector3 position, Texture2D texture, Vector2 size)
        {
            Handles.BeginGUI();
            var guiPos = HandleUtility.WorldToGUIPoint(position);
            GUI.DrawTexture(new Rect(guiPos.x - size.x / 2f, guiPos.y - size.y / 2f, size.x, size.y), texture);
            Handles.EndGUI();
        }
    }
}
#endif 