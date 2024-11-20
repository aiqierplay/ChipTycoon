/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathTriangle.cs
//  Info     : 数学辅助计算类 —— 三角形
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
    {  /// <summary>
       /// 点T是否在三角形内
       /// </summary>
       /// <param name="point">点</param>
       /// <param name="a">三角形A点坐标</param>
       /// <param name="b">三角形B点坐标</param>
       /// <param name="c">三角形C点坐标</param>
       /// <returns>结果</returns>
        public static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            var temp1 = IsPointInIntercept(a, b, c, point);
            var temp2 = IsPointInIntercept(a, c, b, point);
            var temp3 = IsPointInIntercept(b, c, a, point);
            var result = temp1 && temp2 && temp3;
            return result;
        }

        /// <summary>
        /// 点T是否在线AB和C的截距范围内 (三角形碰撞检测辅助方法)
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="a">线端点A</param>
        /// <param name="b">线端点B</param>
        /// <param name="c">三角形的另一个点C</param>
        /// <returns>结果</returns>
        public static bool IsPointInIntercept(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            // AB延长线在Y轴上的截点，AB过C和T平行线在坐标轴上的截点
            float p1, p2, pt;
            // 斜率不存在时
            if (Math.Abs(a.x - b.x) < FloatPrecision)
            {
                p1 = a.x;
                p2 = c.x;
                pt = point.x;
            }
            else
            {
                // 斜率为0时
                if (Math.Abs(a.y - b.y) < FloatPrecision)
                {
                    p1 = a.y;
                    p2 = c.y;
                    pt = point.y;
                }
                // 斜率不为0时
                else
                {
                    // 斜率
                    var k = (a.y - b.y) / (a.x - b.x);
                    var bb = a.y - k * a.x;
                    // Y轴的截距
                    p1 = bb;
                    p2 = c.y - k * c.x;
                    pt = point.y - k * point.x;
                }
            }
            var result = (pt <= p2 && pt >= p1) || (pt <= p1 && pt >= p2);
            return result;
        }

        /// <summary>
        /// 取三角形内的一个随机点
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="p3">点3</param>
        /// <returns>结果</returns>
        public static Vector3 GetRandPosInTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var result = Vector3.Lerp(p1, p2, UnityEngine.Random.value);
            result = Vector3.Lerp(result, p3, UnityEngine.Random.value);
            return result;
        }
    }
}