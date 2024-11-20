using System;
using System.Collections.Generic;

namespace Aya.TweenPro
{
    public class UTweenPoolList
    {
        public Type Type;

        internal HashSet<object> SpawnList = new HashSet<object>();
        internal HashSet<object> DeSpawnList = new HashSet<object>();

        public int ActiveCount => SpawnList.Count;
        public int DeActiveCount => DeSpawnList.Count;
        public int Count => ActiveCount + DeActiveCount;

        public UTweenPoolList(Type type)
        {
            Type = type;
        }

        public object PreLoad()
        {
            var instance = Activator.CreateInstance(Type);
            DeSpawnList.Add(instance);
            return instance;
        }

        public object Spawn()
        {
            object instance = null;
            if (DeSpawnList.Count > 0)
            {
                foreach (var obj in DeSpawnList)
                {
                    instance = obj;
                    break;
                }

                DeSpawnList.Remove(instance);
            }
            else
            {
                instance = Activator.CreateInstance(Type);
            }

            SpawnList.Add(instance);
            return instance;
        }

        public void DeSpawn(object instance)
        {
            if (instance == null) return;
            if (!SpawnList.Contains(instance)) return;
            SpawnList.Remove(instance);
            DeSpawnList.Add(instance);
        }

        public bool Contains(object instance)
        {
            return SpawnList.Contains(instance) || DeSpawnList.Contains(instance);
        }
    }
}