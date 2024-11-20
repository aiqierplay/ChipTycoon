/////////////////////////////////////////////////////////////////////////////
//
//  Script   : LowPerformace.cs
//  Info     : 低性能模拟
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  How to use : Add this to any gameObject and set a suitable count value to simulate low performce machine.
//	Refer :		 Intel Xeon E3-1231v3 , 500w Count, FPS 60 -> 30;
//
//  Copyright : Aya Game Studio 2016
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Aya.Test 
{
	public class LowPerformace : MonoBehaviour
	{
		/// <summary>
		/// 循环次数
		/// </summary>
		public int Count = 5000000;

		private int _sum;

		void Update() 
		{
			_sum = 0;
			for (var i = 0; i < Count; i++)
			{
				_sum += i;
			}
		}
	}
}
