using System.Collections.Generic;
using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void Fill(Texture2D texture, Vector2 point, Color fillColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            Fill(texture, (int) point.x, (int) point.y, fillColor, blendMode);
        }

        public static void Fill(Texture2D texture, int x, int y, Color fillColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var isBatch = texture.IsInDrawBatch(out var drawTextureBatch);
            if (isBatch)
            {
                var startColor = drawTextureBatch.GetPixel(x, y);
                FillScanLine(texture, x, y, fillColor, startColor, blendMode);
                texture.SetPixels32(drawTextureBatch.DrawColors);
            }
            else
            {
                var startColor = texture.GetPixel(x, y);
                FillScanLine(texture, x, y, fillColor, startColor, blendMode);
                texture.Apply();
            }
        }

        public static void FillWithBorder(Texture2D texture, Vector2 point, Color fillColor, Color borderColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            FillWithBorder(texture, (int)point.x, (int)point.y, fillColor, borderColor, blendMode);
        }

        public static void FillWithBorder(Texture2D texture, int x, int y, Color fillColor, Color borderColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var isBatch = texture.IsInDrawBatch(out var batch);
            FillScanLine(texture, x, y, fillColor, borderColor, blendMode);
            if (isBatch)
            {
                texture.SetPixels32(batch.DrawColors);
            }
            else
            {
                texture.Apply();
            }
        }

        internal static void FillScanLine(Texture2D texture, int x, int y, Color fillColor, Color checkColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var chromaticAberration = DrawUtil.DefaultChromaticAberration;
            var isBatch = texture.IsInDrawBatch(out var batch);
            var stack = new Stack<Vector2Int>();
            stack.Push(new Vector2Int(x, y));

            Color GetColor(int xx, int yy) => isBatch ? batch.GetPixel(xx, yy) : texture.GetPixel(xx, yy);

            void SetColor(int xx, int yy, Color color)
            {
                if (isBatch)
                {
                    batch.SetPixel(xx, yy, color, blendMode);
                }
                else
                {
                    texture.SetPixel(xx, yy, color, blendMode);
                }
            }

            bool CheckSkipDraw(Color color)
            {
                return DrawUtil.ColorEquals(color, checkColor, chromaticAberration) || DrawUtil.ColorEquals(color, fillColor, chromaticAberration);
            }

            while (stack.Count > 0)
            {
                var seed = stack.Pop();
                x = seed.x;
                y = seed.y;

                var xRight = x;
                for (var i = x; i < texture.width; i++)
                {
                    var currentColor = GetColor(i, y);
                    if (CheckSkipDraw(currentColor))
                    {
                        xRight = i - 1;
                        break;
                    }

                    SetColor(i, y, fillColor);
                }

                var xLeft = x;
                for (var i = x - 1; i >= 0; i--)
                {
                    var currentColor = GetColor(i, y);
                    if (CheckSkipDraw(currentColor))
                    {
                        xLeft = i + 1;
                        break;
                    }

                    SetColor(i, y, fillColor);
                }

                var skipCount = 0;
                var find = false;
                for (var i = xLeft; i <= xRight; i++)
                {
                    if (isBatch)
                    {
                        if (batch.CheckOutOfRange(i, y + 1)) continue;
                    }
                    else
                    {
                        if (texture.CheckOutOfRange(i, y + 1)) continue;
                    }

                    var upColor = GetColor(i, y + 1);
                    if (CheckSkipDraw(upColor))
                    {
                        skipCount++;
                        find = false;
                        continue;
                    }

                    if (skipCount % 2 == 0 && !find)
                    {
                        var upSeed = new Vector2Int(i, y + 1);
                        stack.Push(upSeed);
                        find = true;
                    }
                }

                skipCount = 0;
                find = false;
                for (var i = xLeft; i <= xRight; i++)
                {
                    if (isBatch)
                    {
                        if (batch.CheckOutOfRange(i, y - 1)) continue;
                    }
                    else
                    {
                        if (texture.CheckOutOfRange(i, y - 1)) continue;
                    }

                    var downColor = GetColor(i, y - 1);
                    if (CheckSkipDraw(downColor))
                    {
                        skipCount++;
                        find = false;
                        continue;
                    }

                    if (skipCount % 2 == 0 && !find)
                    {
                        var downSeed = new Vector2Int(i, y - 1);
                        stack.Push(downSeed);
                        find = true;
                    }
                }
            }
        }
    }

    public static partial class Texture2DExtension
    {
        public static Texture2D FillWithBorder(this Texture2D texture, Vector2 point, Color fillColor, Color borderColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.FillWithBorder(texture, point, fillColor, borderColor, blendMode);
            return texture;
        }

        public static Texture2D FillWithBorder(this Texture2D texture, int x, int y, Color fillColor, Color borderColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.FillWithBorder(texture, x, y, fillColor, borderColor, blendMode);
            return texture;
        }

        public static Texture2D Fill(this Texture2D texture, Vector2 point, Color fillColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.Fill(texture, point, fillColor, blendMode);
            return texture;
        }

        public static Texture2D Fill(this Texture2D texture, int x, int y, Color fillColor, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.Fill(texture, x, y, fillColor, blendMode);
            return texture;
        }
    }
}