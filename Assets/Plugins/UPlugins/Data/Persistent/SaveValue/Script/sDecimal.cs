/////////////////////////////////////////////////////////////////////////////
//
//  Script   : sDecimal.cs
//  Info     : 可存取数据类型 Float
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using Aya.Security;

namespace Aya.Data.Persistent
{
    [Serializable]
    public class sDecimal : SaveValue<decimal>, IConvertible
    {
        public sDecimal(string key, decimal defaultValue = 0) : base(key, defaultValue)
        {
        }

        #region Override operator
        public static decimal operator +(sDecimal lhs, decimal rhs)
        {
            return lhs.Value + rhs;
        }

        public static decimal operator -(sDecimal lhs, decimal rhs)
        {
            return lhs.Value - rhs;
        }

        public static decimal operator *(sDecimal lhs, decimal rhs)
        {
            return lhs.Value * rhs;
        }

        public static decimal operator /(sDecimal lhs, decimal rhs)
        {
            return lhs.Value / rhs;
        }

        public static sDecimal operator ++(sDecimal lhs)
        {
            lhs.Value++;
            return lhs;
        }

        public static sDecimal operator --(sDecimal lhs)
        {
            lhs.Value--;
            return lhs;
        }

        public static bool operator ==(sDecimal lhs, sDecimal rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(sDecimal lhs, sDecimal rhs)
        {
            return lhs.Value != rhs.Value;
        }


        public static implicit operator decimal(sDecimal obj)
        {
            return obj.Value;
        }

        public static implicit operator cDecimal(sDecimal obj)
        {
            cDecimal ret = obj.Value;
            return ret;
        }

        #endregion

        #region Override object

        public bool Equals(decimal obj)
        {
            return Value == obj;
        }

        public bool Equals(cDecimal obj)
        {
            return Value == obj;
        }

        public override bool Equals(object obj)
        {
            return this == (sDecimal)obj;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion

        #region IConvertible

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(Value);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(Value);
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(Value);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(Value);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(Value);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(Value);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(Value);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(Value);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(Value);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(Value);
        }

        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(Value);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(Value);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(Value);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(Value);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Value, conversionType);
        }

        #endregion
    }
}