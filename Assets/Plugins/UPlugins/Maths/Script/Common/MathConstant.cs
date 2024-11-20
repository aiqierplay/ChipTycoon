/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathConstant.cs
//  Info     : 数学辅助计算类 —— 常数
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2022
//
/////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;

namespace Aya.Maths
{
    public static partial class MathUtil
    {
        /// <summary>
        /// 浮点精度
        /// </summary>
        public static float FloatPrecision = 1e-6f;

        /// <summary>
        /// Pi or 180 deg
        /// </summary>
        public const float Pi = 3.14159265358979f;

        /// <summary>
        /// Pi (Double) or 180 deg
        /// </summary>
        public const double PiD = Math.PI;

        /// <summary>
        /// Pi/2 or 90 deg
        /// </summary>
        public const float Pi2 = 1.57079633f;

        /// <summary>
        /// PiD/2 or 90 deg
        /// </summary>
        public const double Pi2D = PiD / 2.0;

        /// <summary>
        /// Pi/3 or 60 deg
        /// </summary>
        public const float Pi3 = 1.04719755119659666667f;

        /// <summary>
        /// Pi/4 or 45 deg
        /// </summary>
        public const float Pi4 = 0.785398163397448f;

        /// <summary>
        /// Pi/8 or 22.5 deg
        /// </summary>
        public const float Pi8 = 0.392699081698724f;

        /// <summary>
        /// Pi/16 or 11.25 deg
        /// </summary>
        public const float Pi16 = 0.196349540849362f;

        /// <summary>
        /// 2Pi or 360 deg
        /// </summary>
        public const float TwoPi = 6.28318531f;

        /// <summary>
        /// 3Pi/2 or 270 deg
        /// </summary>
        public const float ThreePi2 = 4.71238898038469f;

        /// <summary>
        /// E 自然常数
        /// </summary>
        public const float E = 2.71828182845905f;

        /// <summary>
        /// ln(10)
        /// </summary>
        public const float Ln10 = 2.30258509299405f;

        /// <summary>
        /// ln(2)
        /// </summary>
        public const float Ln2 = 0.693147180559945f;

        /// <summary>
        /// logB10(e)
        /// </summary>
        public const float Log10E = 0.434294481903252f;

        /// <summary>
        /// logB2(e)
        /// </summary>
        public const float Log2E = 1.44269504088896f;

        /// <summary>
        /// sqrt(1/2)
        /// </summary>
        public const float Sqrt12 = 0.707106781186548f;

        /// <summary>
        /// sqrt(2)
        /// </summary>
        public const float Sqrt2 = 1.41421356f;

        /// <summary>
        /// sqrt(3)
        /// </summary>
        public const float Sqrt3 = 1.73205081f;

        /// <summary>
        /// Pi/180
        /// </summary>
        public const float DegToRad = Pi / 180f;

        /// <summary>
        /// Pi/180 (double)
        /// </summary>
        public const double DegToRadD = PiD / 180.0;

        /// <summary>
        /// 180.0/Pi
        /// </summary>
        public const float RadToDeg = 180f / Pi;

        /// <summary>
        /// 180.0/Pi (double)
        /// </summary>
        public const double RadToDegD = 180.0 / PiD;

        /// <summary>
        ///  2^16
        /// </summary>
        public const int B16 = 65536;

        /// <summary>
        /// 2^31
        /// </summary>
        public const long B31 = 2147483648L;

        /// <summary>
        /// 2^32
        /// </summary>
        public const long B32 = 4294967296L;

        /// <summary>
        /// 2^48
        /// </summary>
        public const long B48 = 281474976710656L;

        /// <summary>
        /// 2^53 !!NOTE!! largest accurate double floating point whole value
        /// </summary>
        public const long B53 = 9007199254740992L;

        /// <summary>
        /// 2^63
        /// </summary>
        public const ulong B63 = 9223372036854775808;

        /// <summary>
        /// 18446744073709551615 or 2^64 - 1 or ULong.MaxValue...
        /// </summary>
        public const ulong B64M1 = ulong.MaxValue;

        /// <summary>
        /// 1.0/3.0
        /// </summary>
        public const float OneThird = 0.333333333333333f;

        /// <summary>
        /// 2.0/3.0
        /// </summary>
        public const float TwoThirds = 0.666666666666667f;

        /// <summary>
        /// 1.0/6.0
        /// </summary>
        public const float OneSixth = 0.166666666666667f;

        /// <summary>
        /// Cos(Pi/3)
        /// </summary>
        public const float CosPi3 = 0.866025403784439f;

        /// <summary>
        /// Sin(2Pi/3)
        /// </summary>
        public const float Sin2Pi3 = 0.03654595f;

        /// <summary>
        /// 4*(Math.Sqrt(2)-1)/3.0
        /// </summary>
        public const float CircleAlpha = 0.552284749830793f;

        /// <summary>
        /// 取整精度
        /// </summary>
        public const float ShortEpsilon = 0.1f;

        /// <summary>
        /// 百分比精度
        /// </summary>
        public const float PctEpsilon = 0.001f;

        /// <summary>
        /// 单精度浮点的机器精度
        /// </summary>
        public const float EpsilonSingle = 1.192093E-07f;

        /// <summary>
        /// 双精度浮点的机器精度
        /// </summary>
        public const double EpsilonDouble = 2.22044604925031E-16;

        /// <summary>
        /// 大于0的最小单精度浮点数
        /// </summary>
        public const float MinSingle = 1.175494E-38f;

        /// <summary>
        /// 最大的负单精度浮点数
        /// </summary>
        public const float MaxNegativeSingle = -EpsilonSingle;

        /// <summary>
        /// 大于0的最小双精度浮点数
        /// </summary>
        public const double MinDouble = 4.94065645841247E-324;

        /// <summary>
        /// 最大的负双精度浮点数
        /// </summary>
        public const double MaxNegativeDouble = -MinDouble;

        /// <summary>
        /// 开
        /// </summary>
        public const bool On = true;

        /// <summary>
        /// 关
        /// </summary>
        public const bool Off = false;

        /// <summary>
        /// 一百万分之一
        /// </summary>
        public const float OneMillionth = 1e-6f;

        /// <summary>
        /// 一百万
        /// </summary>
        public const float Million = 1e6f;
    }
}