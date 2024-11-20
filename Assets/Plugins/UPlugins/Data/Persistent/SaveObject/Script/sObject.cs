/////////////////////////////////////////////////////////////////////////////
//
//  Script   : sObject.cs
//  Info     : 可存储对象类型
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Data.Persistent
{
    [Serializable]
    public abstract class sObject<T> : sObject where T : sObject
    {
        protected sObject(string key) : base(key)
        {
        }

        public void Save()
        {
            USave.SetObject( Key, this);
            USave.Save();
        }

        public static T Load(string key)
        {
            var ret = USave.GetObject<T>(key);
            return ret;
        }

        public static void Save(T obj)
        {
            USave.SetObject(obj.Key, obj);
            USave.Save();
        }
    }

    public abstract class sObject
    {
        public string Key { get; protected set; }

        protected sObject(string key)
        {
            Key = key;
        }
    }
}