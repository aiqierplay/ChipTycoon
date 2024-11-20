/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathSphere.cs
//  Info     : 数学辅助计算类 —— 球
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2022
//
/////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Maths
{
    public static partial class MathUtil
    {
        /// <summary>
        /// 获取球体所有顶点坐标
        /// </summary>
        /// <param name="center">球心</param>
        /// <param name="radius">半径</param>
        /// <param name="angle">角度</param>
        /// <returns>结果</returns>
        public static Vector3[] GetSphereVertices(Vector3 center, float radius, int angle)
        {
            var vertices = new List<Vector3>();
            vertices.Clear();
            var direction = Vector3.zero; // 射线方向
            var point = new Vector3(center.x, center.y + radius, center.z);
            vertices.Add(point); // 添加圆球最高点
            // 通过球坐标系方式遍历所需的点并加入网格顶点中
            for (var theta = 1; theta <= angle; theta += 1)
            {
                for (var alpha = 0; alpha < 360; alpha += 1)
                {
                    var radTheta = theta * Mathf.Deg2Rad;
                    var radAlpha = alpha * Mathf.Deg2Rad;

                    // 计算出方向向量
                    direction.Set(Mathf.Sin(radTheta) * Mathf.Cos(radAlpha), Mathf.Cos(radTheta), Mathf.Sin(radTheta) * Mathf.Sin(radAlpha));
                    point = center + radius * direction;
                    vertices.Add(point);
                }
            }

            // 加入圆心点
            vertices.Add(center);
            return vertices.ToArray();
        }

        /// <summary>
        /// 获取球表面平均分布点
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="count">数量</param>
        /// <returns>结果</returns>
        public static Vector3[] GetSphericalDistributionPoints(float radius, int count)
        {
            var result = new Vector3[count];
            for (var i = 0; i < count; i++)
            {
                var p = GetSphericalDistributionPoints(radius, count, i);
                result[i] = p;
            }

            return result;
        }

        /// <summary>
        /// 获取球表面平均分布点的第 index 个点
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="count">数量</param>
        /// <param name="index">索引</param>
        /// <returns>结果</returns>
        public static Vector3 GetSphericalDistributionPoints(float radius, int count, int index)
        {
            var fi = (Mathf.Sqrt(5f) - 1f) / 2f; // 0.618
            var i = index + 1;
            var z = (2f * i - 1) / count - 1;
            var x = Mathf.Sqrt(1 - Mathf.Pow(z, 2f)) * Mathf.Cos(2f * Mathf.PI * i * fi);
            var y = Mathf.Sqrt(1 - Mathf.Pow(z, 2f)) * Mathf.Sin(2f * Mathf.PI * i * fi);
            var p = new Vector3(x, y, z);
            p = p.normalized * radius;
            return p;
        }
    }
}