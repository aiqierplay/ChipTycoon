using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void DrawRectangle(Texture2D texture, Vector2 position, Vector2 size, Color color, int border = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            DrawRectangle(texture, (int)position.x, (int)position.y, (int)size.x, (int)size.y, color, border, blendMode);
        }

        public static void DrawRectangle(Texture2D texture, int x, int y, int width, int height, Color color, int border = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var isBatch = texture.IsInDrawBatch(out var batch);
            if (border < 1) return;
            Color32 batchColor = color;
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (i >= border && j >= border && i < width - border && j < height - border) continue;
                    var xx = i + x;
                    var yy = j + y;
                  
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

            if (isBatch)
            {
                batch.SyncPixels(x, y, width, height);
            }
            else
            {
                texture.Apply(texture);
            }
        }

        public static void FillRectangle(Texture2D texture, Vector2 position, Vector2 size, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            FillRectangle(texture, (int)position.x, (int)position.y, (int)size.x, (int)size.y, color, blendMode);
        }

        public static void FillRectangle(Texture2D texture, int x, int y, int width, int height, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var isBatch = texture.IsInDrawBatch(out var batch);
            Color32 batchColor = color;
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var xx = i + x;
                    var yy = j + y;
                    
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

            if (isBatch)
            {
                batch.SyncPixels(x, y, width, height);
            }
            else
            {
                texture.Apply();
            }
        }
    }

    public static partial class Texture2DExtension
    {
        public static Texture2D DrawRectangle(this Texture2D texture, Vector2 position, Vector2 size, Color color, int border = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawRectangle(texture, position, size, color, border, blendMode);
            return texture;
        }

        public static Texture2D DrawRectangle(this Texture2D texture, int x, int y, int width, int height, Color color, int border = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawRectangle(texture, x, y, width, height, color, border, blendMode);
            return texture;
        }

        public static Texture2D FillRectangle(this Texture2D texture, Vector2 position, Vector2 size, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.FillRectangle(texture, position, size, color, blendMode);
            return texture;
        }

        public static Texture2D FillRectangle(this Texture2D texture, int x, int y, int width, int height, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.FillRectangle(texture, x, y, width, height, color, blendMode);
            return texture;
        }
    }
}