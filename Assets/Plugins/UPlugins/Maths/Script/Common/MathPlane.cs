/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathPlane.cs
//  Info     : 数学辅助计算类 —— 平面
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
        #region Plane

        /// <summary>
        /// 三个点确定一个平面，返回平面的法线
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="p3">点3</param>
        /// <returns>平面的法线</returns>
        public static Vector3 PlaneFrom3Points(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var vector = p2 - p1;
            var vector2 = p3 - p1;
            var planeNormal = Vector3.Normalize(Vector3.Cross(vector, vector2));
            var vector3 = p1 + vector / 2f;
            var vector4 = p1 + vector2 / 2f;
            var lineVec = p3 - vector3;
            var lineVec2 = p2 - vector4;
            ClosestPointsOnLines(vector3, lineVec, vector4, lineVec2, out _, out _);
            return planeNormal;
        }

        #endregion

        #region Distance

        /// <summary>
        /// 点到平面的距离
        /// </summary>
        /// <param name="point"></param>
        /// <param name="planePoint"></param>
        /// <param name="planeNormal"></param>
        /// <returns></returns>
        public static float DistancePointToPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
        {
            var result = Vector3.Dot(planeNormal, point - planePoint);
            return result;
        } 

        #endregion

        #region Project

        /// <summary>
        /// 空间中一个点在一个面上的投影点
        /// </summary>
        /// <param name="planePoint">平面上一个点</param>
        /// <param name="planeNormal">平面的法线</param>
        /// <param name="point">点</param>
        /// <returns>结果</returns>
        public static Vector3 ProjectPointOnPlane(Vector3 planePoint, Vector3 planeNormal, Vector3 point)
        {
            var num = DistancePointToPlane(point, planePoint, planeNormal);
            num *= -1f;
            var b = SetLength(planeNormal, num);
            var result = point + b;
            return result;
        }

        /// <summary>
        /// 一个向量在一个面上的投影向量
        /// </summary>
        /// <param name="planeNormal"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
        {
            var result = vector - Vector3.Dot(vector, planeNormal) * planeNormal;
            return result;
        }

        #endregion

        #region Intersection

        /// <summary>
        /// 两个面是否相交
        /// </summary>
        /// <param name="planePoint1">平面1上的点</param>
        /// <param name="planeNormal1">平面1的法线</param>
        /// <param name="planePoint2">平面2上的点</param>
        /// <param name="planeNormal2">平面2的法线</param>
        /// <param name="linePoint">相交点</param>
        /// <param name="lineVector">相交线的向量</param>
        /// <returns>结果</returns>
        public static bool PlaneIntersection(Vector3 planePoint1, Vector3 planeNormal1, Vector3 planePoint2, Vector3 planeNormal2, out Vector3 linePoint, out Vector3 lineVector)
        {
            linePoint = Vector3.zero;
            lineVector = Vector3.zero;
            lineVector = Vector3.Cross(planeNormal1, planeNormal2);
            var vector = Vector3.Cross(planeNormal2, lineVector);
            var num = Vector3.Dot(planeNormal1, vector);
            if (Mathf.Abs(num) > 0.006f)
            {
                var rhs = planePoint1 - planePoint2;
                var d = Vector3.Dot(planeNormal1, rhs) / num;
                linePoint = planePoint2 + d * vector;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 线面是否相交
        /// </summary>
        /// <param name="linePoint">线上一个点</param>
        /// <param name="lineVector">线的向量</param>
        /// <param name="planePoint">平面上一个点</param>
        /// <param name="planeNormal">平面的法线</param>
        /// <param name="intersection">交点</param>
        /// <returns>结果</returns>
        public static bool LinePlaneIntersection(Vector3 linePoint, Vector3 lineVector, Vector3 planePoint, Vector3 planeNormal, out Vector3 intersection)
        {
            intersection = Vector3.zero;
            var num = Vector3.Dot(planePoint - linePoint, planeNormal);
            var num2 = Vector3.Dot(lineVector, planeNormal);
            if (Math.Abs(num2) > FloatPrecision)
            {
                var size = num / num2;
                var vector = SetLength(lineVector, size);
                intersection = linePoint + vector;
                return true;
            }
            return false;
        }

        #endregion
    }
}