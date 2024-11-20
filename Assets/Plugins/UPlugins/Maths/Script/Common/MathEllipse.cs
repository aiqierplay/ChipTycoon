/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathEllipse.cs
//  Info     : 数学辅助计算类 —— 椭圆
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
        /// 获取椭圆上的点
        /// </summary>
        /// <param name="center">中点</param>
        /// <param name="radiusShort">短轴</param>
        /// <param name="radiusLong">长轴</param>
        /// <param name="deltaAngle">间隔角度</param>
        /// <returns>结果</returns>
        public static Vector3[] GetEllipsePoints(Vector3 center, float radiusShort, float radiusLong, int deltaAngle)
        {
            var points = new Vector3[deltaAngle];
            var j = 0;
            for (float i = 0; i < 360; j++, i += 360f / deltaAngle)
            {
                var angle = (i / 180) * Mathf.PI;
                points[j] = center + new Vector3(radiusShort * Mathf.Cos(angle), 0, radiusLong * Mathf.Sin(angle));
            }

            return points;
        }

        /// <summary>
        /// 椭圆的近似圆
        /// </summary>
        /// <param name="a">A</param>
        /// <param name="b">B</param>
        /// <returns>结果</returns>
        public static float ApproxCircumOfEllipse(float a, float b)
        {
            return (float)(Pi * Math.Sqrt((a * a + b * b) / 2));
        }
    }
}