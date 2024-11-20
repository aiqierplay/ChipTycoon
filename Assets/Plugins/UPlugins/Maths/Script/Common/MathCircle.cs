/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathCircle.cs
//  Info     : 数学辅助计算类 —— 圆
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2022
//
/////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Maths
{
    public static partial class MathUtil
    {
        /// <summary>
        /// 获取半径为r的圆周上的点坐标集合
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="radius">半径</param>
        /// <param name="deltaAngle">间隔角度</param>
        /// <returns>结果</returns>
        public static Vector3[] GetCirclePoints(Vector3 center, float radius, int deltaAngle)
        {
            var points = new Vector3[deltaAngle];
            for (int i = 0, j = 0; i < 360; j++, i += 360 / deltaAngle)
            {
                var angle = Mathf.Deg2Rad * i;
                points[j] = center + new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
            }

            return points;
        }

        #region Circle Point

        /// <summary>
        /// 获取在圆形内均匀分布的点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<Vector3> GetPointsInCircle(Vector3 center, float radius, int count)
        {
            var result = new List<Vector3>();
            var alpha = 2;
            var phi = (Mathf.Sqrt(5f) + 1f) / 2f;
            var angle = 2f * Mathf.PI / Mathf.Pow(phi, 2);
            var b = Mathf.RoundToInt(alpha * Mathf.Sqrt(count));
            for (var i = 1; i <= count; i++)
            {
                var r = radius;
                if (i <= count - b)
                {
                    r = Mathf.Sqrt(i - 0.5f) / Mathf.Sqrt(count - (b + 1f) / 2f) * radius;
                }

                var theta = i * angle;
                var x = r * Mathf.Cos(theta);
                var y = r * Mathf.Sin(theta);
                var point = new Vector3(x, 0f, y) + center;
                result.Add(point);
            }

            return result;
        }

        /// <summary>
        /// 获取在圆形内的随机点
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="count">数量</param>
        /// <returns>结果</returns>
        public static List<Vector3> GetRandPointsInCircle(Vector3 center, float radius, int count)
        {
            var result = new List<Vector3>();
            for (var i = 0; i < count; i++)
            {
                var randomValue = Random.value;
                var r = Mathf.Sqrt(randomValue) * radius;
                randomValue = Random.value;
                var delta = 2f * Mathf.PI * randomValue;
                var x = r * Mathf.Cos(delta);
                var z = r * Mathf.Sin(delta);
                var point = new Vector3(x, 0f, z) + center;
                result.Add(point);
            }

            return result;
        }

        #endregion
    }
}