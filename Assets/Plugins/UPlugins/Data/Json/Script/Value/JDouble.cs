﻿using System;

namespace Aya.Data.Json
{
    public class JDouble : JValue
    {
        private readonly double _value;

        public JDouble(double value) : base(JType.Number)
        {
            this._value = value;
        }

        public override object Value => _value;

        public override void Write(JsonWriter writer)
        {
            writer.Write(_value);
        }

        public override bool Equals(object obj)
        {
            var other = obj as JDouble;
            if (other == null)
            {
                return false;
            }
            return Math.Abs(_value - other._value) < 1e-6;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        #region As

        public override int AsInt()
        {
            return (int) _value;
        }

        public override long AsLong()
        {
            return (long) _value;
        }

        public override float AsFloat()
        {
            return (float) _value;
        }

        public override double AsDouble()
        {
            return (double) _value;
        }

        #endregion

        #region Get

        public override int? GetInt()
        {
            return (int) _value;
        }

        public override long? GetLong()
        {
            return (long) _value;
        }

        public override float? GetFloat()
        {
            return (float) _value;
        }

        public override double? GetDouble()
        {
            return (double) _value;
        }

        #endregion

        #region Opt

        public override int OptInt(int defaultValue = default(int))
        {
            return (int) _value;
        }

        public override long OptLong(long defaultValue = default(long))
        {
            return (long) _value;
        }

        public override float OptFloat(float defaultValue = default(float))
        {
            return (float) _value;
        }

        public override double OptDouble(double defaultValue = default(double))
        {
            return (double) _value;
        }

        public override string OptString(string defaultValue)
        {
            return ToString();
        }

        #endregion
    }
}