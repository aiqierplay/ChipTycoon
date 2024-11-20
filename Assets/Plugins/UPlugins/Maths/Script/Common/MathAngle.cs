/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathAngle.cs
//  Info     : 数学辅助计算类 —— 角度 / 弧度
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
        #region Radian <-> Degree

        /// <summary>
        /// 弧度转换为角度
        /// </summary>
        /// <param name="radian">弧度</param>
        /// <returns>结果</returns>
        public static float RadiansToDegrees(float radian)
        {
            return radian * RadToDeg;
        }

        /// <summary>
        /// 角度转换为弧度
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns>结果</returns>
        public static float DegreesToRadians(float angle)
        {
            return angle * DegToRad;
        }

        #endregion

        #region Angle
       
        /// <summary>
        /// 计算两个向量的夹角度数
        /// </summary>
        /// <param name="vector1">向量1</param>
        /// <param name="vector2">向量2</param>
        /// <returns>结果</returns>
        public static float Angle(Vector2 vector1, Vector2 vector2)
        {
            var d = Vector2.Dot(vector1, vector2) / ((double)vector1.magnitude * vector2.magnitude);
            if (d >= 1d)
            {
                return 0f;
            }

            if (d <= -1d)
            {
                return 180f;
            }

            return (float)Math.Acos(d) * RadToDeg;
        }

        /// <summary>
        /// 计算两个向量的夹角度数
        /// </summary>
        /// <param name="vector1">向量1</param>
        /// <param name="vector2">向量2</param>
        /// <returns>结果</returns>
        public static float Angle(Vector3 vector1, Vector3 vector2)
        {
            var d = Math.Sqrt(vector1.sqrMagnitude * (double)vector2.sqrMagnitude);
            if (d < MinDouble)
            {
                return 0f;
            }

            d = Vector3.Dot(vector1, vector2) / d;
            if (d >= 1d)
            {
                return 0f;
            }

            if (d <= -1d)
            {
                return 180f;
            }

            return (float)Math.Acos(d) * RadToDeg;
        }

        /// <summary>
        /// 从 x 定义的前向获取角度（以度为单位）
        /// </summary>
        /// <param name="forward">方向</param>
        /// <returns>结果</returns>
        public static float Angle(Vector2 forward)
        {
            return Angle(forward.y, forward.x);
        }

        /// <summary>
        /// 从 x 定义的前向获取角度（以度为单位）
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>结果</returns>
        public static float Angle(float x, float y)
        {
            return Mathf.Atan2(y, x) * RadToDeg;
        }

        /// <summary>
        /// 计算两个向量的夹角度数(带正负号)
        /// </summary>
        /// <param name="vector1">向量1</param>
        /// <param name="vector2">向量2</param>
        /// <param name="normal">所处平面法向量</param>
        /// <returns>结果</returns>
        public static float AngleSigned(Vector3 vector1, Vector3 vector2, Vector3 normal)
        {
            var result = Mathf.Atan2(Vector3.Dot(normal, Vector3.Cross(vector1, vector2)), Vector3.Dot(vector1, vector2)) * Mathf.Rad2Deg;
            return result;
        }

        /// <summary>
        /// 一个向量和平面的夹角
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="planeNormal">平面法线</param>
        /// <returns>结果</returns>
        public static float AngleVectorPlane(Vector3 vector, Vector3 planeNormal)
        {
            var num = Vector3.Dot(vector, planeNormal);
            var num2 = (float)Math.Acos(num);
            var result = 1.57079637f - num2;
            return result;
        }

        /// <summary>
        /// 限制角度到360之间
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns>结果</returns>
        public static float ClampAngle360(float angle)
        {
            while (angle < 0)
            {
                angle += 360f;
            }

            while (angle > 360)
            {
                angle -= 360f;
            }

            return angle;
        }

        /// <summary>
        /// 计算旋转指定角度的向量
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="useRadians">是否使用弧度</param>
        /// <param name="yDominant">以y轴为起始</param>
        /// <returns>结果</returns>
        public static Vector2 AngleToVector2(float angle, bool useRadians = false, bool yDominant = false)
        {
            if (!useRadians)
            {
                angle *= DegToRad;
            }

            return yDominant ? new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) : new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        /// <summary>
        /// 在逆时针方向上偏离某个轴的角度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="axis">轴</param>
        /// <returns>结果</returns>
        public static float AngleOff(Vector2 vector, Vector2 axis)
        {
            if (axis.sqrMagnitude < EpsilonSingle)
            {
                return float.NaN;
            }

            axis.Normalize();
            var tang = new Vector2(-axis.y, axis.x);
            return Angle(vector, axis) * Mathf.Sign(Vector2.Dot(vector, tang));
        }

        #endregion

        #region Normalize

        /// <summary>
        /// 将角度限制到 (-Pi， Pi) 之间
        /// </summary>
        /// <param name="angle">角度</param>
        /// <returns>结果</returns>
        public static float NormalizeAngle(float angle)
        {
            const float rd = 180;
            return Wrap(angle, -rd, rd);
        }

        /// <summary>
        /// 将弧度限制到 (-Pi， Pi)
        /// </summary>
        /// <param name="angle">弧度</param>
        /// <returns>结果</returns>
        public static float NormalizeAngleRadian(float angle)
        {
            const float rd = Pi;
            return Wrap(angle, -rd, rd);
        }

        /// <summary>
        /// 归一化角度
        /// </summary>
        /// <param name="angle">旋转</param>
        /// <returns>结果</returns>
        public static Vector3 NormalizeEuler(Vector3 angle)
        {
            angle.x = NormalizeAngle(angle.x);
            angle.y = NormalizeAngle(angle.y);
            angle.z = NormalizeAngle(angle.z);
            return angle;
        }

        /// <summary>
        /// 归一化弧度
        /// </summary>
        /// <param name="angle">旋转</param>
        /// <returns>结果</returns>
        public static Vector3 NormalizeEulerRadian(Vector3 angle)
        {
            angle.x = NormalizeAngleRadian(angle.x);
            angle.y = NormalizeAngleRadian(angle.y);
            angle.z = NormalizeAngleRadian(angle.z);
            return angle;
        }

        /// <summary>
        /// 归一化两个角度的差值
        /// </summary>
        /// <param name="dep">角度1</param>
        /// <param name="ind">角度2</param>
        /// <returns>结果</returns>
        public static float NormalizeAngleToAnother(float dep, float ind)
        {
            const float div = 360f;
            var v = (dep - ind) / div;
            return dep - (float)Math.Floor(v) * div;
        }

        /// <summary>
        /// 归一化两个弧度的差值
        /// </summary>
        /// <param name="dep">弧度1</param>
        /// <param name="ind">弧度2</param>
        /// <returns>结果</returns>
        public static float NormalizeAngleRadianToAnother(float dep, float ind)
        {
            const float div = TwoPi;
            var v = (dep - ind) / div;
            return dep - (float)Math.Floor(v) * div;
        }

        #endregion

        #region Nearest / Shorten
        
        /// <summary>
        /// 两个角度之间的最近角度
        /// </summary>
        /// <param name="angle1">角度1</param>
        /// <param name="angle2">角度2</param>
        /// <returns>结果</returns>
        public static float NearestAngleBetween(float angle1, float angle2)
        {
            const float rd = 180f;
            var ra = Wrap(angle2 - angle1, 0, rd * 2f);
            if (ra > rd)
            {
                ra -= rd * 2f;
            }

            return ra;
        }

        /// <summary>
        /// 两个角度之间的最近弧度
        /// </summary>
        /// <param name="angle1">角度1</param>
        /// <param name="angle2">角度2</param>
        /// <returns>结果</returns>
        public static float NearestAngleRadianBetween(float angle1, float angle2)
        {
            const float rd = Pi;
            var ra = Wrap(angle2 - angle1, 0, rd * 2f);
            if (ra > rd)
            {
                ra -= rd * 2f;
            }

            return ra;
        }

        /// <summary>
        /// 从一个角度到另一个角度的最近旋转角度
        /// </summary>
        /// <param name="dep">角度1</param>
        /// <param name="ind">角度2</param>
        /// <returns>结果</returns>
        public static float ShortenAngleToAnother(float dep, float ind)
        {
            return ind + NearestAngleBetween(ind, dep);
        }

        /// <summary>
        /// 从一个角度到另一个角度的最近旋转弧度
        /// </summary>
        /// <param name="dep">角度1</param>
        /// <param name="ind">角度2</param>
        /// <returns>结果</returns>
        public static float ShortenAngleRadianToAnother(float dep, float ind)
        {
            return ind + NearestAngleRadianBetween(ind, dep);
        } 

        #endregion
    }
}