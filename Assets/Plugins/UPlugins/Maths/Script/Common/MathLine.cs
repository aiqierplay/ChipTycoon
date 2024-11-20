/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathLine.cs
//  Info     : 数学辅助计算类 —— 线
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
        #region Closest Points

        /// <summary>
        /// 计算离直线上离指定点最近的点，直线无限长
        /// </summary>
        /// <param name="lineStart">起点</param>
        /// <param name="lineEnd">终点</param>
        /// <param name="point">点</param>
        /// <returns>结果</returns>
        public static Vector3 ClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            var lineDirection = Vector3.Normalize(lineEnd - lineStart);
            var dot = Vector3.Dot(point - lineStart, lineDirection);
            return lineStart + dot * lineDirection;
        }

        /// <summary>
        /// 计算离直线上离指定点最近的点在线段上的因子，超出(0,1)则不在线段上
        /// </summary>
        /// <param name="lineStart">起点</param>
        /// <param name="lineEnd">终点</param>
        /// <param name="point">点</param>
        /// <returns>结果</returns>
        public static float ClosestPointOnLineFactor(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            var dir = lineEnd - lineStart;
            var sqrMgn = dir.sqrMagnitude;
            if (sqrMgn <= EpsilonSingle)
            {
                return 0;
            }

            return Vector3.Dot(point - lineStart, dir) / sqrMgn;
        }

        /// <summary>
        /// 计算离直线上离指定点最近的点在线段上的因子，超出(0,1)则不在线段上
        /// </summary>
        /// <param name="lineStart">起点</param>
        /// <param name="lineEnd">终点</param>
        /// <param name="point">点</param>
        /// <returns>结果</returns>
        public static float ClosestPointOnLineFactor(Vector3Int lineStart, Vector3Int lineEnd, Vector3Int point)
        {
            var lineDirection = lineEnd - lineStart;
            float mgn = lineDirection.sqrMagnitude;
            var closestPoint = Vector3.Dot(point - lineStart, lineDirection);
            if (mgn > EpsilonSingle)
            {
                closestPoint /= mgn;
            }

            return closestPoint;
        }

        /// <summary>
        /// 计算离直线上离指定点最近的点在线段上的因子，超出(0,1)则不在线段上
        /// </summary>
        /// <param name="lineStart">起点</param>
        /// <param name="lineEnd">终点</param>
        /// <param name="point">点</param>
        /// <returns>结果</returns>
        public static float ClosestPointOnLineFactor(Vector2Int lineStart, Vector2Int lineEnd, Vector2Int point)
        {
            var lineDirection = lineEnd - lineStart;
            var mgn = lineDirection.sqrMagnitude;
            var closestPoint = Vector2.Dot(point - lineStart, lineDirection);
            if (mgn > EpsilonSingle)
            {
                closestPoint /= mgn;
            }

            return closestPoint;
        }

        /// <summary>
        /// 三维空间中两条直线上彼此最近的两个点
        /// </summary>
        /// <param name="p1">直线1起点</param>
        /// <param name="v1">直线1向量</param>
        /// <param name="p2">直线2起点</param>
        /// <param name="v2">直线2向量</param>
        /// <param name="closestPoint1">直线1上的最近点</param>
        /// <param name="closestPoint2">直线2上的最近点</param>
        /// <returns>是否有结果</returns>
        public static bool ClosestPointsOnLines(Vector3 p1, Vector3 v1, Vector3 p2, Vector3 v2, out Vector3 closestPoint1, out Vector3 closestPoint2)
        {
            closestPoint1 = Vector3.zero;
            closestPoint2 = Vector3.zero;
            var num = Vector3.Dot(v1, v1);
            var num2 = Vector3.Dot(v1, v2);
            var num3 = Vector3.Dot(v2, v2);
            var num4 = num * num3 - num2 * num2;
            if (Math.Abs(num4) > FloatPrecision)
            {
                var rhs = p1 - p2;
                var num5 = Vector3.Dot(v1, rhs);
                var num6 = Vector3.Dot(v2, rhs);
                var d = (num2 * num6 - num5 * num3) / num4;
                var d2 = (num * num6 - num5 * num2) / num4;
                closestPoint1 = p1 + v1 * d;
                closestPoint2 = p2 + v2 * d2;
                return true;
            }
            return false;
        }

        #endregion

        #region Project

        /// <summary>
        /// 一个点在一条直线上的投影点
        /// </summary>
        /// <param name="linePoint">直线上的点</param>
        /// <param name="lineVector">直线的向量</param>
        /// <param name="point">点</param>
        /// <returns>结果</returns>
        public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVector, Vector3 point)
        {
            var lhs = point - linePoint;
            var d = Vector3.Dot(lhs, lineVector);
            var result = linePoint + lineVector * d;
            return result;
        }

        #endregion

        #region Intersection

        /// <summary>
        /// 判断线与线之间的相交
        /// </summary>
        /// <param name="intersection">交点</param>
        /// <param name="p1">直线1上一点</param>
        /// <param name="v1">直线1方向</param>
        /// <param name="p2">直线2上一点</param>
        /// <param name="v2">直线2方向</param>
        /// <returns>是否相交</returns>
        public static bool LineIntersection(Vector3 p1, Vector3 v1, Vector3 p2, Vector3 v2, out Vector3 intersection)
        {
            intersection = Vector3.zero;
            if (Math.Abs(Vector3.Dot(v1, v2) - 1) <= FloatPrecision)
            {
                // 两线平行
                return false;
            }

            var startPointSeg = p2 - p1;
            var vecS1 = Vector3.Cross(v1, v2);            // 有向面积1
            var vecS2 = Vector3.Cross(startPointSeg, v2); // 有向面积2
            var num = Vector3.Dot(startPointSeg, vecS1);

            // 判断两这直线是否共面
            if (num >= FloatPrecision || num <= -FloatPrecision)
            {
                return false;
            }

            // 有向面积比值，利用点乘是因为结果可能是正数或者负数
            var num2 = Vector3.Dot(vecS2, vecS1) / vecS1.sqrMagnitude;

            intersection = p1 + v1 * num2;
            return true;
        }

        /// <summary>
        /// 计算两直线的起点分别到交点的有向距离
        /// </summary>
        /// <param name="p1">直线1起点</param>
        /// <param name="v1">直线1向量</param>
        /// <param name="p2">直线2起点</param>
        /// <param name="v2">直线2向量</param>
        /// <param name="len1">直线1起点到交点的距离</param>
        /// <param name="len2">直线2起点到交点的距离</param>
        /// <returns>是否相交</returns>
        public static bool LineIntersection(Vector3 p1, Vector3 v1, Vector3 p2, Vector3 v2, out float len1, out float len2)
        {
            len1 = float.PositiveInfinity;
            len2 = float.PositiveInfinity;
            var lhs = p2 - p1;
            var rhs = Vector3.Cross(v1, v2);
            var lhs2 = Vector3.Cross(lhs, v2);
            var lhs3 = Vector3.Cross(lhs, v1);
            var num = Vector3.Dot(lhs, rhs);
            if (num >= FloatPrecision || num <= -FloatPrecision)
            {
                return false;
            }
            len1 = Vector3.Dot(lhs2, rhs) / rhs.sqrMagnitude;
            len2 = Vector3.Dot(lhs3, rhs) / rhs.sqrMagnitude;
            return true;
        }

        #endregion

        #region Distance

        /// <summary>
        /// 点到直线距离
        /// </summary>
        /// <param name="point">点坐标</param>
        /// <param name="p1">直线上一个点的坐标</param>
        /// <param name="p2">直线上另一个点的坐标</param>
        /// <returns>距离</returns>
        public static float DistancePointToLine(Vector3 point, Vector3 p1, Vector3 p2)
        {
            var vec1 = point - p1;
            var vec2 = p2 - p1;
            var project = Vector3.Project(vec1, vec2);
            var dis = Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(vec1), 2) - Mathf.Pow(Vector3.Magnitude(project), 2));
            return dis;
        } 

        #endregion
    }
}