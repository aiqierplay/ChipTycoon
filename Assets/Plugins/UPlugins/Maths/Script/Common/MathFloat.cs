/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathFloat.cs
//  Info     : 数学辅助计算类 —— float
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2022
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace Aya.Maths
{
    public static partial class MathUtil
    {
        #region Real
       
        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static bool IsReal(float value)
        {
            return !float.IsNaN(value) && !float.IsNegativeInfinity(value) && !float.IsPositiveInfinity(value);
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static bool IsReal(double value)
        {
            return !double.IsNaN(value) && !double.IsNegativeInfinity(value) && !double.IsPositiveInfinity(value);
        }

        #endregion

        #region Compare

        /// <summary>
        /// 按指定精度比较值是否相等
        /// </summary>
        /// <param name="a">值1</param>
        /// <param name="b">值2</param>
        /// <param name="epsilon">精度</param>
        /// <returns>结果</returns>
        public static bool FuzzyEqual(float a, float b, float epsilon = EpsilonSingle)
        {
            return Math.Abs(a - b) < epsilon;
        }

        /// <summary>
        /// 按指定精度比较是否小于
        /// </summary>
        /// <param name="a">值1</param>
        /// <param name="b">值2</param>
        /// <param name="epsilon">精度</param>
        /// <param name="exclusive">不包含</param>
        /// <returns>结果</returns>
        public static bool FuzzyLessThan(float a, float b, float epsilon, bool exclusive = true)
        {
            if (exclusive)
            {
                return a < b - epsilon;
            }

            return a < b + epsilon;
        }

        /// <summary>
        /// 按指定精度比较是否小于
        /// </summary>
        /// <param name="a">值1</param>
        /// <param name="b">值2</param>
        /// <param name="exclusive">不包含</param>
        /// <returns>结果</returns>
        public static bool FuzzyLessThan(float a, float b, bool exclusive = true)
        {
            return FuzzyLessThan(a, b, EpsilonSingle, exclusive);
        }

        /// <summary>
        /// 按指定精度比较是否大于
        /// </summary>
        /// <param name="a">值1</param>
        /// <param name="b">值2</param>
        /// <param name="epsilon">精度</param>
        /// <param name="exclusive">不包含</param>
        /// <returns>结果</returns>
        public static bool FuzzyGreaterThan(float a, float b, float epsilon, bool exclusive = true)
        {
            if (exclusive)
            {
                return a > b + epsilon;
            }

            return a > b - epsilon;
        }

        /// <summary>
        /// 按指定精度比较是否大于
        /// </summary>
        /// <param name="a">值1</param>
        /// <param name="b">值2</param>
        /// <param name="exclusive">不包含</param>
        /// <returns>结果</returns>
        public static bool FuzzyGreaterThan(float a, float b, bool exclusive = true)
        {
            return FuzzyGreaterThan(a, b, EpsilonSingle, exclusive);
        }

        #endregion

        #region Approx / Float Test

        /// <summary>
        /// 测试接近指定浮点的值（取决于浮点精度，默认阈值 1e-6）
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="about">靠近</param>
        /// <returns>结果</returns>
        public static bool Approx(float value, float about)
        {
            var result = Approx(value, about, FloatPrecision);
            return result;
        }

        /// <summary>
        /// 测试接近指定浮点的值（取决于浮点精度）
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="about">靠近</param>
        /// <param name="range">误差范围</param>
        /// <returns>结果</returns>
        public static bool Approx(float value, float about, float range)
        {
            var result = Math.Abs(value - about) < range;
            return result;
        }

        /// <summary>
        /// 测试接近制定向量的值（取决于浮点精度，默认阈值 1e-6）
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="about">靠近</param>
        /// <returns>结果</returns>
        public static bool Approx(Vector3 value, Vector3 about)
        {
            var result = Approx(value, about, FloatPrecision);
            return result;
        }

        /// <summary>
        /// 测试接近制定向量的值（取决于浮点精度）
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="about">靠近</param>
        /// <param name="range">误差范围</param>
        /// <returns>结果</returns>
        public static bool Approx(Vector3 value, Vector3 about, float range)
        {
            var result = (value - about).sqrMagnitude < range * range;
            return result;
        }

        #endregion

        #region Exp

        /// <summary>
        /// 2的N次方
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static double Exp2(double value)
        {
            var result = Math.Exp(value * 0.69314718055994530941723212145818);
            return result;
        }

        #endregion

        #region Power

        /// <summary>
        /// 是否是2的幂
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static bool IsPowerOfTwo(ulong value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }

        #endregion

        #region Round

        /// <summary>
        /// 四舍五入时保留指定的有效数字<para/>
        /// (double 有效精度 15-17)
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="digits">精度</param>
        /// <returns>结果</returns>
        public static double RoundToSignificantDigits(double value, int digits)
        {
            if (Math.Abs(value) < FloatPrecision) return 0.0;
            var intDigits = (int)Math.Floor(Math.Log10(Math.Abs(value))) + 1;
            if (intDigits <= digits) return Math.Round(value, digits - intDigits);
            var scale = Math.Pow(10, intDigits - digits);
            var result = Math.Round(value / scale) * scale;
            return result;
        }

        /// <summary>
        /// 四舍五入时保留指定的有效数字<para/>
        /// (float 有效精度 6-9)
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="digits">精度</param>
        /// <returns>结果</returns>
        public static float RoundToSignificantDigitsFloat(float value, int digits)
        {
            var result = (float)RoundToSignificantDigits(value, digits);
            return result;
        }

        /// <summary>
        /// 将指定值按范围线性拟合到[0，1]
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="min">输入最小值</param>
        /// <param name="max">输入最大值</param>
        /// <returns>结果</returns>
        public static float Linear01(float value, float min, float max)
        {
            var result = (value - min) / (max - min);
            return result;
        }

        /// <summary>
        /// 将指定值按范围线性拟合到[0，1] (限制边界)
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="min">输入最小值</param>
        /// <param name="max">输入最大值</param>
        /// <returns>结果</returns>
        public static float Linear01Clamped(float value, float min, float max)
        {
            var result = Mathf.Clamp01((value - min) / (max - min));
            return result;
        }

        /// <summary>
        /// 将指定值和范围，线性拟合到另一个范围区间
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="min">输入最小值</param>
        /// <param name="max">输入最大值</param>
        /// <param name="outputMin">输出最小值</param>
        /// <param name="outputMax">输出最大值</param>
        /// <returns>结果</returns>
        public static float Linear(float value, float min, float max, float outputMin, float outputMax)
        {
            var result = (value - min) / (max - min) * (outputMax - outputMin) + outputMin;
            return result;
        }

        /// <summary>
        /// 将指定值和范围，线性拟合到另一个范围区间 (限制边界)
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="min">输入最小值</param>
        /// <param name="max">输入最大值</param>
        /// <param name="outputMin">输出最小值</param>
        /// <param name="outputMax">输出最大值</param>
        /// <returns>结果</returns>
        public static float LinearClamped(float value, float min, float max, float outputMin, float outputMax)
        {
            var result = Mathf.Clamp01((value - min) / (max - min)) * (outputMax - outputMin) + outputMin;
            return result;
        }

        #endregion

        #region Clamp

        /// <summary>
        /// 限制范围
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(T value, T min, T max)
            where T : IComparable<T>
        {
            Assert.IsTrue(min.CompareTo(max) <= 0, "min <= max");
            if (value.CompareTo(min) < 0)
            {
                value = min;
            }
            else if (value.CompareTo(max) > 0)
            {
                value = max;
            }

            return value;
        }

        /// <summary>
        /// 限制范围，不强制要求 min < max
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">数值</param>
        /// <param name="argA">值1</param>
        /// <param name="argB">值2</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SafeClamp<T>(T value, T argA, T argB)
            where T : IComparable<T>
        {
            return argA.CompareTo(argB) < 0 ? Min(Max(value, argA), argB) : Min(Max(value, argB), argA);
        }

        /// <summary>
        /// 限制最小值为0
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ClampMin0(float value)
        {
            return ClampMin(value, 0);
        }

        /// <summary>
        /// 限制最小值为0
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampMin0(int value)
        {
            return ClampMin(value, 0);
        }

        /// <summary>
        /// 限制最小值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="min">最小值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ClampMin<T>(T value, T min)
            where T : IComparable<T>
        {
            return value.CompareTo(min) < 0 ? min : value;
        }

        /// <summary>
        /// 限制最大值为0
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ClampMax0(float value)
        {
            return ClampMax(value, 0);
        }

        /// <summary>
        /// 限制最大值为0
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampMax0(int value)
        {
            return ClampMax(value, 0);
        }

        /// <summary>
        /// 限制最大值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="max">最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ClampMax<T>(T value, T max)
            where T : IComparable<T>
        {
            return value.CompareTo(max) > 0 ? max : value;
        }

        #endregion

        #region Min / Max

        /// <summary>
        /// 最大值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="argA">数值1</param>
        /// <param name="argB">数值2</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Max<T>(T argA, T argB)
            where T : IComparable<T>
        {
            return argA.CompareTo(argB) < 0 ? argB : argA;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="argA">数值1</param>
        /// <param name="argB">数值2</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Min<T>(T argA, T argB)
            where T : IComparable<T>
        {
            return argA.CompareTo(argB) < 0 ? argA : argB;
        }

        #endregion

        #region Map

        /// <summary>
        /// 将数值映射到[0,1]
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Map01(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        /// <summary>
        /// 将数值映射到[0,1]（限制范围）
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Map01Clamped(float value, float min, float max)
        {
            return Mathf.Clamp01(Map01(value, min, max));
        }

        /// <summary>
        /// 将输入值从左范围映射到右范围
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="leftMin">左范围最小值</param>
        /// <param name="leftMax">左范围最大值</param>
        /// <param name="rightMin">右范围最小值</param>
        /// <param name="rightMax">右范围最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
        {
            return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
        }

        /// <summary>
        /// 将输入值从左范围映射到右范围（限制范围）
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="leftMin">左范围最小值</param>
        /// <param name="leftMax">左范围最大值</param>
        /// <param name="rightMin">右范围最小值</param>
        /// <param name="rightMax">右范围最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MapClamped(float value, float leftMin, float leftMax, float rightMin, float rightMax)
        {
            Assert.IsTrue(rightMin <= rightMax, $"{nameof(rightMin)} <= {nameof(rightMax)}");
            return Mathf.Clamp(Map(value, leftMin, leftMax, rightMin, rightMax), rightMin, rightMax);
        }

        /// <summary>
        /// 将输入值从左范围映射到右范围，并此范围内循环
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="leftMin">左范围最小值</param>
        /// <param name="leftMax">左范围最大值</param>
        /// <param name="rightMin">右范围最小值</param>
        /// <param name="rightMax">右范围最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MapRepeat(float value, float leftMin, float leftMax, float rightMin, float rightMax)
        {
            var rightLen = rightMax - rightMin;
            return rightMin + (value - leftMin) * rightLen / (leftMax - leftMin) % rightLen;
        }

        /// <summary>
        /// 将输入值从左范围映射到右范围
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="leftMin">左范围最小值</param>
        /// <param name="leftMax">左范围最大值</param>
        /// <param name="rightMin">右范围最小值</param>
        /// <param name="rightMax">右范围最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MapSafeClamped(float value, float leftMin, float leftMax, float rightMin, float rightMax)
        {
            return SafeClamp(Map(value, leftMin, leftMax, rightMin, rightMax), rightMin, rightMax);
        }

        /// <summary>
        /// 将输入值从左范围映射到右整数范围
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="leftMin">左范围最小值</param>
        /// <param name="leftMax">左范围最大值</param>
        /// <param name="rightMin">右范围最小值</param>
        /// <param name="rightMax">右范围最大值</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MapToInt(float value, float leftMin, float leftMax, int rightMin, int rightMax)
        {
            return Mathf.RoundToInt(Map(value, leftMin, leftMax, rightMin, rightMax));
        }

        #endregion

        #region Wrap

        /// <summary>
        /// 将值限制在主要范围内<para/>
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <returns>结果</returns>
        public static int Wrap(int angle, int min, int max)
        {
            max -= min;
            if (max == 0)
            {
                return min;
            }

            return angle - max * (int)Math.Floor((double)(angle - min) / max);
        }

        /// <summary>
        /// 将值限制在主要范围内<para/>
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <returns>结果</returns>
        public static long Wrap(long value, long min, long max)
        {
            max -= min;
            if (max == 0)
            {
                return min;
            }

            return value - max * (long)Math.Floor((double)(value - min) / max);
        }

        /// <summary>
        /// 将值限制在主要范围内<para/>
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <returns>结果</returns>
        public static float Wrap(float value, float min, float max)
        {
            max -= min;
            if (Math.Abs(max) < FloatPrecision)
            {
                return min;
            }

            return value - max * (float)Math.Floor((value - min) / max);
        }

        #endregion

        #region Log

        /// <summary>
        /// 计算某个常数（底数）必须达到的指数
        /// </summary>
        /// <param name="value">常数</param>
        /// <param name="baseValue">底数</param>
        /// <returns>结果</returns>
        public static float LogBaseOf(float value, float baseValue)
        {
            return (float)(Math.Log(value) / Math.Log(baseValue));
        } 

        #endregion

        #region Prime

        /// <summary>
        /// 是否是质数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static bool IsPrime(long value)
        {
            if (value < 2)
            {
                return false;
            }

            if ((value % 2 == 0) & (value != 2))
            {
                return false;
            }

            var sqrt = (long)Math.Floor(Math.Sqrt(value));
            for (long i = 3; i <= sqrt; i += 2)
            {
                if (value % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 是否互质
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值2</param>
        /// <returns>结果</returns>
        public static bool IsRelativelyPrime(short value1, short value2)
        {
            return Gcd(value1, value2) == 1;
        }

        /// <summary>
        /// 是否互质
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值2</param>
        /// <returns>结果</returns>
        public static bool IsRelativelyPrime(int value1, int value2)
        {
            return Gcd(value1, value2) == 1;
        }

        /// <summary>
        /// 是否互质
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值2</param>
        /// <returns>结果</returns>
        public static bool IsRelativelyPrime(long value1, long value2)
        {
            return Gcd((int)value1, (int)value2) == 1;
        }

        #endregion

        #region Divider / GCD / LCM

        /// <summary>
        /// 计算所有约数
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>结果</returns>
        public static int[] DividersOf(int value)
        {
            value = Math.Abs(value);
            var arr = new List<int>();
            var sqrt = (int)Math.Sqrt(value);
            var c = 0;

            for (var i = 1; i <= sqrt; i++)
            {
                if (value % i == 0)
                {
                    arr.Add(i);
                    c = value / i;
                    if (c != i)
                    {
                        arr.Add(c);
                    }
                }
            }

            arr.Sort();
            return arr.ToArray();
        }

        /// <summary>
        /// 计算所有公约数
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值1</param>
        /// <returns>结果</returns>
        public static int[] CommonDividersOf(int value1, int value2)
        {
            var i = 0;
            var j = 0;
            if (value1 < 0)
            {
                value1 = -value1;
            }

            if (value2 < 0)
            {
                value2 = -value2;
            }

            if (value1 > value2)
            {
                i = value1;
                value1 = value2;
                value2 = i;
            }

            var set = new HashSet<int>();
            var r = (int)Math.Sqrt(value1);
            for (i = 1; i <= r; i++)
            {
                if (value1 % i == 0 && value2 % i == 0)
                {
                    set.Add(i);
                    j = value1 / i;
                    if (value2 % j == 0)
                    {
                        set.Add(j);
                    }

                    j = value2 / i;
                    if (value1 % j == 0)
                    {
                        set.Add(j);
                    }
                }
            }

            var array = new int[set.Count];
            var index = 0;
            foreach (var item in set)
            {
                array[index] = item;
                index++;
            }

            Array.Sort((Array)array);
            return array;
        }

        /// <summary>
        /// 最大公约数
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值2</param>
        /// <returns>结果</returns>
        public static int Gcd(int value1, int value2)
        {
            var r = 0;
            if (value1 < 0)
            {
                value1 = -value1;
            }

            if (value2 < 0)
            {
                value2 = -value2;
            }

            if (value1 < value2)
            {
                r = value1;
                value1 = value2;
                value2 = r;
            }

            while (true)
            {
                r = value1 % value2;
                if (r == 0)
                {
                    return value2;
                }

                value1 = value2;
                value2 = r;
            }
        }

        /// <summary>
        /// 最大公约数
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值2</param>
        /// <returns>结果</returns>
        public static long Gcd(long value1, long value2)
        {
            long r = 0;
            if (value1 < 0)
            {
                value1 = -value1;
            }

            if (value2 < 0)
            {
                value2 = -value2;
            }

            if (value1 < value2)
            {
                r = value1;
                value1 = value2;
                value2 = r;
            }

            while (true)
            {
                r = value1 % value2;
                if (r == 0)
                {
                    return value2;
                }

                value1 = value2;
                value2 = r;
            }
        }

        /// <summary>
        /// 最小公倍数
        /// </summary>
        /// <param name="value1">数值1</param>
        /// <param name="value2">数值2</param>
        /// <returns>结果</returns>
        public static int Lcm(int value1, int value2)
        {
            return value1 * value2 / Gcd(value1, value2);
        }

        #endregion

        #region Factorial

        /// <summary>
        /// 阶乘
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>结果</returns>
        public static long Factorial(uint value)
        {
            if (value <= 0)
            {
                return 1;
            }

            long res = value;

            while (--value != 0)
            {
                res *= value;
            }

            return res;
        }

        /// <summary>
        /// 下降阶乘
        /// </summary>
        /// <param name="n">N</param>
        /// <param name="x">X</param>
        /// <returns>结果</returns>
        public static long FallingFactorial(uint n, uint x)
        {
            return Factorial(n) / Factorial(n - x);
        }

        /// <summary>
        /// 上升阶乘
        /// </summary>
        /// <param name="n">N</param>
        /// <param name="x">X</param>
        /// <returns>结果</returns>
        public static long RisingFactorial(uint n, uint x)
        {
            return Factorial(n + x - 1) / Factorial(n - 1);
        }

        #endregion

        #region Binomial Coefficient

        /// <summary>
        /// 二项式系数
        /// </summary>
        /// <param name="n">N</param>
        /// <param name="k">K</param>
        /// <returns>结果</returns>
        public static long BinomialCoefficient(uint n, uint k)
        {
            Assert.IsTrue(n >= k); // >=0
            return FallingFactorial(n, k) / Factorial(k);
        }

        /// <summary>
        /// 上升二项式系数
        /// </summary>
        /// <param name="n">N</param>
        /// <param name="k">K</param>
        /// <returns>结果</returns>
        public static long RisingBinomialCoefficient(uint n, uint k)
        {
            return RisingFactorial(n, k) / Factorial(k);
        }

        #endregion

        #region Shear
        
        /// <summary>
        /// 小数部分
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>结果</returns>
        public static float Shear(float value)
        {
            return value % 1.0f;
        }

        #endregion

        #region Range / Bounds

        /// <summary>
        /// 是否在范围内
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>结果</returns>
        public static bool InRange(float value, float min, float max)
        {
            if (max < min)
            {
                return value >= max && value <= min;
            }

            return value >= min && value <= max;
        }

        /// <summary>
        /// 是否在范围内
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>结果</returns>
        public static bool InRange(int value, int min, int max)
        {
            if (max < min)
            {
                return value >= max && value <= min;
            }

            return value >= min && value <= max;
        }

        /// <summary>
        /// 是否不在范围内
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>结果</returns>
        public static bool InRangeExclusive(float value, float min, float max)
        {
            if (max < min)
            {
                return value > max && value < min;
            }

            return value > min && value < max;
        }

        /// <summary>
        /// 索引是否在有效范围
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="bound">范围</param>
        /// <returns>结果</returns>
        public static bool InBounds(int index, int bound)
        {
            return index >= 0 && index < bound;
        }

        #endregion
    }
}