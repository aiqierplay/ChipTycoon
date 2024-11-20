using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void DrawCurve(Texture2D texture, Vector2[] points, Color color, float accuracy = 0.1f, int size = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            var isBatch = texture.IsInDrawBatch(out var batch);
            var step = Mathf.Clamp(accuracy, 0.01f, 1f);
            for (var i = 0; i < points.Length - 1; i++)
            {
                for (var t = 0f; t <= 1f; t += step)
                {
                    var v1 = DrawUtil.EvaluateCurve(points, i, t);
                    var v2 = DrawUtil.EvaluateCurve(points, i, t + step > 1f ? 1f : t + step);
                    DrawLine(texture, (int)v1.x, (int)v1.y, (int)v2.x, (int)v2.y, color, size, blendMode);
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
        public static Texture2D DrawCurve(this Texture2D texture, Vector2[] points, Color color, float accuracy = 0.1f, int size = 1, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawCurve(texture, points, color, accuracy, size, blendMode);
            return texture;
        }
    }
}