using System;
using UnityEngine;

namespace Aya.Render.Draw
{
    public static class TextureUtil
    {
        public static Texture2D Copy(Texture2D source)
        {
            var texture = new Texture2D(source.width, source.height);
            var pixels = source.GetPixels();
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        public static Texture2D DrawTextureTrans(int x, int y, int width, int height, Texture2D source, Texture2D dest)
        {
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var originColor = source.GetPixel(i + x, j + y);
                    var drawColor = dest.GetPixel(i, j);
                    var retColor = drawColor * drawColor.a + originColor * (1f - drawColor.a);
                    source.SetPixel(i + x, j + y, retColor);
                }
            }

            source.Apply();
            return source;
        }

        public static Texture2D ClipBlank(Texture2D origin)
        {
            var left = 0;
            var top = 0;
            var right = origin.width;
            var bottom = origin.height;

            // Left
            for (var i = 0; i < origin.width; i++)
            {
                var find = false;
                for (var j = 0; j < origin.height; j++)
                {
                    var color = origin.GetPixel(i, j);
                    if (!(Math.Abs(color.a) > 1e-6)) continue;
                    find = true;
                    break;
                }

                if (!find) continue;
                left = i;
                break;
            }

            // Right
            for (var i = origin.width - 1; i >= 0; i--)
            {
                var find = false;
                for (var j = 0; j < origin.height; j++)
                {
                    var color = origin.GetPixel(i, j);
                    if (!(Math.Abs(color.a) > 1e-6)) continue;
                    find = true;
                    break;
                }

                if (!find) continue;
                right = i;
                break;
            }

            // Up
            for (var j = 0; j < origin.height; j++)
            {
                var find = false;
                for (var i = 0; i < origin.width; i++)
                {
                    var color = origin.GetPixel(i, j);
                    if (!(Math.Abs(color.a) > 1e-6)) continue;
                    find = true;
                    break;
                }

                if (!find) continue;
                top = j;
                break;
            }

            // Down
            for (var j = origin.height - 1; j >= 0; j--)
            {
                var find = false;
                for (var i = 0; i < origin.width; i++)
                {
                    var color = origin.GetPixel(i, j);
                    if (!(Math.Abs(color.a) > 1e-6)) continue;
                    find = true;
                    break;
                }

                if (!find) continue;
                bottom = j;
                break;
            }

            var width = right - left;
            var height = bottom - top;

            var result = new Texture2D(width, height, TextureFormat.ARGB32, false);
            var colors = origin.GetPixels(left, top, width, height);
            result.SetPixels(0, 0, width, height, colors);
            result.Apply();

            return result;
        }
    }
}
