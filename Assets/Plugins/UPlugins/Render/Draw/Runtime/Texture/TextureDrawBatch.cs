using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Aya.Render.Draw
{
    [Serializable]
    public partial class TextureDrawBatch : IDisposable
    {
        internal static Dictionary<Texture2D, TextureDrawBatch> BatchDataDic = new Dictionary<Texture2D, TextureDrawBatch>();

        public bool UpdateMipMaps = false;
        public Texture2D Texture;
        public int Width;
        public int Height;

        public Color32[] DrawColors;

        public TextureDrawBatch(Texture2D texture)
        {
            Texture = texture;
            Width = texture.width;
            Height = texture.height;
            DrawColors = Texture.GetPixels32();
        }

        public void BeginDraw()
        {

        }

        public void EndDraw()
        {
            Texture.SetPixels32(DrawColors);
            Texture.Apply(UpdateMipMaps);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Color32 GetPixel(int x, int y)
        {
            var result = DrawColors[y * Width + x];
            return result;
        }

        internal void SetPixel(int x, int y, Color32 color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            if (blendMode != ColorBlendMode.None)
            {
                var dstColor = GetPixel(x, y);
                color = ColorBlendUtil.Blend(color, dstColor, blendMode);
            }

            DrawColors[y * Width + x] = color;
        }

        internal void SyncPixels(int x, int y, int width, int height)
        {
            SyncPixels(x, y, width, height, DrawColors);
        }

        internal void SyncPixels(int x, int y, int width, int height, Color32[] colors)
        {
            if (x == 0 && width == Width)
            {
                Array.Copy(colors, y * Width, DrawColors, y * Width, width * height);
            }
            else
            {
                for (var j = y; j < y + height; j++)
                {
                    Array.Copy(colors, j * Width + x, DrawColors, j * Width + x, width);
                }
            }
        }

        public void Dispose()
        {
            Texture = null;
            Width = 0;
            Height = 0;
            DrawColors = null;
        }

        #region Check

        public bool CheckOutOfRange(int x, int y)
        {
            var result = x < 0 || x >= Width || y < 0 || y >= Height;
            return result;
        }

        public bool CheckOutOfRange(ref int x, ref int y)
        {
            var mode = TextureDraw.RepeatMode;
            if (mode == TextureDrawRepeatMode.InRange)
            {
                var result = x < 0 || x >= Width || y < 0 || y >= Height;
                return result;
            }
            else
            {
                if (mode == TextureDrawRepeatMode.RepeatX)
                {
                    if (y < 0 || y >= Height) return true;
                }

                if (mode == TextureDrawRepeatMode.RepeatY)
                {
                    if (x < 0 || x >= Width) return true;
                }

                if (mode == TextureDrawRepeatMode.RepeatX || mode == TextureDrawRepeatMode.RepeatXY)
                {
                    while (x < 0) x += Width;
                    while (x >= Width) x -= Width;
                }

                if (mode == TextureDrawRepeatMode.RepeatY || mode == TextureDrawRepeatMode.RepeatXY)
                {
                    while (y < 0) y += Height;
                    while (y >= Height) y -= Height;
                }

                return false;
            }
        }

        #endregion

        #region Static

        public static bool IsInBatch(Texture2D texture)
        {
            var exist = BatchDataDic.ContainsKey(texture);
            return exist;
        }

        public static TextureDrawBatch BeginDraw(Texture2D texture)
        {
            if (!BatchDataDic.TryGetValue(texture, out var drawTextureBatch))
            {
                drawTextureBatch = new TextureDrawBatch(texture);
                drawTextureBatch.BeginDraw();
                BatchDataDic.Add(texture, drawTextureBatch);
            }

            return drawTextureBatch;
        }

        public static void EndDraw(Texture2D texture)
        {
            if (BatchDataDic.TryGetValue(texture, out var drawTextureBatch))
            {
                drawTextureBatch.EndDraw();
                BatchDataDic.Remove(texture);
            }
        }

        #endregion
    }
}