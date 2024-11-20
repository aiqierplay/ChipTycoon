/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathVector4.cs
//  Info     : 数学辅助计算类 —— Vector4
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
        public static bool IsNaN(Vector4 vector)
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
        public static Vector4 AddLength(Vector4 vector, float length)
        {
            var num = Vector4.Magnitude(vector);
            num += length;
            var normalize = Vector4.Normalize(vector);
            var result = Vector4.Scale(normalize, new Vector4(num, num, num));
            return result;
        }

        /// <summary>
        /// 设置向量的长度
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="length">长度</param>
        /// <returns>结果</returns>
        public static Vector4 SetLength(Vector4 vector, float length)
        {
            var normalize = Vector4.Normalize(vector);
            var result = normalize * length;
            return result;
        }

        #endregion

        #region Abs

        /// <summary>
        /// 对向量的所有值取绝对值
        /// </summary>
        /// <param name="vector">向量</param>
        /// <returns>结果</returns>
        public static Vector4 Abs(Vector4 vector)
        {
            var result = new Vector4(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z), Mathf.Abs(vector.w));
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
        public static float SqrDistance(Vector4 from, Vector4 to)
        {
            var result = Mathf.Pow(from.x - to.x, 2) + Mathf.Pow(from.y - to.y, 2) + Mathf.Pow(from.z - to.z, 2) + Mathf.Pow(from.w - to.w, 2);
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
        public static Vector4 Clamp(Vector4 input, Vector4 min, Vector4 max)
        {
            input.x = Mathf.Clamp(input.x, min.x, max.x);
            input.y = Mathf.Clamp(input.y, min.y, max.y);
            input.z = Mathf.Clamp(input.z, min.z, max.z);
            input.w = Mathf.Clamp(input.w, min.w, max.w);
            return input;
        }

        #endregion
    }
}