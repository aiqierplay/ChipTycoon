using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void DrawLine(Texture2D texture, Vector2 p1, Vector2 p2, Color color, int size = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            DrawLine(texture, (int) p1.x, (int) p1.y, (int) p2.x, (int) p2.y, color, size);
        }

        public static void DrawLine(Texture2D texture, int x1, int y1, int x2, int y2, Color color, int size = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var isBatch = texture.IsInDrawBatch(out var batch);

            void SetColor(int xx, int yy)
            {
                if (isBatch)
                {
                    if (batch.CheckOutOfRange(xx, yy)) return;
                }
                else
                {
                    if (texture.CheckOutOfRange(ref xx, ref yy)) return;
                }

                if (size == 1)
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
                else
                {
                    FillCircle(texture, xx, yy, size, color, blendMode);
                }
            }

            if (Mathf.Abs(x1 - x2) <= 1e-6)
            {
                if (y1 > y2)
                {
                    DrawUtil.Switch(ref x1, ref x2);
                    DrawUtil.Switch(ref y1, ref y2);
                }
                
                for (var i = y1; i <= y2; i++)
                {
                    SetColor(x1, i);
                }
            }
            else if (Mathf.Abs(y1 - y2) <= 1e-6)
            {
                if (x1 > x2)
                {
                    DrawUtil.Switch(ref x1, ref x2);
                    DrawUtil.Switch(ref y1, ref y2);
                }
                
                for (var i = x1; i <= x2; i++)
                {
                    SetColor(i, y1);
                }
            }
            else
            {
                var k = ((float)(y2 - y1)) / (x2 - x1);
                var step = 1f;
                if (Mathf.Abs(x1 - x2) > Mathf.Abs(y1 - y2))
                {
                    if (x1 > x2)
                    {
                        DrawUtil.Switch(ref x1, ref x2);
                        DrawUtil.Switch(ref y1, ref y2);
                    }

                    for (float i = x1; i < x2; i += step)
                    {
                        var yy = (int)(k * (i - x1) + y1);
                        var xx = (int)i;
                        SetColor(xx, yy);
                    }
                }
                else
                {
                    if (y1 > y2)
                    {
                        DrawUtil.Switch(ref x1, ref x2);
                        DrawUtil.Switch(ref y1, ref y2);
                    }

                    for (float i = y1; i < y2; i += step)
                    {
                        var xx = (int) ((i - y1) / k + x1);
                        var yy = (int) i;
                        SetColor(xx, yy);
                    }
                }
            }

            if (!isBatch && size == 1)
            {
                texture.Apply();
            }
        }
    }

    public static partial class Texture2DExtension
    {
        public static Texture2D DrawLine(this Texture2D texture, Vector2 p1, Vector2 p2, Color color, int size = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawLine(texture, p1, p2, color, size, blendMode);
            return texture;
        }

        public static Texture2D DrawLine(this Texture2D texture, int x1, int y1, int x2, int y2, Color color, int size = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawLine(texture, x1, y1, x2, y2, color, size, blendMode);
            return texture;
        }
    }
}