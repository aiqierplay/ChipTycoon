using System;
using System.Collections.Generic;

namespace Aya.TweenPro
{
    public static class UTweenPool
    {
        #region Info

        internal static Dictionary<Type, UTweenPoolList> PoolListDic = new Dictionary<Type, UTweenPoolList>();

        public static int Count
        {
            get
            {
                var count = 0;
                foreach (var kv in PoolListDic)
                {
                    count += kv.Value.Count;
                }

                return count;
            }
        }

        public static int ActiveCount
        {
            get
            {
                var count = 0;
                foreach (var kv in PoolListDic)
                {
                    count += kv.Value.ActiveCount;
                }

                return count;
            }
        }

        public static int DeActiveCount
        {
            get
            {
                var count = 0;
                foreach (var kv in PoolListDic)
                {
                    count += kv.Value.DeActiveCount;
                }

                return count;
            }
        } 

        #endregion

        public static T PreLoad<T>()
        {
            return (T)PreLoad(typeof(T));
        }

        public static object PreLoad(Type type)
        {
            var poolList = GetPoolList(type);
            return poolList.PreLoad();
        }

        public static T Spawn<T>()
        {
            var poolList = GetPoolList<T>();
            var instance = (T)poolList.Spawn();
            return instance;
        }

        public static object Spawn(Type type)
        {
            var poolList = GetPoolList(type);
            var instance = poolList.Spawn();
            return instance;
        }

        public static void DeSpawn(object instance)
        {
            if (instance == null) return;
            var type = instance.GetType();
            var poolList = GetPoolList(type);
            poolList.DeSpawn(instance);
        }

        public static UTweenPoolList GetPoolList<T>()
        {
            var type = typeof(T);
            var poolList = GetPoolList(type);
            return poolList;
        }

        public static UTweenPoolList GetPoolList(Type type)
        {
            if (PoolListDic.TryGetValue(type, out var poolList)) return poolList;
            poolList= new UTweenPoolList(type);
            PoolListDic.Add(type, poolList);
            return poolList;
        }

        public static bool Contains<T>(T instance)
        {
            var poolList = GetPoolList<T>();
            return poolList.Contains(instance);
        }
    }
}