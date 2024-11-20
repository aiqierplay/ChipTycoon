/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathRotate.cs
//  Info     : 数学辅助计算类 —— 旋转
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
        /// 逆时针方向旋转角度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="angle">角度</param>
        /// <returns>结果</returns>
        public static Vector2 RotateByDegrease(Vector2 vector, float angle)
        {
            return RotateByRadians(vector, angle * DegToRad);
        }

        /// <summary>
        /// 逆时针方向旋转弧度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="radians">弧度</param>
        /// <returns>结果</returns>
        public static Vector2 RotateByRadians(Vector2 vector, float radians)
        {
            var ca = Math.Cos(radians);
            var sa = Math.Sin(radians);
            var rx = vector.x * ca - vector.y * sa;
            return new Vector2((float)rx, (float)(vector.x * sa + vector.y * ca));
        }

        /// <summary>
        /// 朝向指定向量旋转，并保持向量长度
        /// </summary>
        /// <param name="from">起始向量</param>
        /// <param name="to">目标向量</param>
        /// <param name="radians">旋转弧度</param>
        /// <returns>结果</returns>
        public static Vector2 RotateTowardRadians(Vector2 from, Vector2 to, float radians)
        {
            var a1 = Mathf.Atan2(from.y, from.x);
            var a2 = Mathf.Atan2(to.y, to.x);
            a2 = ShortenAngleToAnother(a2, a1);
            var ra = a2 - a1 >= 0f ? a1 + radians : a1 - radians;
            var l = from.magnitude;
            return new Vector2(Mathf.Cos(ra) * l, Mathf.Sin(ra) * l);
        }

        /// <summary>
        /// 朝向指定向量旋转，并保持向量长度
        /// </summary>
        /// <param name="from">起始向量</param>
        /// <param name="to">目标向量</param>
        /// <param name="degrease">旋转角度</param>
        /// <returns>结果</returns>
        public static Vector2 RotateToward(Vector2 from, Vector2 to, float degrease)
        {
            return RotateTowardRadians(from, to, degrease * DegToRad);
        }

        /// <summary>
        /// 朝向指定向量旋转，并保持向量长度，且限制大于0
        /// </summary>
        /// <param name="from">起始向量</param>
        /// <param name="to">目标向量</param>
        /// <param name="radians">旋转弧度</param>
        /// <returns>结果</returns>
        public static Vector2 RotateTowardRadiansClamped(Vector2 from, Vector2 to, float radians)
        {
            var a1 = Mathf.Atan2(from.y, from.x);
            var a2 = Mathf.Atan2(to.y, to.x);
            a2 = ShortenAngleToAnother(a2, a1);
            var da = a2 - a1;
            var ra = a1 + Mathf.Clamp(Mathf.Abs(radians), 0f, Mathf.Abs(da)) * Mathf.Sign(da);
            var l = from.magnitude;
            return new Vector2(Mathf.Cos(ra) * l, Mathf.Sin(ra) * l);
        }

        /// <summary>
        /// 朝向指定向量旋转，并保持向量长度，且限制大于0
        /// </summary>
        /// <param name="from">起始向量</param>
        /// <param name="to">目标向量</param>
        /// <param name="degrease">旋转角度</param>
        /// <returns>结果</returns>
        public static Vector2 RotateTowardClamped(Vector2 from, Vector2 to, float degrease)
        {
            return RotateTowardRadiansClamped(from, to, degrease * DegToRad);
        }

        /// <summary>
        /// 点绕指定坐标旋转指定角度
        /// </summary>
        /// <param name="origin">原始点</param>
        /// <param name="point">旋转中心</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>旋转后的点</returns>
        public static Vector2 Rotate(Vector2 origin, Vector2 point, float angle)
        {
            try
            {
                // 与中心点相同则不处理
                if (origin == point) return origin;
                // 中心点
                var xx = point.x;
                var yy = point.x;
                var L = Mathf.Sqrt(Mathf.Pow(origin.x - xx, 2) + Mathf.Pow(origin.y - yy, 2));
                var tanSita = (origin.y - yy) * 1.0f / (origin.x - xx);
                var sita = Mathf.Atan(tanSita);
                if (origin.y > yy && sita < 0) sita += Mathf.PI;
                if (origin.y < yy && sita > 0) sita += Mathf.PI;
                if (origin.y < yy && sita < 0) sita += 2 * Mathf.PI;
                if (Math.Abs(origin.y - yy) < FloatPrecision && origin.x > xx) sita = 0;
                if (Math.Abs(origin.y - yy) < FloatPrecision && origin.x < xx) sita = Mathf.PI;
                // 旋转后的点
                var rx = xx + L * Mathf.Cos(sita - angle * 2 * Mathf.PI / 360);
                var ry = yy + L * Mathf.Sin(sita - angle * 2 * Mathf.PI / 360);
                var result = new Vector2(rx, ry);
                return result;
            }
            catch
            {
                return Vector2.zero;
            }
        }

        /// <summary>
        /// 点绕指定坐标按轴旋转指定角度
        /// </summary>
        /// <param name="origin">原始点</param>
        /// <param name="point">旋转中心</param>
        /// <param name="axis">旋转轴</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>结果</returns>
        public static Vector3 Rotate(Vector3 origin, Vector3 point, Vector3 axis, float angle)
        {
            // 旋转系数
            var quaternion = Quaternion.AngleAxis(angle, axis);
            // 旋转中心到源点的偏移向量
            var offset = origin - point;
            // 旋转偏移向量，得到旋转中心到目标点的偏移向量
            offset = quaternion * offset;
            var result = point + offset;
            return result;
        }
    }
}