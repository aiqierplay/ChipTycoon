using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void DrawTriangle(Texture2D texture, Vector2 p1, Vector3 p2, Vector3 p3, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            DrawPolygon(texture, new Vector2[] {p1, p2, p3}, color, blendMode);
        }

        public static void FillTriangle(Texture2D texture, Vector2 p1, Vector2 p2, Vector2 p3, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            FillPolygon(texture, new Vector2[] {p1, p2, p3}, color, blendMode);
        }
    }

    public static partial class Texture2DExtension
    {
        public static Texture2D DrawTriangle(this Texture2D texture, Vector2 p1, Vector3 p2, Vector3 p3, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawTriangle(texture, p1, p2, p3, color, blendMode);
            return texture;
        }

        public static Texture2D FillTriangle(this Texture2D texture, Vector2 p1, Vector2 p2, Vector2 p3, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.FillTriangle(texture, p1, p2, p3, color, blendMode);
            return texture;
        }
    }
}