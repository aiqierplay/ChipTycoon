/////////////////////////////////////////////////////////////////////////////
//
//  Script   : RuntimeUtil.cs
//  Info     : 基于 Time.realtimeSinceStartup 的执行时间测试类
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Test
{
    public static class RuntimeUtil
    {
        private static readonly Stack<float> BeginTime = new Stack<float>();

        #region Begin / End

        public static void Begin()
        {
            var beginTime = Time.realtimeSinceStartup;
            BeginTime.Push(beginTime);
        }

        public static float End(bool log = true)
        {
            var beginTime = BeginTime.Pop();
            var end = Time.realtimeSinceStartup;
            var duration = ((end - beginTime) * 1000);
            if (log)
            {
                Log(duration);
            }

            return duration;
        }

        #endregion

        #region Test Once / Action / Average

        public static float TestOnce(Action action, bool log = true)
        {
            if (action == null) return 0f;
            Begin();
            action();
            var duration = End(true);
            return duration;
        }

        public static float TestCount(Action action, int count, bool log = true)
        {
            if (action == null) return 0f;
            var durationSum = 0f;
            for (var i = 0; i < count; i++)
            {
                Begin();
                action();
                var duration = End(false);
                durationSum += duration;
            }

            if (log)
            {
                Log(durationSum);
            }

            return durationSum;
        }

        public static float TestAverage(Action action, int count, bool log = true)
        {
            if (action == null) return 0f;
            var durationSum = 0f;
            for (var i = 0; i < count; i++)
            {
                Begin();
                action();
                var duration = End(false);
                durationSum += duration;
            }

            var durationAverage = durationSum / count;
            if (log)
            {
                Log(durationAverage);
            }

            return durationAverage;
        }

        #endregion

        #region Private

        private static void Log(float time)
        {
            Debug.Log("<color=yellow>[Runtime Test]</color>\t"+ " " + DateTime.Now.ToString("u") + "\t" + time.ToString("F2"));
        } 
        
        #endregion
    }
}
