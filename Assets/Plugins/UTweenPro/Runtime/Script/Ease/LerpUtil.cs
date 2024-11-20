using System.Runtime.CompilerServices;
using UnityEngine;

namespace Aya.TweenPro
{
    public static class LerpUtil
    {
        #region Lerp Float

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LerpUnclamped(float from, float to, float factor)
        {
            var result = from + (to - from) * factor;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float from, float to, float factor)
        {
            if (factor < 0) factor = 0;
            if (factor > 1) factor = 1;
            var result = from + (to - from) * factor;
            return result;
        }

        #endregion

        #region Lerp Vector2

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 LerpUnclamped(Vector2 from, Vector2 to, float factor)
        {
            var x = from.x + (to.x - from.x) * factor;
            var y = from.y + (to.y - from.y) * factor;
            var result = new Vector3(x, y);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(Vector2 from, Vector2 to, float factor)
        {
            if (factor < 0) factor = 0;
            if (factor > 1) factor = 1;
            var x = from.x + (to.x - from.x) * factor;
            var y = from.y + (to.y - from.y) * factor;
            var result = new Vector3(x, y);
            return result;
        }

        #endregion

        #region Lerp Vector3

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 LerpUnclamped(Vector3 from, Vector3 to, float factor)
        {
            var x = from.x + (to.x - from.x) * factor;
            var y = from.y + (to.y - from.y) * factor;
            var z = from.z + (to.z - from.z) * factor;
            var result = new Vector3(x, y, z);
            return result;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 from, Vector3 to, float factor)
        {
            if (factor < 0) factor = 0;
            if (factor > 1) factor = 1;
            var x = from.x + (to.x - from.x) * factor;
            var y = from.y + (to.y - from.y) * factor;
            var z = from.z + (to.z - from.z) * factor;
            var result = new Vector3(x, y, z);
            return result;
        }

        #endregion

        #region Lerp Vector4

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 LerpUnclamped(Vector4 from, Vector4 to, float factor)
        {
            var x = from.x + (to.x - from.x) * factor;
            var y = from.y + (to.y - from.y) * factor;
            var z = from.z + (to.z - from.z) * factor;
            var w = from.w + (to.w - from.w) * factor;
            var result = new Vector4(x, y, z, w);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Lerp(Vector4 from, Vector4 to, float factor)
        {
            if (factor < 0) factor = 0;
            if (factor > 1) factor = 1;
            var x = from.x + (to.x - from.x) * factor;
            var y = from.y + (to.y - from.y) * factor;
            var z = from.z + (to.z - from.z) * factor;
            var w = from.w + (to.w - from.w) * factor;
            var result = new Vector4(x, y, z, w);
            return result;
        }

        #endregion

        #region Lerp Angle

        public static float LerpAngleUnclamped(float from, float to, float factor)
        {
            var num = Mathf.Repeat(to - from, 360f);
            if (num > 180f)
            {
                num -= 360f;
            }

            var result = from + num * factor;
            return result;
        }

        public static float LerpAngle(float from, float to, float factor)
        {
            var num = Mathf.Repeat(to - from, 360f);
            if (num > 180f)
            {
                num -= 360f;
            }

            if (factor < 0) factor = 0;
            if (factor > 1) factor = 1;
            var result = from + num * factor;
            return result;
        }

        public static Vector2 LerpAngleUnclamped(Vector2 from, Vector2 to, float factor)
        {
            var x = LerpAngleUnclamped(from.x, to.x, factor);
            var y = LerpAngleUnclamped(from.y, to.y, factor);
            return new Vector2(x, y);
        }

        public static Vector2 LerpAngle(Vector2 from, Vector2 to, float factor)
        {
            var x = LerpAngle(from.x, to.x, factor);
            var y = LerpAngle(from.y, to.y, factor);
            return new Vector2(x, y);
        }


        public static Vector3 LerpAngleUnclamped(Vector3 from, Vector3 to, float factor)
        {
            var x = LerpAngleUnclamped(from.x, to.x, factor);
            var y = LerpAngleUnclamped(from.y, to.y, factor);
            var z = LerpAngleUnclamped(from.z, to.z, factor);
            return new Vector3(x, y, z);
        }

        public static Vector3 LerpAngle(Vector3 from, Vector3 to, float factor)
        {
            var x = LerpAngle(from.x, to.x, factor);
            var y = LerpAngle(from.y, to.y, factor);
            var z = LerpAngle(from.z, to.z, factor);
            return new Vector3(x, y, z);
        }

        #endregion

        #region Lerp Color

        public static Color LerpColorHsvUnclamped(Color from, Color to, float factor)
        {
            Color.RGBToHSV(from, out var fH, out var fS, out var fV);
            Color.RGBToHSV(to, out var tH, out var tS, out var tV);
            var color = Color.HSVToRGB(
                Mathf.LerpUnclamped(fH, tH, factor),
                Mathf.LerpUnclamped(fS, tS, factor),
                Mathf.LerpUnclamped(fV, tV, factor));
            var alpha = Mathf.LerpUnclamped(from.a, to.a, factor);
            var result = new Color(color.r, color.g, color.b, alpha);
            return result;
        }

        #endregion
    }
}
