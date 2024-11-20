/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathLine.cs
//  Info     : 数学辅助计算类 —— 线段
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
        /// 计算线段上离指定点最近的点
        /// </summary>
        /// <param name="lineStart">线段起点</param>
        /// <param name="lineEnd">线段终点</param>
        /// <param name="point">点</param>
        /// <returns>结果</returns>
        public static Vector3 ClosestPointOnSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            var dir = lineEnd - lineStart;
            var sqrMgn = dir.sqrMagnitude;
            if (sqrMgn <= EpsilonSingle)
            {
                return lineStart;
            }

            var factor = Vector3.Dot(point - lineStart, dir) / sqrMgn;
            return lineStart + Mathf.Clamp01(factor) * dir;
        }

        /// <summary>
        /// 计算线段在XZ平面上离指定点最近的点
        /// </summary>
        /// <param name="lineStart">线段起点</param>
        /// <param name="lineEnd">线段终点</param>
        /// <param name="point">点</param>
        /// <returns>结果</returns>
        public static Vector3 ClosestPointOnSegmentXZ(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            lineStart.y = point.y;
            lineEnd.y = point.y;
            var fullDirection = lineEnd - lineStart;
            var fullDirection2 = fullDirection;
            fullDirection2.y = 0;
            var mgn = fullDirection2.magnitude;
            var lineDirection = mgn > float.Epsilon ? fullDirection2 / mgn : Vector3.zero;
            var closestPoint = Vector3.Dot(point - lineStart, lineDirection);
            return lineStart + Mathf.Clamp(closestPoint, 0.0f, fullDirection2.magnitude) * lineDirection;
        }

        #endregion

        #region Check Side

        /// <summary>
        /// 判断一个点在一个线段的哪一边
        /// </summary>
        /// <param name="p1">线段起点</param>
        /// <param name="p2">线段终点</param>
        /// <param name="point">点</param>
        /// <returns>1 0 2</returns>
        public static int PointOnWhichSideOfLineSegment(Vector3 p1, Vector3 p2, Vector3 point)
        {
            var rhs = p2 - p1;
            var lhs = point - p1;
            var num = Vector3.Dot(lhs, rhs);
            if (num <= 0f)
            {
                return 1;
            }
            if (lhs.magnitude <= rhs.magnitude)
            {
                return 0;
            }
            return 2;
        }

        #endregion

        #region Project

        /// <summary>
        ///  一个点在一条线段上的投影点
        /// </summary>
        /// <param name="p1">线段起点</param>
        /// <param name="p2">线段终点</param>
        /// <param name="point"></param>
        /// <returns>结果</returns>
        public static Vector3 ProjectPointOnLineSegment(Vector3 p1, Vector3 p2, Vector3 point)
        {
            var vector = ProjectPointOnLine(p1, (p2 - p1).normalized, point);
            var num = PointOnWhichSideOfLineSegment(p1, p2, vector);
            if (num == 0)
            {
                return vector;
            }
            if (num == 1)
            {
                return p1;
            }
            if (num == 2)
            {
                return p2;
            }
            return Vector3.zero;
        }

        #endregion

        #region Check On

        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="p1">线段端点1</param>
        /// <param name="p2">线段端点2</param>
        /// <returns>结果</returns>
        public static bool IsPointOnLineSegment(Vector2 point, Vector2 p1, Vector2 p2)
        {
            var disLine = Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2);
            var dis1 = Math.Pow(point.x - p1.x, 2) + Math.Pow(point.y - p1.y, 2);
            var dis2 = Math.Pow(point.x - p2.x, 2) + Math.Pow(point.y - p2.y, 2);
            var result = Math.Abs(dis1 + dis2 - disLine) < FloatPrecision;
            return result;
        }

        #endregion

        #region Intersection

        /// <summary>
        /// 判断两条线段是否相交，并求交点
        /// </summary>
        /// <param name="p1">线段1起点坐标</param>
        /// <param name="p2">线段1终点坐标</param>
        /// <param name="p3">线段2起点坐标</param>
        /// <param name="p4">线段2终点坐标</param>
        /// <param name="intersection">相交点坐标</param>
        public static bool LineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
        {
            var vecA = p2 - p1;
            var vecB = p4 - p3;
            var t = ((p3.x - p1.x) * vecB.y - (p3.y - p1.y) * vecB.x) / (vecA.x * vecB.y - vecA.y * vecB.x);
            var s = ((p1.x - p3.x) * vecA.y - (p1.y - p3.y) * vecA.x) / (vecB.x * vecA.y - vecB.y * vecA.x);
            if (t >= 0 && t <= 1 && s >= 0 && s <= 1)
            {
                intersection = p1 + t * vecA;
                return true;
            }
            else
            {
                intersection = Vector2.zero;
                return false;
            }
        }

        #endregion
    }
}