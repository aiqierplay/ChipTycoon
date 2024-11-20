using System.Collections.Generic;
using UnityEngine;

namespace Aya.Render.Draw
{
    public static class DrawUtil
    {
        public const float DefaultChromaticAberration = 0.00390625f;

        internal static Vector2 EvaluateCurve(Vector2[] vectors, int index, float factor)
        {
            if (index > vectors.Length - 2) return vectors[vectors.Length - 1];
            var i = index - 1;
            var p1 = i < 0 ? vectors[0] * 2f - vectors[1] : vectors[i];
            var p2 = vectors[i + 1];
            var p3 = vectors[i + 2];
            var p4 = i >= vectors.Length - 3 ? vectors[i + 2] * 2f - vectors[i + 1] : vectors[i + 3];
            var pos = CatmullRom(p1, p2, p3, p4, factor);
            return pos;
        }

        internal static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            var factor = 0.5f;
            var c0 = p1;
            var c1 = (p2 - p0) * factor;
            var c2 = (p2 - p1) * 3f - (p3 - p1) * factor - (p2 - p0) * 2f * factor;
            var c3 = (p2 - p1) * -2f + (p3 - p1) * factor + (p2 - p0) * factor;
            var curvePoint = c3 * t * t * t + c2 * t * t + c1 * t + c0;
            return curvePoint;
        }

        internal static int DistanceSquare(int x1, int y1, int x2, int y2)
        {
            var result = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            return result;
        }

        internal static bool CheckInPolygon(Vector2 point, Vector2[] points)
        {
            int i, j, c = 0;
            for (i = 0, j = points.Length - 1; i < points.Length; j = i++)
            {
                if (((points[i].y > point.y) != (points[j].y > point.y)) && (point.x < (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
                {
                    c = 1 + c;
                }
            }

            return c % 2 != 0;
        }

        internal static bool ColorEquals(Color c1, Color c2, float chromaticAberration = DefaultChromaticAberration)
        {
            var r = Mathf.Abs(c1.r - c2.r) <= chromaticAberration;
            var g = Mathf.Abs(c1.g - c2.g) <= chromaticAberration;
            var b = Mathf.Abs(c1.b - c2.b) <= chromaticAberration;
            return r && g && b;
        }

        internal static bool ColorEquals(Texture2D texture, int x , int y, Color c2, float chromaticAberration = DefaultChromaticAberration)
        {
            var color = texture.GetPixel(x, y);
            return ColorEquals(color, c2, chromaticAberration);
        }

        internal static bool ColorEquals(TextureDrawBatch batch, int x, int y, Color c2, float chromaticAberration = DefaultChromaticAberration)
        {
            var color = batch.GetPixel(x, y);
            return ColorEquals(color, c2, chromaticAberration);
        }

        internal static void Switch<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}
