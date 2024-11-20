/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathAlgorithm.cs
//  Info     : 数学辅助计算类 —— 算法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2022
//
/////////////////////////////////////////////////////////////////////////////
using System.Runtime.CompilerServices;

namespace Aya.Maths
{
    public static partial class MathUtil
    {
        /// <summary>
        /// 斐波那契数列
        /// </summary>
        /// <param name="n">第N项</param>
        /// <returns>结果</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Fibonacci(int n)
        {
            var a = 0;
            var b = 1;
            for (var i = 0; i < n; i++)
            {
                var temp = a;
                a = b;
                b = temp + b;
            }

            return a;
        }
    }
}