using System;
using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void Clear(Texture2D texture)
        {
            texture.Clear(Color.clear);
        }

        public static void Clear(Texture2D texture, Color color)
        {
            var isBatch = texture.IsInDrawBatch(out var batch);
            var width = texture.width;
            var height = texture.height;
            if (isBatch)
            {
                // 逐个填充第一行，然后复制其他行
                Color32 batchColor = color;
                for (var i = 0; i < width; i++)
                {
                    batch.DrawColors[i] = batchColor;
                }

                for (var j = 1; j < height; j++)
                {
                    Array.Copy(batch.DrawColors, 0, batch.DrawColors, j * width, width);
                }

                batch.SyncPixels(0, 0, width, height);
            }
            else
            {
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        texture.SetPixel(i, j, color);
                    }
                }

                texture.Apply();
            }
        }
    }

    public static partial class Texture2DExtension
    {
        public static Texture2D Clear(this Texture2D texture)
        {
            TextureDraw.Clear(texture);
            return texture;
        }

        public static Texture2D Clear(this Texture2D texture, Color color)
        {
            TextureDraw.Clear(texture, color);
            return texture;
        }
    }
}