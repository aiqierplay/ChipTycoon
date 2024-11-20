/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathVector3.cs
//  Info     : 数学辅助计算类 —— Vector3
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
        #region NaN
       
        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="vector">向量</param>
        /// <returns>结果</returns>
        public static bool IsNaN(Vector3 vector)
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
        public static Vector3 AddLength(Vector3 vector, float length)
        {
            var num = Vector3.Magnitude(vector);
            num += length;
            var normalize = Vector3.Normalize(vector);
            var result = Vector3.Scale(normalize, new Vector3(num, num, num));
            return result;
        }

        /// <summary>
        /// 设置向量的长度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="length">长度</param>
        /// <returns>结果</returns>
        public static Vector3 SetLength(Vector3 vector, float length)
        {
            var normalize = Vector3.Normalize(vector);
            var result = normalize * length;
            return result;
        }

        #endregion

        #region Abs
      
        /// <summary>
        /// 对向量的所有值取绝对值
        /// </summary>
        /// <param name="vector3">向量</param>
        /// <returns>结果</returns>
        public static Vector3 Abs(Vector3 vector3)
        {
            var result = new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
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
        public static float SqrDistance(Vector3 from, Vector3 to)
        {
            var result = Mathf.Pow(from.x - to.x, 2) + Mathf.Pow(from.y - to.y, 2) + Mathf.Pow(from.z - to.z, 2);
            return result;
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
        public static Vector3 Clamp(Vector3 input, Vector3 min, Vector3 max)
        {
            input.x = Mathf.Clamp(input.x, min.x, max.x);
            input.y = Mathf.Clamp(input.y, min.y, max.y);
            input.z = Mathf.Clamp(input.z, min.z, max.z);
            return input;
        }

        #endregion

        #region Tangent
       
        /// <summary>
        /// 计算向上方向的正交
        /// </summary>
        /// <param name="forward">方向</param>
        /// <param name="up">上方</param>
        /// <returns>结果</returns>
        public static Vector3 GetForwardTangent(Vector3 forward, Vector3 up)
        {
            return Vector3.Cross(Vector3.Cross(up, forward), up);
        } 

        #endregion
    }
}