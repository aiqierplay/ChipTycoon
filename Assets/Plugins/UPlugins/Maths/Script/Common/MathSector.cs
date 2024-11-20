/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathSector.cs
//  Info     : 数学辅助计算类 —— 扇形
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2022
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Aya.Maths
{
    public static partial class MathUtil
    {
        /// <summary>
        /// 获取扇形区域的点集合
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="egdePoint">边界点</param>
        /// <param name="angleDiffer">角度</param>
        /// <returns>结果</returns>
        public static Vector3[] GetSectorPoints(Vector3 center, Vector3 egdePoint, float angleDiffer)
        {
            // 取两点的向量
            var dir = egdePoint - center;
            // 获取扇形的半径
            var radius = dir.magnitude;

            // 取数组长度 如60度的弧边取61个点 0~60 再加上一个圆心点
            var points = new Vector3[(int)(angleDiffer / 3) + 2];
            // 取圆心点
            points[0] = center;
            var startEuler = (int)Vector2.Angle(Vector2.right, new Vector2(dir.x, dir.z));
            for (int i = startEuler, j = 1; i <= angleDiffer + startEuler; j++, i += 3)
            {
                var angle = Mathf.Deg2Rad * i;
                // 高度差的绝对值
                var differ = Mathf.Abs(Mathf.Cos(angle - (float)(0.5 * angleDiffer * Mathf.Deg2Rad)) * egdePoint.y - egdePoint.y);
                // 给底面点赋值
                points[j] = center + new Vector3(radius * Mathf.Cos(angle), egdePoint.y + differ, radius * Mathf.Sin(angle));
            }

            return points;
        }
    }
}