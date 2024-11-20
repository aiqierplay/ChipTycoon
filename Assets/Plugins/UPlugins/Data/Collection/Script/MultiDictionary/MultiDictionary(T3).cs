/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MultiDictionary.fcs
//  Info     : 复合字典，使用两个Key对应一个Value
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2024
//
/////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

namespace Aya.Data
{
    public class MultiDictionary<TKey1, TKey2, TKey3, TValue> : Dictionary<CompositeKey<TKey1, TKey2, TKey3>, TValue>
    {
        public bool CheckKeySequence { get; protected set; } = false;

        public MultiDictionary(bool checkKeySequence = false)
        {
            CheckKeySequence = checkKeySequence;
        }

        public TValue this[TKey1 key1, TKey2 key2, TKey3 key3]
        {
            get
            {
                var key = new CompositeKey<TKey1, TKey2, TKey3>(key1, key2, key3);
                var result = this[key];
                return result;
            }
            set
            {
                var key = new CompositeKey<TKey1, TKey2, TKey3>(key1, key2, key3);
                this[key] = value;
            }
        }

        public void Add(TKey1 key1, TKey2 key2, TKey3 key3, TValue value)
        {
            var key = new CompositeKey<TKey1, TKey2,  TKey3>(key1, key2, key3);
            Add(key, value);
        }

        public void Remove(TKey1 key1, TKey2 key2, TKey3 key3)
        {
            var key = new CompositeKey<TKey1, TKey2, TKey3>(key1, key2, key3);
            Remove(key);
        }

        public bool ContainsKey(TKey1 key1, TKey2 key2, TKey3 key3)
        {
            var key = new CompositeKey<TKey1, TKey2, TKey3>(key1, key2, key3);
            var result = ContainsKey(key);
            return result;
        }

        public bool TryGetValue(TKey1 key1, TKey2 key2, TKey3 key3, out TValue value)
        {
            var key = new CompositeKey<TKey1, TKey2, TKey3>(key1, key2, key3);
            var result = TryGetValue(key, out value);
            return result;
        }
    }
}