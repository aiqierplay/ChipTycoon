using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void DrawTexture(Texture2D texture, int destX, int destY, Texture2D source, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            DrawTexture(texture, destX, destY, source.width, source.height, source, 0, 0, source.width, source.height, blendMode);
        }

        public static void DrawTexture(Texture2D texture, int destX, int destY, int destWidth, int destHeight, Texture2D source, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            DrawTexture(texture, destX, destY, destWidth, destHeight, source, 0, 0, source.width, source.height, blendMode);
        }

        public static void DrawTexture(Texture2D texture, int destX, int destY, int destWidth, int destHeight, Texture2D source, int srcX, int srcY, int srcWidth, int srcHeight, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var isBatch = texture.IsInDrawBatch(out var batch);
            source.ClampInRange(ref srcX, ref srcY, ref srcWidth, ref srcHeight);

            if (isBatch)
            {
                var srcColors = source.GetPixels32();
                for (var i = destX; i < destX + destWidth; i++)
                {
                    for (var j = destY; j < destY + destHeight; j++)
                    {
                        var x = i;
                        var y = j;
                        if (batch.CheckOutOfRange(ref x, ref y)) continue;
                        var getX = (int)(srcX + (i - destX) * 1f / destWidth * srcWidth);
                        var getY = (int)(srcY + (j - destY) * 1f / destHeight * srcHeight);
                        var getIndex = getY * srcWidth + getX;
                        getIndex = Mathf.Clamp(getIndex, 0, srcColors.Length - 1);
                        var color = srcColors[getIndex];
                        batch.SetPixel(x, y, color, blendMode);
                    }
                }
            }
            else
            {
                var srcColors = source.GetPixels();
                for (var i = destX; i < destX + destWidth; i++)
                {
                    for (var j = destY; j < destY + destHeight; j++)
                    {
                        var x = i;
                        var y = j;
                        if (texture.CheckOutOfRange(ref x, ref y)) continue;
                        var getX = (int)(srcX + (i - destX) * 1f / destWidth * srcWidth);
                        var getY = (int)(srcY + (j - destY) * 1f / destHeight * srcHeight);
                        var getIndex = getY * srcWidth + getX;
                        getIndex = Mathf.Clamp(getIndex, 0, srcColors.Length - 1);
                        var color = srcColors[getIndex];
                        texture.SetPixel(x, y, color, blendMode);
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
        public static Texture2D DrawTexture(this Texture2D texture, int destX, int destY, Texture2D source, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawTexture(texture, destX, destY, source, blendMode);
            return texture;
        }

        public static Texture2D DrawTexture(this Texture2D texture, int destX, int destY, int destWidth, int destHeight, Texture2D source, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawTexture(texture, destX, destY, destWidth, destHeight, source, blendMode);
            return texture;
        }

        public static Texture2D DrawTexture(this Texture2D texture, int destX, int destY, int destWidth, int destHeight, Texture2D source, int srcX, int srcY, int srcWidth, int srcHeight, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawTexture(texture, destX, destY, destWidth, destHeight, source, srcX, srcY, srcWidth, srcHeight, blendMode);
            return texture;
        }
    }
}