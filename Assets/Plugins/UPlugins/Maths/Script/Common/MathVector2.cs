/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathVector2.cs
//  Info     : 数学辅助计算类 —— Vector2
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2022
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Aya.Maths
{
    public static partial class MathUtil
    {
        #region NaN
        
        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="vector">向量</param>
        /// <returns>结果</returns>
        public static bool IsNaN(Vector2 vector)
        {
            return float.IsNaN(vector.sqrMagnitude);
        }

        #endregion

        #region Length
        
        /// <summary>
        /// 增加向量的长度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="length">长度</param>
        /// <returns>结果</returns>
        public static Vector2 AddLength(Vector2 vector, float length)
        {
            var num = (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y);
            num += length;
            vector.Normalize();
            var result = Vector2.Scale(vector, new Vector2(num, num));
            return result;
        }

        /// <summary>
        /// 设置向量的长度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="length">长度</param>
        /// <returns>结果</returns>
        public static Vector2 SetLength(Vector2 vector, float length)
        {
            var normalize = vector.normalized;
            var result = normalize * length;
            return result;
        }

        #endregion

        #region Abs
       
        /// <summary>
        /// 对向量的所有值取绝对值
        /// </summary>
        /// <param name="vector">向量</param>
        /// <returns>结果</returns>
        public static Vector2 Abs(Vector2 vector)
        {
            var result = new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
            return result;
        }

        #endregion

        #region Distance
       
        /// <summary>
        /// 距离的平方
        /// 计算距离的开根计算有一定开销，可使用距离的平方比较大小
        /// </summary>
        /// <param name="from">开始</param>
        /// <param name="to">结束</param>
        /// <returns>结果</returns>
        public static float SqrDistance(Vector2 from, Vector2 to)
        {
            var result = Mathf.Pow(from.x - to.x, 2) + Mathf.Pow(from.y - to.y, 2);
            return result;
        }

        /// <summary>
        /// 点到直线距离
        /// </summary>
        /// <param name="vector">点坐标</param>
        /// <param name="linePoint1">直线上一个点的坐标</param>
        /// <param name="linePoint2">直线上另一个点的坐标</param>
        /// <returns>距离</returns>
        public static float DistancePointToLine(Vector2 vector, Vector2 linePoint1, Vector2 linePoint2)
        {
            float space;
            // 线段的长度  
            var a = Vector2.Distance(linePoint1, linePoint2);
            // lineA到点的距离
            var b = Vector2.Distance(linePoint1, vector);
            // lineB到点的距离
            var c = Vector2.Distance(linePoint2, vector);
            if (c <= FloatPrecision || b <= FloatPrecision)
            {
                space = 0;
                return space;
            }
            if (a <= FloatPrecision)
            {
                space = b;
                return space;
            }
            if (c * c >= a * a + b * b)
            {
                space = b;
                return space;
            }
            if (b * b >= a * a + c * c)
            {
                space = c;
                return space;
            }
            // 半周长 
            var p = (a + b + c) / 2;
            // 海伦公式求面积  
            var s = Mathf.Sqrt(p * (p - a) * (p - b) * (p - c));
            // 返回点到线的距离（利用三角形面积公式求高）
            space = 2 * s / a;
            return space;
        }

        #endregion

        #region Clamp

        /// <summary>
        /// 限制范围
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="min">最小</param>
        /// <param name="max">最大</param>
        /// <returns>结果</returns>
        public static Vector3 Clamp(Vector2 input, Vector2 min, Vector2 max)
        {
            input.x = Mathf.Clamp(input.x, min.x, max.x);
            input.y = Mathf.Clamp(input.y, min.y, max.y);
            return input;
        }

        #endregion

        #region Reflect
        
        /// <summary>
        /// 基于指定法线反射
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="normal">法线</param>
        /// <returns>结果</returns>
        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            var dp = 2 * Vector2.Dot(vector, normal);
            return new Vector2(vector.x - normal.x * dp, vector.y - normal.y * dp);
        }

        #endregion

        #region Mirror
       
        /// <summary>
        /// 基于指定轴镜像
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="axis">轴</param>
        /// <returns>结果</returns>
        public static Vector2 Mirror(Vector2 vector, Vector2 axis)
        {
            return 2 * (Vector2.Dot(vector, axis) / Vector2.Dot(axis, axis)) * axis - vector;
        }

        #endregion

        #region Cross Product

        /// <summary>
        /// 向量的叉积
        /// </summary>
        /// <param name="vector1">向量1</param>
        /// <param name="vector2">向量2</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CrossProduct2D(Vector2 vector1, Vector2 vector2)
        {
            return vector1.x * vector2.y - vector1.y * vector2.x;
        }

        /// <summary>
        /// 向量的叉积
        /// </summary>
        /// <param name="vector">向量</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 CrossProduct2D(Vector2 vector)
        {
            return new Vector2(vector.y, -vector.x);
        }

        #endregion

        #region Orthogonal
        
        /// <summary>
        /// 正交
        /// </summary>
        /// <param name="vector">向量</param>
        /// <returns>结果</returns>
        public static Vector2 Orthogonal(Vector2 vector)
        {
            return new Vector2(-vector.y, vector.x);
        } 

        #endregion
    }
}