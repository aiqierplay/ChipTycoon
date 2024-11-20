/////////////////////////////////////////////////////////////////////////////
//
//  Script   : PRD.cs
//  Info     : Pseudo-random distribution 伪随机分布
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Tip      : 用于控制暴击率，例如 20% 暴击，将会稳定3-7次触发一次
//
//  Copyright : Aya Game Studio 2018
//
//  URL       : http://www.techweb.com.cn/shoujiyouxi/2015-07-21/2178195.shtml
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using Aya.Security;

namespace Aya.Maths
{
    public class PRD
    {
        /// <summary>
        /// 目标稳定概率
        /// </summary>
        public cFloat Probability { get; private set; }
        /// <summary>
        /// 初始单倍概率(由P计算得出)
        /// </summary>
        public cFloat C { get; private set; }
        /// <summary>
        /// 第 K 次尝试
        /// </summary>
        public cInt K { get; private set; }
        /// <summary>
        /// 第 K 次的命中概率
        /// </summary>
        public cFloat PK => C * K;

        /// <summary>
        /// 最大 K 值 = 最大连续不命中次数 + 1
        /// </summary>
        public cInt KMax { get; private set; }

        protected static System.Random Rand = new System.Random();

        // How to use
//        public static void Test()
//        {
//            var prd = new PRD(0.2f);
//            var count = 0;
//            for (var i = 0; i < 100; i++)
//            {
//                var hit = prd.Check();
//                if (!hit)
//                {
//                    Debug.Log("false".ToMarkup(Color.gray));
//                }
//                else
//                {
//                    Debug.Log("true".ToMarkup(Color.cyan));
//                    count++;
//                }
//            }
//            Debug.Log("Pa: " + (count * 1f / 100));
//        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="targetProbability">目标概率</param>
        public PRD(float targetProbability)
        {
            Probability = Mathf.Clamp01(targetProbability);
            C = CalcCFromP(targetProbability);
            KMax = (int) (1f / C);
            K = 1;
        }

        /// <summary>
        /// 检测是否命中（每次调用都会往后计次，如必要请缓存结果使用）
        /// </summary>
        /// <returns>命中结果</returns>
        public bool Check()
        {
            var rand = (float)Rand.NextDouble();
            if (rand <= PK)
            {
                K = 1;
                return true;
            }
            else
            {
                K++;
                if (K >= KMax)
                {
                    K = 1;
                }
                return false;
            }
        }

        #region Static
        /// <summary>
        /// 根据 P 值 计算出初始 C 值
        /// </summary>
        /// <param name="probability">预期概率</param>
        /// <param name="eps">精度偏差</param>
        /// <returns>结果</returns>
        public static float CalcCFromP(float probability, float eps = 0.01f)
        {
            var c1 = 0f;
            var c2 = 1f;
            var c3 = 0f;
            var ret = probability;
            do
            {
                c3 = (c1 + c2) / 2f;
                var pa = CalcPFromC(c3);
                if (Mathf.Abs(pa - probability) <= eps)
                {
                    ret = c3;
                    break;
                }
                if (pa < probability)
                {
                    c1 = c3;
                }
                else
                {
                    c2 = c3;
                }
            } while (true);
            return ret;
        }

        /// <summary>
        /// 计算指定 C 值的实际概率
        /// </summary>
        /// <param name="c">C</param>
        /// <returns>结果</returns>
        public static float CalcPFromC(float c)
        {
            var k = 0;              // 第 k 次
            var pk = 0f;            // 第 k 次命中概率
            var pk_1 = 1f;          // 第 k-1 次命中概率
            var kc = 0f;            // 命中次数和 = 累加 命中概率 * 尝试次数
            var pc = 0f;            // 总概率
            var p = 0f;             // 平均命中概率
            do
            {
                k++;
                if (k == 1)
                {
                    pk = c;
                    pk_1 = 1f - pk;
                }
                else
                {
                    pk = (c * k) * pk_1;
                    pk_1 = (1f - c * k) * pk_1;
                }
                kc += pk * k;
                pc += pk;
            } while (c * k < 1f);
            p = 1f / kc;
            return p;
        } 
        #endregion
    }
}

