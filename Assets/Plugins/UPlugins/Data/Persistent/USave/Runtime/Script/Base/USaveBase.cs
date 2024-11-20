using System;
using System.Collections.Generic;

namespace Aya.Data.Persistent
{
    public abstract class USaveBase
    {
        public virtual bool SupportAsync => true;

        #region Key List
        
        public abstract IEnumerable<string> GetAllKeys(string slotKey); 

        #endregion

        #region Get / Set Value

        public virtual T GetValue<T>(string valueKey, T defaultValue = default(T))
        {
            var slotKey = USave.CurrentSlotKey;
            return GetValue(slotKey, valueKey, defaultValue);
        }

        public virtual void SetValue<T>(string valueKey, T value)
        {
            var slotKey = USave.CurrentSlotKey;
            SetValue(slotKey, valueKey, value);
        }

        public abstract T GetValue<T>(string slotKey, string valueKey, T defaultValue = default(T));
        public abstract void SetValue<T>(string slotKey, string valueKey, T value);

        #endregion

        #region Get / Set Object

        public virtual T GetObject<T>(string valueKey, T defaultValue = default(T))
        {
            var slotKey = USave.CurrentSlotKey;
            return GetObject(slotKey, valueKey, defaultValue);
        }

        public virtual void SetObject<T>(string valueKey, T value)
        {
            var slotKey = USave.CurrentSlotKey;
            SetObject(slotKey, valueKey, value);
        }

        public abstract T GetObject<T>(string slotKey, string valueKey, T defaultValue = default(T));
        public abstract void SetObject<T>(string slotKey, string valueKey, T value);

        #endregion

        #region Exsit Key

        public virtual bool ExistKey(string valueKey)
        {
            var slotKey = USave.CurrentSlotKey;
            return ExistKey(slotKey, valueKey);
        }

        public abstract bool ExistKey(string slotKey, string valueKey);

        #endregion

        #region Delete

        public virtual bool DeleteKey(string valueKey)
        {
            var slotKey = USave.CurrentSlotKey;
            return DeleteKey(slotKey, valueKey);
        }

        public virtual void DeleteAll()
        {
            var slotKey = USave.CurrentSlotKey;
            DeleteAll(slotKey);
        }

        public abstract bool DeleteKey(string slotKey, string valueKey);

        public virtual void DeleteAll(string slotKey)
        {
            var keyList = GetAllKeys(slotKey);
            foreach (var valueKey in keyList)
            {
                DeleteKey(slotKey, valueKey);
            }
        }

        #endregion

        #region Load / Save

        public virtual void Load()
        {
            var slotKey = USave.CurrentSlotKey;
            Load(slotKey);
        }

        public abstract void Load(string slotKey);

        public virtual void Save()
        {
            var slotKey = USave.CurrentSlotKey;
            Save(slotKey);
        }

        public abstract void Save(string slotKey);

        #endregion

        #region Value Type Check

        public static readonly HashSet<Type> ValueTypeDefine = new HashSet<Type>()
        {
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(string),
            typeof(byte),
            typeof(Enum)
        };

        public bool IsValueType(Type type)
        {
            return ValueTypeDefine.Contains(type);
        }

        #endregion
    }
}