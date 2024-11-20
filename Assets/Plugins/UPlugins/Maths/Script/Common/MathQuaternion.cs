/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathQuaternion.cs
//  Info     : 数学辅助计算类 —— 四元数
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
        /// 是否无效
        /// </summary>
        /// <param name="quaternion">四元数</param>
        /// <returns>结果</returns>
        public static bool IsNaN(Quaternion quaternion)
        {
            return float.IsNaN(quaternion.x * quaternion.y * quaternion.z * quaternion.w);
        }

        /// <summary>
        /// 归一化
        /// </summary>
        /// <param name="quaternion">四元数</param>
        /// <returns>结果</returns>
        public static Quaternion Normalize(Quaternion quaternion)
        {
            var mag = Math.Sqrt(quaternion.w * quaternion.w + quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z);
            quaternion.w = (float)(quaternion.w / mag);
            quaternion.x = (float)(quaternion.x / mag);
            quaternion.y = (float)(quaternion.y / mag);
            quaternion.z = (float)(quaternion.z / mag);
            return quaternion;
        }

        /// <summary>
        /// 计算两个向量之间的旋转
        /// </summary>
        /// <param name="v1">向量1</param>
        /// <param name="v2">向量2</param>
        /// <returns>结果</returns>
        public static Quaternion FromToRotation(Vector3 v1, Vector3 v2)
        {
            var a = Vector3.Cross(v1, v2);
            var w = Math.Sqrt(v1.sqrMagnitude * v2.sqrMagnitude) + Vector3.Dot(v1, v2);
            if (a.sqrMagnitude < Mathf.Epsilon)
            {
                // the vectors are parallel, check w to find direction
                // if w is 0 then values are opposite, and we should rotate 180 degrees around some axis
                // otherwise the vectors in the same direction and no rotation should occur
                return Math.Abs(w) < Mathf.Epsilon ? new Quaternion(0f, 1f, 0f, 0f) : Quaternion.identity;
            }

            return new Quaternion(a.x, a.y, a.z, (float)w).normalized;
        }

        /// <summary>
        /// 计算两个向量之间的旋转
        /// </summary>
        /// <param name="v1">向量1</param>
        /// <param name="v2">向量2</param>
        /// <returns>结果</returns>
        public static Quaternion FromToRotation(Vector3 v1, Vector3 v2, Vector3 defaultAxis)
        {
            var a = Vector3.Cross(v1, v2);
            var w = Math.Sqrt(v1.sqrMagnitude * v2.sqrMagnitude) + Vector3.Dot(v1, v2);
            if (a.sqrMagnitude < Mathf.Epsilon)
            {
                // the vectors are parallel, check w to find direction
                // if w is 0 then values are opposite, and we should rotate 180 degrees around the supplied axis
                // otherwise the vectors in the same direction and no rotation should occur
                return Math.Abs(w) < Mathf.Epsilon ? new Quaternion(defaultAxis.x, defaultAxis.y, defaultAxis.z, 0f).normalized : Quaternion.identity;
            }

            return new Quaternion(a.x, a.y, a.z, (float)w).normalized;
        }

        /// <summary>
        /// 计算两个旋转之间的旋转
        /// </summary>
        /// <param name="start">旋转1</param>
        /// <param name="end">旋转2</param>
        /// <returns>结果</returns>
        public static Quaternion FromToRotation(Quaternion start, Quaternion end)
        {
            return Quaternion.Inverse(start) * end;
        }

        /// <summary>
        ///     Create a LookRotation for a non-standard 'forward' axis.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="forwardAxis"></param>
        /// <param name="upAxis"></param>
        /// <returns></returns>
        public static Quaternion AltForwardLookRotation(Vector3 dir, Vector3 forwardAxis, Vector3 upAxis)
        {
            return Quaternion.LookRotation(dir) * Quaternion.Inverse(Quaternion.LookRotation(forwardAxis, upAxis));
        }

        /// <summary>
        /// 计算旋转的旋转轴以及角度
        /// </summary>
        /// <param name="quaternion">旋转</param>
        /// <param name="axis">旋转轴</param>
        /// <param name="angle">角度</param>
        public static void GetAngleAxis(Quaternion quaternion, out Vector3 axis, out float angle)
        {
            if (quaternion.w > 1)
            {
                quaternion = Normalize(quaternion);
            }

            // get as doubles for precision
            var qw = (double)quaternion.w;
            var qx = (double)quaternion.x;
            var qy = (double)quaternion.y;
            var qz = (double)quaternion.z;
            var ratio = Math.Sqrt(1.0d - qw * qw);

            angle = (float)(2.0d * Math.Acos(qw)) * Mathf.Rad2Deg;
            if (ratio < 0.001d)
            {
                axis = new Vector3(1f, 0f, 0f);
            }
            else
            {
                axis = new Vector3(
                    (float)(qx / ratio),
                    (float)(qy / ratio),
                    (float)(qz / ratio));
                axis.Normalize();
            }
        }

        /// <summary>
        /// 计算两个旋转的最小旋转轴
        /// </summary>
        /// <param name="a">旋转A</param>
        /// <param name="b">旋转B</param>
        /// <param name="axis">旋转轴</param>
        /// <param name="angle">旋转角度</param>
        public static void GetShortestAngleAxisBetween(Quaternion a, Quaternion b, out Vector3 axis, out float angle)
        {
            var dq = Quaternion.Inverse(a) * b;
            if (dq.w > 1)
            {
                dq = Normalize(dq);
            }

            // get as doubles for precision
            var qw = (double)dq.w;
            var qx = (double)dq.x;
            var qy = (double)dq.y;
            var qz = (double)dq.z;
            var ratio = Math.Sqrt(1.0d - qw * qw);

            angle = (float)(2.0d * Math.Acos(qw)) * Mathf.Rad2Deg;
            if (ratio < 0.001d)
            {
                axis = new Vector3(1f, 0f, 0f);
            }
            else
            {
                axis = new Vector3(
                    (float)(qx / ratio),
                    (float)(qy / ratio),
                    (float)(qz / ratio));
                axis.Normalize();
            }
        }

    }
}