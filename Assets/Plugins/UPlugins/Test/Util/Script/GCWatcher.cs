/////////////////////////////////////////////////////////////////////////////
//
//  Script   : GCWatcher.cs
//  Info     : GC监视器 —— 使用弱引用字典实现监测类的析构释放，在 Unity 下有待验证
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Runtime.CompilerServices;

namespace Aya.Test
{
    public static class GCWatcher
    {
        private static readonly ConditionalWeakTable<object, NotifyWhenGC> WatcherDic = new ConditionalWeakTable<object, NotifyWhenGC>();

        private sealed class NotifyWhenGC
        {
            private Action Value { get; }

            internal NotifyWhenGC(Action value)
            {
                Value = value;
            }

            ~NotifyWhenGC()
            {
                Value?.Invoke();
            }
        }

        public static T GCWatch<T>(this T obj, Action callback) where T : class
        {
            WatcherDic.Add(obj, new NotifyWhenGC(callback));
            return obj;
        }
    }
}
