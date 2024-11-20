/////////////////////////////////////////////////////////////////////////////
//
//  Script   : CompositeKey.fcs
//  Info     : 复合键，不可选区分键顺序
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2024
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Data
{
    public readonly struct CompositeKey<TKey1, TKey2, TKey3> : IEquatable<CompositeKey<TKey1, TKey2, TKey3>>
    {
        public readonly TKey1 Key1;
        public readonly TKey2 Key2;
        public readonly TKey3 Key3;

        public CompositeKey(TKey1 key1, TKey2 key2, TKey3 key3)
        {
            Key1 = key1;
            Key2 = key2;
            Key3 = key3;
        }

        public bool Equals(CompositeKey<TKey1, TKey2, TKey3> other)
        {
            var result = Key1.Equals(other.Key1) && Key2.Equals(other.Key2) && Key3.Equals(other.Key3);
            return result;
        }

        public override bool Equals(object obj)
        {
            var result = obj != null && Equals((CompositeKey<TKey1, TKey2, TKey3>)obj);
            return result;
        }

        public override int GetHashCode()
        {
            var hashCode1 = Key1 == null ? 0 : Key1.GetHashCode();
            var hashCode2 = Key2 == null ? 0 : Key2.GetHashCode();
            var hashCode3 = Key3 == null ? 0 : Key3.GetHashCode();
            var initHash = 17;
            var result1 = (initHash << 5) - initHash + hashCode1;
            var result2 = (result1 << 5) - result1 + hashCode2;
            var result3 = (result2 << 5) - result2 + hashCode3;
            return result3;
        }
    }
}