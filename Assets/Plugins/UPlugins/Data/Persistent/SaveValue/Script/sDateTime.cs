/////////////////////////////////////////////////////////////////////////////
//
//  Script   : sLong.cs
//  Info     : 可存取数据类型 Long
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Globalization;

namespace Aya.Data.Persistent
{
    [Serializable]
    public class sDateTime : SaveValue<long>, IConvertible
    {
        public DateTime DateTimeValue
        {
            get => TimeStamp2DateTime(Value);
            set => Value = DateTime2TimeStamp13(value);
        }

        public sDateTime(string key, DateTime defaultValue = default) : base(key, DateTime2TimeStamp13(defaultValue))
        {
        }

        public static long DateTime2TimeStamp13(DateTime dt, bool isUtc = false)
        {
            var start = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var end = isUtc ? dt.ToUniversalTime() : dt;
            var timestamp = (long)(end - start).TotalMilliseconds;
            return timestamp;
        }

        public static DateTime TimeStamp2DateTime(long timeStamp)
        {
            var start = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime ret;
            if (timeStamp <= 9999999999)
            {
                ret = start.AddSeconds(timeStamp);
            }
            else
            {
                ret = start.AddMilliseconds(timeStamp);
            }
            return ret;
        }

        #region Override operator

        public static bool operator ==(sDateTime lhs, sDateTime rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(sDateTime lhs, sDateTime rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static implicit operator DateTime(sDateTime obj)
        {
            return TimeStamp2DateTime(obj.Value);
        }

        #endregion

        #region Override object

        public bool Equals(DateTime obj)
        {
            return Value == DateTime2TimeStamp13(obj);
        }

        public bool Equals(sDateTime obj)
        {
            return Value == obj.Value;
        }

        public override bool Equals(object obj)
        {
            return this == (sDateTime)obj;
        }

        public override string ToString()
        {
            return DateTimeValue.ToString(CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return DateTimeValue.GetHashCode();
        }

        #endregion

        #region IConvertible

        public TypeCode GetTypeCode()
        {
            return TypeCode.DateTime;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(DateTimeValue);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(DateTimeValue);
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(DateTimeValue);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(DateTimeValue);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(DateTimeValue);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(DateTimeValue);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(DateTimeValue);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(DateTimeValue);
        }
        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(DateTimeValue);
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(DateTimeValue);
        }

        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(DateTimeValue, CultureInfo.InvariantCulture);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(DateTimeValue);
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(DateTimeValue);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(DateTimeValue);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(DateTimeValue, conversionType);
        }

        #endregion
    }
}