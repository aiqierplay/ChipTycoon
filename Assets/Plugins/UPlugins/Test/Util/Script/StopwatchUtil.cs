/////////////////////////////////////////////////////////////////////////////
//
//  Script   : StopwatchUtil.cs
//  Info     : 用于测试函数执行时间
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//	How to use 1 :	StopwatchUtil.Start(key);
//					// To do something...
//					var periodTime = StopwatchUtil.Stop(key);
//
//	How to use 2 :	var time = StopwatchUtil.TestOnce(action());
//					var totalTime = StopwatchUtil.TestCount(action(), count);
//					var averageTime = StopwatchUtil.TestAverage(action(), count);
//
//  Copyright : Aya Game Studio 2017
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Aya.Extension;

namespace Aya.Test
{
	public class StopwatchUtil
	{
		protected static readonly Dictionary<int, Stopwatch> Dic = new Dictionary<int, Stopwatch>();
		private static readonly Stopwatch Stopwatch = new Stopwatch();

		#region Start & Stop

		/// <summary>
		/// 开启指定键值的定时器，如果是运行中的定时器则会被重设，使用Start后务必记得要调用Stop释放资源
		/// </summary>
		/// <param name="key">键</param>
		public static void Start(int key)
		{
			var stopwatch = Dic.GetOrAdd(key, new Stopwatch());
			stopwatch.Reset();
			stopwatch.Start();
		}

		/// <summary>
		/// 停止指定键值的定时器，并释放
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="log">输出日志</param>
		/// <returns>结果</returns>
		public static float Stop(int key, bool log = true)
		{
			var stopwatch = Dic.GetValue(key);
            stopwatch.Stop();
			var result = stopwatch.Elapsed.Milliseconds;
			Dic.Remove(key);
		    if (log) Log(result);
            return result;
		}

		#endregion

		#region TestOnce

		/// <summary>
		/// 测试某个方法执行消耗的时间，单位为毫秒
		/// </summary>
		/// <param name="action">方法</param>
		/// <param name="log">输出日志</param>
		/// <returns>结果</returns>
		public static float TestOnce(Action action, bool log = true)
		{
			if (action == null) return 0f;
			Stopwatch.Reset();
			Stopwatch.Start();
			action();
			Stopwatch.Stop();
			var result = Stopwatch.Elapsed.Milliseconds;
			if (log) Log(result);
			return result;
		}

		#endregion

		#region TestCount

		/// <summary>
		/// 测试某个方法执行指定次数的总时间，单位为毫秒
		/// </summary>
		/// <param name="action">方法</param>
		/// <param name="count">执行次数</param>
		/// <param name="log">输出日志</param>
		/// <returns>结果</returns>
		public static float TestCount(Action action, int count, bool log = true)
		{
		    if (action == null) return 0f;
		    Stopwatch.Reset();
		    Stopwatch.Start();
		    for (var i = 0; i < count; i++)
		    {
		        action();
		    }
            Stopwatch.Stop();
		    var result = Stopwatch.Elapsed.Milliseconds;
			if (log) Log(result);
			return result;
		}

		#endregion

		#region TestAverage

		/// <summary>
		/// 测试某个方法执行指定次数的平均时间，单位为毫秒
		/// </summary>
		/// <param name="action">方法</param>
		/// <param name="count">执行次数</param>
		/// <param name="log">输出日志</param>
		/// <returns>结果</returns>
		public static float TestAverage(Action action, int count, bool log = true)
		{
		    var result = TestCount(action, count, false) * 1f / count;
			if (log) Log(result);
			return result;
		}

		#endregion

		/// <summary>
		/// 输出日志
		/// </summary>
		/// <param name="time">时间</param>
		private static void Log(float time)
		{
			UnityEngine.Debug.Log("<color>[Stopwatch Test]</color>\t" + " " + DateTime.Now.ToString("u") + "\t" + +time);
		}
	}
}
