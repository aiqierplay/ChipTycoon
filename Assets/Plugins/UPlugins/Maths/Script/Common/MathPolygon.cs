/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathPolygon.cs
//  Info     : 数学辅助计算类 —— 多边形
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
        /// 判断点是否在多边形区域内（射线法/支持凹多边形）
        /// </summary>
        /// <param name="point">待判断的点</param>
        /// <param name="polygonPoints">多边形顶点数组</param>
        /// <param name="containEdge">包含边</param>
        /// <returns>true:在多边形内，凹点   false：在多边形外，凸点</returns>
        public static bool IsPointInPolygon(Vector2 point, Vector2[] polygonPoints, bool containEdge = true)
        {
            var result = false;
            var j = polygonPoints.Length - 1;
            for (var i = 0; i < polygonPoints.Length; j = i++)
            {
                var p1 = polygonPoints[i];
                var p2 = polygonPoints[j];
                // 这里判断是否刚好被测点在多边形的边上
                if (IsPointOnLineSegment(point, p1, p2)) return containEdge;
                if ((p1.y > point.y != p2.y > point.y) && (point.x < (point.y - p1.y) * (p1.x - p2.x) / (p1.y - p2.y) + p1.x))
                {
                    result = !result;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断点是否在多边形区域内 实现方式2
        /// </summary>
        /// <param name="point">待判断的点</param>
        /// <param name="points">多边形顶点数组</param>
        /// <returns>结果</returns>
        public static bool IsPointInPolygon2(Vector2 point, Vector2[] points)
        {
            int i, j, c = 0;
            for (i = 0, j = points.Length - 1; i < points.Length; j = i++)
            {
                if (((points[i].y > point.y) != (points[j].y > point.y)) && (point.x < (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
                {
                    c = 1 + c;
                }
            }

            var result = c % 2 != 0;
            return result;
        }

        /// <summary>
        /// 计算多边形面积(忽略y轴)
        /// </summary>
        /// <param name="polygonPoints">多边形顶点数组</param>
        /// <returns>结果</returns>
        public static float PolygonArea(Vector3[] polygonPoints)
        {
            var iArea = 0f;
            for (int iCycle = 0, iCount = polygonPoints.Length; iCycle < iCount; iCycle++)
            {
                iArea += (polygonPoints[iCycle].x * polygonPoints[(iCycle + 1) % iCount].z - polygonPoints[(iCycle + 1) % iCount].x * polygonPoints[iCycle].z);
            }

            var result = (float)Math.Abs(0.5 * iArea);
            return result;
        }

    }
}