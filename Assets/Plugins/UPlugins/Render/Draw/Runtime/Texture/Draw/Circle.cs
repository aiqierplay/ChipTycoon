using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void DrawCircle(Texture2D texture, Vector2 center, int radius, Color color, int border = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            DrawCircle(texture, (int) center.x, (int) center.y, radius, color, border, blendMode);
        }

        public static void DrawCircle(Texture2D texture, int x, int y, int radius, Color color, int border = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            if (radius < 1 || border < 1) return;
            var isBatch = texture.IsInDrawBatch(out var batch);
            Color32 batchColor = color;
            for (var i = -radius; i <= radius; i++)
            {
                for (var j = -radius; j <= radius; j++)
                {
                    var xx = i + x;
                    var yy = j + y;
                    var dis = DrawUtil.DistanceSquare(x, y, xx, yy);
                    if (dis >= radius * radius || dis < (radius - border) * (radius - border)) continue;
                    if (isBatch)
                    {
                        if (batch.CheckOutOfRange(ref xx, ref yy)) continue;
                        batch.SetPixel(xx, yy, batchColor, blendMode);
                    }
                    else
                    {
                        if (texture.CheckOutOfRange(ref xx, ref yy)) continue;
                        texture.SetPixel(xx, yy, color, blendMode);
                    }
                }
            }

            if (!isBatch)
            {
                texture.Apply();
            }
        }

        public static void FillCircle(Texture2D texture, Vector2 center, int radius, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            FillCircle(texture, (int)center.x, (int)center.y, radius, color, blendMode);
        }

        public static void FillCircle(Texture2D texture, int x, int y, int radius, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            if (radius < 1) return;
            var isBatch = texture.IsInDrawBatch(out var batch);
            Color32 batchColor = color;
            for (var i = -radius; i <= radius; i++)
            {
                for (var j = -radius; j <= radius; j++)
                {
                    var xx = i + x;
                    var yy = j + y;
                    var dis = DrawUtil.DistanceSquare(x, y, xx, yy);
                    if (dis >= radius * radius) continue;
                    if (isBatch)
                    {
                        if (batch.CheckOutOfRange(ref xx, ref yy)) continue;
                        batch.SetPixel(xx, yy, batchColor, blendMode);
                    }
                    else
                    {
                        if (texture.CheckOutOfRange(ref xx, ref yy)) continue;
                        texture.SetPixel(xx, yy, color, blendMode);
                    }
                }
            }

            if (!isBatch)
            {
                texture.Apply();
            }
        }
    }

    public static partial class Texture2DExtension
    {
        public static Texture2D DrawCircle(this Texture2D texture, Vector2 center, int radius, Color color, int border = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawCircle(texture, center, radius, color, border, blendMode);
            return texture;
        }

        public static Texture2D DrawCircle(this Texture2D texture, int x, int y, int radius, Color color, int border = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawCircle(texture, x, y, radius, color, border, blendMode);
            return texture;
        }

        public static Texture2D FillCircle(this Texture2D texture, Vector2 center, int radius, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.FillCircle(texture, center, radius, color, blendMode);
            return texture;
        }

        public static Texture2D FillCircle(this Texture2D texture, int x, int y, int radius, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.FillCircle(texture, x, y, radius, color, blendMode);
            return texture;
        }
    }
}