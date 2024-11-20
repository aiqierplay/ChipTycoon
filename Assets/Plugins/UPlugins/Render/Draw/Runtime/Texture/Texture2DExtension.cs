using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class Texture2DExtension 
    {
        public static Texture2D BeginDraw(this Texture2D texture)
        {
            TextureDrawBatch.BeginDraw(texture);
            return texture;
        }

        public static Texture2D EndDraw(this Texture2D texture)
        {
            TextureDrawBatch.EndDraw(texture);
            return texture;
        }

        public static bool IsInDrawBatch(this Texture2D texture)
        {
            var result = TextureDrawBatch.IsInBatch(texture);
            return result;
        }

        public static bool IsInDrawBatch(this Texture2D texture, out TextureDrawBatch textureDrawBatch)
        {
            if (TextureDrawBatch.BatchDataDic.TryGetValue(texture, out var tempBatch))
            {
                textureDrawBatch = tempBatch;
                return true;
            }
            else
            {
                textureDrawBatch = null;
                return false;
            }
        }

        public static bool CheckInRange(this Texture2D texture, int x, int y)
        {
            var result = x >= 0 || x < texture.width || y >= 0 || y < texture.height;
            return result;
        }

        public static bool CheckOutOfRange(this Texture2D texture, int x, int y)
        {
            var result = x < 0 || x >= texture.width || y < 0 || y >= texture.height;
            return result;
        }

        public static bool CheckOutOfRange(this Texture2D texture, ref int x, ref int y)
        {
            var mode = TextureDraw.RepeatMode;
            if (mode == TextureDrawRepeatMode.InRange)
            {
                var result = x < 0 || x >= texture.width || y < 0 || y >= texture.height;
                return result;
            }
            else 
            {
                if (mode == TextureDrawRepeatMode.RepeatX)
                {
                    if (y < 0 || y >= texture.height) return true;
                }

                if (mode == TextureDrawRepeatMode.RepeatY)
                {
                    if (x < 0 || x >= texture.width) return true;
                }

                if (mode == TextureDrawRepeatMode.RepeatX || mode == TextureDrawRepeatMode.RepeatXY)
                {
                    while (x < 0) x += texture.width;
                    while (x >= texture.width) x -= texture.width;
                }

                if (mode == TextureDrawRepeatMode.RepeatY || mode == TextureDrawRepeatMode.RepeatXY)
                {
                    while (y < 0) y += texture.height;
                    while (y >= texture.height) y -= texture.height;
                }

                return false;
            }
        }

        public static void ClampInRange(this Texture2D texture, ref int x, ref int y, ref int width, ref int height)
        {
            x = Mathf.Clamp(x, 0, texture.width);
            y = Mathf.Clamp(y, 0, texture.height);
            width = Mathf.Clamp(width, 0, texture.width - x);
            height = Mathf.Clamp(height, 0, texture.height - y);
        }

        public static void ClampInRange(this Texture2D texture, ref int x, ref int y)
        {
            x = Mathf.Clamp(x, 0, texture.width);
            y = Mathf.Clamp(y, 0, texture.height);
        }

        public static void SetPixel(this Texture2D texture, int x, int y, Color color, ColorBlendMode blendMode)
        {
            if (blendMode != ColorBlendMode.None)
            {
                var dstColor = texture.GetPixel(x, y);
                color = ColorBlendUtil.Blend(color, dstColor, blendMode);
            }

            texture.SetPixel(x, y, color);
        }
    }
}

