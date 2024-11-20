using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void DrawPoint(Texture2D texture, int x, int y, Color color, int size = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var isBatch = texture.IsInDrawBatch(out var batch);
            if (size < 1) return;
            if (size == 1)
            {
             
                if (isBatch)
                {
                    if (batch.CheckOutOfRange(ref x, ref y)) return;
                    batch.SetPixel(x, y, color, blendMode);
                }
                else
                {
                    if (texture.CheckOutOfRange(ref x, ref y)) return;
                    texture.SetPixel(x, y, color, blendMode);
                    texture.Apply();
                }
            }
            else
            {
                FillCircle(texture, x, y, size, color, blendMode);
            }
        }
    }

    public static partial class Texture2DExtension
    {
        public static Texture2D DrawPoint(this Texture2D texture, int x, int y, Color color, int size = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawPoint(texture, x, y, color, size, blendMode);
            return texture;
        }
    }
}
