using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;
// https://www.jianshu.com/p/175631f45ec6

namespace Aya.Render.Draw
{
    public enum ColorBlendMode
    {
        None = -1,              // 不混合

        // 普通
        Normal = 0,             // 正常
        Dissolve = 1,           // 溶解

        // 加深
        Darken = 2,             // 变暗
        Multiply = 3,           // 正片叠底
        ColorBurn = 4,          // 颜色加深
        LinearBurn = 5,         // 线性加深
        DarkerColor = 6,        // 深色

        // 减淡
        Lighten = 7,            // 变亮
        Screen = 8,             // 滤色
        ColorDodge = 9,         // 颜色减淡
        LinearDodge = 10,       // 线性减淡
        LighterColor = 11,      // 浅色

        // 对比
        Overlay = 12,           // 叠加
        SoftLight = 13,         // 柔光
        HardLight = 14,         // 强光
        VividLight = 15,        // 亮光
        LinearLight = 16,       // 线性光
        PinLight = 17,          // 点光
        HardMix = 18,           // 实色混合

        // 差集
        Difference = 19,        // 插值
        Exclusion = 20,         // 排除
        Subtract = 21,          // 减去
        Divide = 22,            // 划分

        // 色彩
        Hue = 23,               // 色调
        Saturation = 24,        // 饱和度
        Color = 25,             // 颜色
        Luminosity = 26,        // 亮度
    }

    public static class ColorBlendUtil
    {
        /// <summary>
        /// 颜色混合
        /// </summary>
        /// <param name="srcColor">混合颜色</param>
        /// <param name="dstColor">底图颜色</param>
        /// <param name="blendMode">混合模式</param>
        /// <returns>结果</returns>
        public static Color Blend(Color srcColor, Color dstColor, ColorBlendMode blendMode)
        {
            switch (blendMode)
            {
                case ColorBlendMode.None: return srcColor;

                case ColorBlendMode.Normal: return Normal(srcColor, dstColor);
                case ColorBlendMode.Dissolve: return Dissolve(srcColor, dstColor);

                case ColorBlendMode.Darken: return Darken(srcColor, dstColor);
                case ColorBlendMode.Multiply: return Multiply(srcColor, dstColor);
                case ColorBlendMode.ColorBurn: return ColorBurn(srcColor, dstColor);
                case ColorBlendMode.LinearBurn: return LinearBurn(srcColor, dstColor);
                case ColorBlendMode.DarkerColor: return DarkerColor(srcColor, dstColor);

                case ColorBlendMode.Lighten: return Lighten(srcColor, dstColor);
                case ColorBlendMode.Screen: return Screen(srcColor, dstColor);
                case ColorBlendMode.ColorDodge: return ColorDodge(srcColor, dstColor);
                case ColorBlendMode.LinearDodge: return LinearDodge(srcColor, dstColor);
                case ColorBlendMode.LighterColor: return LighterColor(srcColor, dstColor);

                case ColorBlendMode.Overlay: return Overlay(srcColor, dstColor);
                case ColorBlendMode.SoftLight: return SoftLight(srcColor, dstColor);
                case ColorBlendMode.HardLight: return HardLight(srcColor, dstColor);
                case ColorBlendMode.VividLight: return VividLight(srcColor, dstColor);
                case ColorBlendMode.LinearLight: return LinearLight(srcColor, dstColor);
                case ColorBlendMode.PinLight: return PinLight(srcColor, dstColor);
                case ColorBlendMode.HardMix: return HardMix(srcColor, dstColor);

                case ColorBlendMode.Difference: return Difference(srcColor, dstColor);
                case ColorBlendMode.Exclusion: return Exclusion(srcColor, dstColor);
                case ColorBlendMode.Subtract: return Subtract(srcColor, dstColor);
                case ColorBlendMode.Divide: return Divide(srcColor, dstColor);

                case ColorBlendMode.Hue: return Hue(srcColor, dstColor);
                case ColorBlendMode.Saturation: return Saturation(srcColor, dstColor);
                case ColorBlendMode.Color: return Color(srcColor, dstColor);
                case ColorBlendMode.Luminosity: return Luminosity(srcColor, dstColor);
            }

            return srcColor;
        }

        /// <summary>
        /// 正常
        /// </summary>
        public static Color Normal(Color srcColor, Color dstColor)
        {
            var result = dstColor * (1f - srcColor.a) + srcColor * srcColor.a;
            return result;
        }

        /// <summary>
        /// 溶解
        /// </summary>
        public static Color Dissolve(Color srcColor, Color dstColor)
        {
            var result = Random.value > 0.5f ? srcColor : dstColor;
            return result;
        }
        
        /// <summary>
        /// 变暗
        /// </summary>
        public static Color Darken(Color srcColor, Color dstColor)
        {
            var r = Mathf.Min(srcColor.r, dstColor.r);
            var g = Mathf.Min(srcColor.g, dstColor.g);
            var b = Mathf.Min(srcColor.b, dstColor.b);
            var a = Mathf.Min(srcColor.a, dstColor.a);
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 正片叠底
        /// </summary>
        public static Color Multiply(Color srcColor, Color dstColor)
        {
            var result = srcColor * dstColor;
            return result;
        }

        /// <summary>
        /// 颜色加深
        /// </summary>
        public static Color ColorBurn(Color srcColor, Color dstColor)
        {
            var r = srcColor.r - (1f - srcColor.r) * (1f - dstColor.r) / dstColor.r;
            var g = srcColor.g - (1f - srcColor.g) * (1f - dstColor.g) / dstColor.g;
            var b = srcColor.b - (1f - srcColor.b) * (1f - dstColor.b) / dstColor.b;
            var a = srcColor.a - (1f - srcColor.a) * (1f - dstColor.a) / dstColor.a;
            var result = new Color(r, g, b, a);
            return result;
        }
        
        /// <summary>
        /// 线性加深
        /// </summary>
        public static Color LinearBurn(Color srcColor, Color dstColor)
        {
            var r = srcColor.r + dstColor.r - 1f;
            var g = srcColor.g + dstColor.g - 1f;
            var b = srcColor.b + dstColor.b - 1f;
            var a = srcColor.a + dstColor.a - 1f;
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 加深
        /// </summary>
        public static Color DarkerColor(Color srcColor, Color dstColor)
        {
            var result = srcColor.r + srcColor.g + srcColor.b < dstColor.r + dstColor.g + dstColor.b ? srcColor : dstColor;
            return result;
        }

        /// <summary>
        /// 变亮
        /// </summary>
        public static Color Lighten(Color srcColor, Color dstColor)
        {
            var r = Mathf.Max(srcColor.r, dstColor.r);
            var g = Mathf.Max(srcColor.g, dstColor.g);
            var b = Mathf.Max(srcColor.b, dstColor.b);
            var a = Mathf.Max(srcColor.a, dstColor.a);
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 滤色
        /// </summary>
        public static Color Screen(Color srcColor, Color dstColor)
        {
            var r = 1f - (1f - srcColor.r) * (1f - dstColor.r);
            var g = 1f - (1f - srcColor.g) * (1f - dstColor.g);
            var b = 1f - (1f - srcColor.b) * (1f - dstColor.b);
            var a = 1f - (1f - srcColor.a) * (1f - dstColor.a);
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 颜色减淡
        /// </summary>
        public static Color ColorDodge(Color srcColor, Color dstColor)
        {
            var r = srcColor.r + (srcColor.r * dstColor.r) / (1f - dstColor.r);
            var g = srcColor.g + (srcColor.g * dstColor.g) / (1f - dstColor.g);
            var b = srcColor.b + (srcColor.b * dstColor.b) / (1f - dstColor.b);
            var a = srcColor.a + (srcColor.a * dstColor.a) / (1f - dstColor.a);
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 线性减淡
        /// </summary>
        public static Color LinearDodge(Color srcColor, Color dstColor)
        {
            var result = srcColor + dstColor;
            return result;
        }

        /// <summary>
        /// 减淡
        /// </summary>
        /// <param name="srcColor"></param>
        /// <param name="dstColor"></param>
        /// <returns></returns>
        public static Color LighterColor(Color srcColor, Color dstColor)
        {
            var result = srcColor.r + srcColor.g + srcColor.b > dstColor.r + dstColor.g + dstColor.b ? srcColor : dstColor;
            return result;
        }

        /// <summary>
        /// 叠加
        /// </summary>
        public static Color Overlay(Color srcColor, Color dstColor)
        {
            var r = srcColor.r * dstColor.r * 2f + (1f - Step(srcColor.r, 0.5f)) * (1f - (1f - srcColor.r) * (1f - dstColor.r) * 2f);
            var g = srcColor.g * dstColor.g * 2f + (1f - Step(srcColor.g, 0.5f)) * (1f - (1f - srcColor.g) * (1f - dstColor.g) * 2f);
            var b = srcColor.b * dstColor.b * 2f + (1f - Step(srcColor.b, 0.5f)) * (1f - (1f - srcColor.b) * (1f - dstColor.b) * 2f);
            var a = srcColor.a * dstColor.a * 2f + (1f - Step(srcColor.a, 0.5f)) * (1f - (1f - srcColor.a) * (1f - dstColor.a) * 2f);
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 柔光
        /// </summary>
        public static Color SoftLight(Color srcColor, Color dstColor)
        {
            var r = Step(dstColor.r, 0.5f) * (srcColor.r * dstColor.r * 2f + srcColor.r * srcColor.r * (1f - dstColor.r * 2f)) + (1f - Step(dstColor.r, 0.5f)) * (srcColor.r * (1f - dstColor.r) * 2f + Mathf.Sqrt(srcColor.r) * (2f * dstColor.r - 1f));
            var g = Step(dstColor.g, 0.5f) * (srcColor.g * dstColor.g * 2f + srcColor.g * srcColor.g * (1f - dstColor.g * 2f)) + (1f - Step(dstColor.g, 0.5f)) * (srcColor.g * (1f - dstColor.g) * 2f + Mathf.Sqrt(srcColor.g) * (2f * dstColor.g - 1f));
            var b = Step(dstColor.b, 0.5f) * (srcColor.b * dstColor.b * 2f + srcColor.b * srcColor.b * (1f - dstColor.b * 2f)) + (1f - Step(dstColor.b, 0.5f)) * (srcColor.b * (1f - dstColor.b) * 2f + Mathf.Sqrt(srcColor.b) * (2f * dstColor.b - 1f));
            var a = Step(dstColor.a, 0.5f) * (srcColor.a * dstColor.a * 2f + srcColor.a * srcColor.a * (1f - dstColor.a * 2f)) + (1f - Step(dstColor.a, 0.5f)) * (srcColor.a * (1f - dstColor.a) * 2f + Mathf.Sqrt(srcColor.a) * (2f * dstColor.a - 1f));
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 强光
        /// </summary>
        public static Color HardLight(Color srcColor, Color dstColor)
        {
            var r = Step(dstColor.r, 0.5f) * srcColor.r * dstColor.r * 2f + (1f - Step(dstColor.r, 0.5f)) * (1f - (1f - srcColor.r) * (1f - dstColor.r) * 2f);
            var g = Step(dstColor.g, 0.5f) * srcColor.g * dstColor.g * 2f + (1f - Step(dstColor.g, 0.5f)) * (1f - (1f - srcColor.g) * (1f - dstColor.g) * 2f);
            var b = Step(dstColor.b, 0.5f) * srcColor.b * dstColor.b * 2f + (1f - Step(dstColor.b, 0.5f)) * (1f - (1f - srcColor.b) * (1f - dstColor.b) * 2f);
            var a = Step(dstColor.a, 0.5f) * srcColor.a * dstColor.a * 2f + (1f - Step(dstColor.a, 0.5f)) * (1f - (1f - srcColor.a) * (1f - dstColor.a) * 2f);
            var result = new Color(r, g, b, a);
            return result;
        }


        /// <summary>
        /// 亮光
        /// </summary>
        public static Color VividLight(Color srcColor, Color dstColor)
        {
            var r = Step(dstColor.r, 0.5f) * (srcColor.r - (1f - srcColor.r) * (1f - 2f * dstColor.r) / (2f * dstColor.r)) + (1f - Step(dstColor.r, 0.5f)) * (srcColor.r * srcColor.r * (2f * dstColor.r - 1f) / (2f * (1f - dstColor.r)));
            var g = Step(dstColor.g, 0.5f) * (srcColor.g - (1f - srcColor.g) * (1f - 2f * dstColor.g) / (2f * dstColor.g)) + (1f - Step(dstColor.g, 0.5f)) * (srcColor.g * srcColor.g * (2f * dstColor.g - 1f) / (2f * (1f - dstColor.g)));
            var b = Step(dstColor.b, 0.5f) * (srcColor.b - (1f - srcColor.b) * (1f - 2f * dstColor.b) / (2f * dstColor.b)) + (1f - Step(dstColor.b, 0.5f)) * (srcColor.b * srcColor.b * (2f * dstColor.b - 1f) / (2f * (1f - dstColor.b)));
            var a = Step(dstColor.a, 0.5f) * (srcColor.a - (1f - srcColor.a) * (1f - 2f * dstColor.a) / (2f * dstColor.a)) + (1f - Step(dstColor.a, 0.5f)) * (srcColor.a * srcColor.a * (2f * dstColor.a - 1f) / (2f * (1f - dstColor.a)));
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 线性光
        /// </summary>
        public static Color LinearLight(Color srcColor, Color dstColor)
        {
            var r = srcColor.r + 2f * dstColor.r - 1f;
            var g = srcColor.g + 2f * dstColor.g - 1f;
            var b = srcColor.b + 2f * dstColor.b - 1f;
            var a = srcColor.a + 2f * dstColor.a - 1f;
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 点光
        /// </summary>
        public static Color PinLight(Color srcColor, Color dstColor)
        {
            var r = Step(dstColor.r, 0.5f) * (Mathf.Min(srcColor.r, 2f * dstColor.r) + (1f - Step(dstColor.r, 0.5f)) * (Mathf.Max(srcColor.r, (dstColor.r * 2f - 1f))));
            var g = Step(dstColor.g, 0.5f) * (Mathf.Min(srcColor.g, 2f * dstColor.g) + (1f - Step(dstColor.g, 0.5f)) * (Mathf.Max(srcColor.g, (dstColor.g * 2f - 1f))));
            var b = Step(dstColor.b, 0.5f) * (Mathf.Min(srcColor.b, 2f * dstColor.b) + (1f - Step(dstColor.b, 0.5f)) * (Mathf.Max(srcColor.b, (dstColor.b * 2f - 1f))));
            var a = Step(dstColor.a, 0.5f) * (Mathf.Min(srcColor.a, 2f * dstColor.a) + (1f - Step(dstColor.a, 0.5f)) * (Mathf.Max(srcColor.a, (dstColor.a * 2f - 1f))));
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 实色混合
        /// </summary>
        public static Color HardMix(Color srcColor, Color dstColor)
        {
            var r = Step(srcColor.r + dstColor.r, 1f);
            var g = Step(srcColor.g + dstColor.g, 1f);
            var b = Step(srcColor.b + dstColor.b, 1f);
            var a = Step(srcColor.a + dstColor.a, 1f);
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 插值
        /// </summary>
        public static Color Difference(Color srcColor, Color dstColor)
        {
            var r = Mathf.Abs(srcColor.r - dstColor.r);
            var g = Mathf.Abs(srcColor.g - dstColor.g);
            var b = Mathf.Abs(srcColor.b - dstColor.b);
            var a = Mathf.Abs(srcColor.a - dstColor.a);
            var result = new Color(r, g, b, a);
           
            return result;
        }

        /// <summary>
        /// 排除
        /// </summary>
        public static Color Exclusion(Color srcColor, Color dstColor)
        {
            var r = srcColor.r + dstColor.r - srcColor.r * dstColor.r * 2f;
            var g = srcColor.g + dstColor.g - srcColor.g * dstColor.g * 2f;
            var b = srcColor.b + dstColor.b - srcColor.b * dstColor.b * 2f;
            var a = srcColor.a + dstColor.a - srcColor.a * dstColor.a * 2f;
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 减去
        /// </summary>
        public static Color Subtract(Color srcColor, Color dstColor)
        {
            var result = srcColor - dstColor;
            return result;
        }

        /// <summary>
        /// 划分
        /// </summary>
        public static Color Divide(Color srcColor, Color dstColor)
        {
            var r = srcColor.r / dstColor.r;
            var g = srcColor.g / dstColor.g;
            var b = srcColor.b / dstColor.b;
            var a = srcColor.a / dstColor.a;
            var result = new Color(r, g, b, a);
            return result;
        }

        /// <summary>
        /// 色相
        /// </summary>
        public static Color Hue(Color srcColor, Color dstColor)
        {
            UnityEngine.Color.RGBToHSV(srcColor, out var aH, out var aS, out var aV);
            UnityEngine.Color.RGBToHSV(dstColor, out var bH, out var bS, out var bV);
            var h = aH;
            var s = bS;
            var v = bV;
            var result = UnityEngine.Color.HSVToRGB(h, s, v);
            return result;
        }

        /// <summary>
        /// 饱和度
        /// </summary>
        public static Color Saturation(Color srcColor, Color dstColor)
        {
            UnityEngine.Color.RGBToHSV(srcColor, out var aH, out var aS, out var aV);
            UnityEngine. Color.RGBToHSV(dstColor, out var bH, out var bS, out var bV);
            var h = bH;
            var s = aS;
            var v = bV;
            var result = UnityEngine.Color.HSVToRGB(h, s, v);
            return result;
        }

        /// <summary>
        /// 色相
        /// </summary>
        public static Color Color(Color srcColor, Color dstColor)
        {
            UnityEngine.Color.RGBToHSV(srcColor, out var aH, out var aS, out var aV);
            UnityEngine.Color.RGBToHSV(dstColor, out var bH, out var bS, out var bV);
            var h = aH;
            var s = aS;
            var v = bV;
            var result = UnityEngine.Color.HSVToRGB(h, s, v);
            return result;
        }

        /// <summary>
        /// 色相
        /// </summary>
        public static Color Luminosity(Color srcColor, Color dstColor)
        {
            UnityEngine.Color.RGBToHSV(srcColor, out var aH, out var aS, out var aV);
            UnityEngine.Color.RGBToHSV(dstColor, out var bH, out var bS, out var bV);
            var h = bH;
            var s = bS;
            var v = aV;
            var result = UnityEngine.Color.HSVToRGB(h, s, v);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Step(float a, float b)
        {
            return a < b ? 0f : 1f;
        }
    }
}
