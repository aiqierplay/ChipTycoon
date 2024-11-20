using UnityEngine;

namespace Aya.Render.Draw
{
    public static partial class TextureDraw
    {
        public static void DrawPolygon(Texture2D texture, Vector2[] points, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            if (points.Length < 3) return;
            for (var i = 0; i < points.Length; i++)
            {
                var p1 = i;
                var p2 = i + 1;
                if (i == points.Length - 1)
                {
                    p1 = i;
                    p2 = 0;
                }

                var x1 = points[p1].x;
                var y1 = points[p1].y;
                var x2 = points[p2].x;
                var y2 = points[p2].y;
                DrawLine(texture, (int)x1, (int)y1, (int)x2, (int)y2, color, 1, blendMode);
            }
        }

        public static void FillPolygon(Texture2D texture, Vector2[] points, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            if (points.Length < 3) return;
            DrawPolygon(texture, points, color, blendMode);
            var isBatch = texture.IsInDrawBatch(out var batch);
            var step = 2;
            for (var i = 0; i < points.Length; i++)
            {
                var point = points[i];
                var x = (int) point.x;
                var y = (int) point.y;
                var neighbors = new[]
                {
                    new Vector2(x + step, y),
                    new Vector2(x - step, y),
                    new Vector2(x, y + step),
                    new Vector2(x, y - step),
                    new Vector2(x + step, y + step),
                    new Vector2(x - step, y - step),
                    new Vector2(x - step, y + step),
                    new Vector2(x + step, y - step),
                };

                for (var j = 0; j < neighbors.Length; j++)
                {
                    var neighbor = neighbors[j];
                    var xx = (int) neighbor.x;
                    var yy = (int) neighbor.y;

                    if (isBatch)
                    {
                        if (batch.CheckOutOfRange(xx, yy)) continue;
                    }
                    else
                    {
                        if (texture.CheckInRange(xx, yy)) continue;
                    }

                    if (DrawUtil.CheckInPolygon(neighbor, points))
                    {
                        var neighborColor = isBatch ? (Color) batch.GetPixel(xx, yy) : texture.GetPixel(xx, yy);
                        if (DrawUtil.ColorEquals(color, neighborColor)) continue;
                        FillWithBorder(texture, xx, yy, color, color, blendMode);
                    }
                }
            }
        }
    }

    public static partial class Texture2DExtension
    {
        public static Texture2D DrawPolygon(this Texture2D texture, Vector2[] points, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.DrawPolygon(texture, points, color, blendMode);
            return texture;
        }

        public static Texture2D FillPolygon(this Texture2D texture, Vector2[] points, Color color, ColorBlendMode blendMode = ColorBlendMode.None)
        {
            TextureDraw.FillPolygon(texture, points, color, blendMode);
            return texture;
        }
    }
}