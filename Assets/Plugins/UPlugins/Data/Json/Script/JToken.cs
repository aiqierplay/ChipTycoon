﻿using System;
using System.Collections;

namespace Aya.Data.Json
{
	public abstract class JToken
	{
		public string Name { get; set; }
		public JType Type { get; protected set; }

		public static JToken Parse(string json)
		{
			var reader = new JsonReader(json);
			reader.Read();
			return ParseToken(reader);
		}

	    public static JToken OptParse(string json, JToken def = null)
	    {
	        try
	        {
	            return Parse(json);
	        }
	        catch (Exception)
	        {
	            return def;
	        }
	    }

	    public override string ToString()
		{
			var writer = new JsonWriter();
			Write(writer);
			return writer.ToString();
		}

		public string ToFormatString()
		{
		    var writer = new JsonWriter {PrettyPrint = true};
		    Write(writer);
			return writer.ToString();
		}

		protected JToken(JType type)
		{
			Type = type;
		}

		public bool Is(JType type)
		{
			return Type == type;
		}

	    public bool IsNone => Is(JType.None);
	    public bool IsNull => Is(JType.Null);
	    public bool IsValid => !IsNone && !IsNull; 
	    public bool IsValue => this is JValue;
	    public bool IsBool => Is(JType.Bool);
	    public bool IsNumber => Is(JType.Number);
	    public bool IsString => Is(JType.String);
	    public bool IsObject => Is(JType.Object);
	    public bool IsArray => Is(JType.Array);

	    public virtual JToken this[int index]
	    {
	        get
	        {
	            throw new JInvalidTypeException(this, "Get[int]");
	        }
	        set
	        {
	            throw new JInvalidTypeException(this, "Set[int]");
	        }
	    }

	    public virtual JToken this[string name]
	    {
	        get
	        {
	            throw new JInvalidTypeException(this, "Get[string]");
	        }
	        set
	        {
	            throw new JInvalidTypeException(this, "Set[string]");
	        }
	    }

	    public virtual JToken Get(string path)
	    {
	        return new JNone(path);
	    }

	    public virtual int Count
	    {
	        get
	        {
	            throw new JInvalidTypeException(this, "Count");
	        }
	    }

	    public virtual object AsValue()
	    {
	        throw new JInvalidTypeException(this, "AsValue");
	    }

	    public virtual bool AsBool()
	    {
	        throw new JInvalidTypeException(this, "AsBool");
	    }

	    public virtual int AsInt()
	    {
	        throw new JInvalidTypeException(this, "AsInt");
	    }

	    public virtual long AsLong()
	    {
	        throw new JInvalidTypeException(this, "AsLong");
	    }

	    public virtual float AsFloat()
	    {
	        throw new JInvalidTypeException(this, "AsFloat");
	    }

	    public virtual double AsDouble()
	    {
	        throw new JInvalidTypeException(this, "AsDouble");
	    }

	    public virtual string AsString()
	    {
	        throw new JInvalidTypeException(this, "AsString");
	    }

	    public virtual JCollection AsCollection()
	    {
	        throw new JInvalidTypeException(this, "AsCollection");
	    }

	    public virtual JArray AsArray()
	    {
	        throw new JInvalidTypeException(this, "AsArray");
	    }

	    public virtual JObject AsObject()
	    {
	        throw new JInvalidTypeException(this, "AsObject");
	    }

	    public T AsEnum<T>() where T : struct
	    {
	        return (T) Enum.ToObject(typeof(T), AsInt());
	    }

	    public virtual object GetValue()
	    {
	        return null;
	    }

	    public virtual bool? GetBool()
	    {
	        return null;
	    }

	    public virtual int? GetInt()
	    {
	        return null;
	    }

	    public virtual long? GetLong()
	    {
	        return null;
	    }

	    public virtual float? GetFloat()
	    {
	        return null;
	    }

	    public virtual double? GetDouble()
	    {
	        return null;
	    }

	    public virtual string GetString()
	    {
	        return null;
	    }

	    public virtual JCollection GetCollection()
	    {
	        return null;
	    }

	    public virtual JArray GetArray()
	    {
	        return null;
	    }

	    public virtual JObject GetObject()
	    {
	        return null;
	    }

	    public T? GetEnum<T>() where T : struct
	    {
	        var value = GetInt();
	        if (value != null)
	        {
	            return (T) Enum.ToObject(typeof(T), (int) value);
	        }
	        return null;
	    }

	    public virtual object OptValue(object def)
	    {
	        return def;
	    }

	    public virtual bool OptBool(bool defaultValue = default(bool))
	    {
	        return defaultValue;
	    }

	    public virtual int OptInt(int defaultValue = default(int))
	    {
	        return defaultValue;
	    }

	    public virtual long OptLong(long defaultValue = default(long))
	    {
	        return defaultValue;
	    }

	    public virtual float OptFloat(float defaultValue = default(float))
	    {
	        return defaultValue;
	    }

	    public virtual double OptDouble(double defaultValue = default(double))
	    {
	        return defaultValue;
	    }

	    public T OptEnum<T>(T def = default(T)) where T : struct
	    {
	        var value = OptInt(Convert.ToInt32(def));
	        return (T) Enum.ToObject(typeof(T), (int) value);
	    }

	    public virtual string OptString(string defaultValue)
	    {
	        return defaultValue;
	    }

	    public virtual JCollection OptCollection(JCollection def)
	    {
	        return def;
	    }

	    public virtual JArray OptArray(JArray def)
	    {
	        return def;
	    }

	    public virtual JObject OptObject(JObject def)
	    {
	        return def;
	    }

	    public string OptString()
	    {
	        var ret = OptString(null);
	        if (ret == null)
	        {
	            ret = "";
	        }
	        return ret;
	    }

	    public JCollection OptCollection()
	    {
	        var ret = OptCollection(null);
	        if (ret == null)
	        {
	            ret = new JArray();
	        }
	        return ret;
	    }

	    public JArray OptArray()
	    {
	        var ret = OptArray(null);
	        if (ret == null)
	        {
	            ret = new JArray();
	        }
	        return ret;
	    }

	    public JObject OptObject()
	    {
	        var ret = OptObject(null);
	        if (ret == null)
	        {
	            ret = new JObject();
	        }
	        return ret;
	    }

	    public static implicit operator JToken(bool value)
	    {
	        return new JBool(value);
	    }

	    public static implicit operator JToken(int value)
	    {
	        return new JInt(value);
	    }

	    public static implicit operator JToken(long value)
	    {
	        return new JLong(value);
	    }

	    public static implicit operator JToken(float value)
	    {
	        return new JFloat(value);
	    }

	    public static implicit operator JToken(double value)
	    {
	        return new JDouble(value);
	    }

	    public static implicit operator JToken(string value)
	    {
	        return value == null ? (JToken) new JNull() : (JToken) new JString((string) value);
	    }

	    public static implicit operator JToken(bool? value)
	    {
	        return value == null ? (JToken) new JNull() : (JToken) new JBool((bool) value);
	    }

	    public static implicit operator JToken(int? value)
	    {
	        return value == null ? (JToken) new JNull() : (JToken) new JInt((int) value);
	    }

	    public static implicit operator JToken(long? value)
	    {
	        return value == null ? (JToken) new JNull() : (JToken) new JLong((long) value);
	    }

	    public static implicit operator JToken(float? value)
	    {
	        return value == null ? (JToken) new JNull() : (JToken) new JFloat((float) value);
	    }

	    public static implicit operator JToken(double? value)
	    {
	        return value == null ? (JToken) new JNull() : (JToken) new JDouble((double) value);
	    }

        public static explicit operator bool(JToken value)
	    {
	        return value.AsBool();
	    }

	    public static explicit operator int(JToken value)
	    {
	        return value.AsInt();
	    }

	    public static explicit operator long(JToken value)
	    {
	        return value.AsLong();
	    }

	    public static explicit operator float(JToken value)
	    {
	        return value.AsFloat();
	    }

	    public static explicit operator double(JToken value)
	    {
	        return value.AsDouble();
	    }

	    public static explicit operator string(JToken value)
	    {
	        return value.AsString();
	    }

	    public static explicit operator bool?(JToken value)
	    {
	        return value.GetBool();
	    }

	    public static explicit operator int?(JToken value)
	    {
	        return value.GetInt();
	    }

	    public static explicit operator long?(JToken value)
	    {
	        return value.GetLong();
	    }

	    public static explicit operator float?(JToken value)
	    {
	        return value.GetFloat();
	    }

	    public static explicit operator double?(JToken value)
	    {
	        return value.GetDouble();
	    }

	    public static bool operator ==(JToken lhs, JToken rhs)
	    {
	        return Object.Equals(lhs, rhs);
	    }

	    public static bool operator !=(JToken lhs, JToken rhs)
	    {
	        return !Object.Equals(lhs, rhs);
	    }

	    public override bool Equals(object obj)
	    {
	        return base.Equals(obj);
	    }

	    public override int GetHashCode()
	    {
	        return base.GetHashCode();
	    }

	    public static JToken Create(object value)
	    {
	        if (value == null)
	        {
	            return new JNull();
	        }
	        if (value is JToken)
	        {
	            return value as JToken;
	        }
	        if (value is string)
	        {
	            return (string) value;
	        }
	        if (value is bool)
	        {
	            return (bool) value;
	        }
	        if (value is int)
	        {
	            return (int) value;
	        }
	        if (value is float)
	        {
	            return (float) value;
	        }
	        if (value is long)
	        {
	            return (long) value;
	        }
	        if (value is double)
	        {
	            return (double) value;
	        }
	        if (value is bool?)
	        {
	            return (bool?) value;
	        }
	        if (value is int?)
	        {
	            return (int?) value;
	        }
	        if (value is float?)
	        {
	            return (float?) value;
	        }
	        if (value is long?)
	        {
	            return (long?) value;
	        }
	        if (value is double?)
	        {
	            return (double?) value;
	        }
	        if (value is IEnumerable)
	        {
	            return new JArray(value as IEnumerable);
	        }
	        throw new JInvalidTypeException(value, typeof(JToken));
	    }

	    public abstract void Write(JsonWriter writer);

	    protected static JToken ParseToken(JsonReader reader)
	    {
	        if (reader.Token == JsonToken.Null)
	        {
	            return new JNull();
	        }
	        if (reader.Token == JsonToken.String)
	        {
	            return new JString((string) reader.Value);
	        }
	        if (reader.Token == JsonToken.Boolean)
	        {
	            return new JBool((bool) reader.Value);
	        }
	        if (reader.Token == JsonToken.Int)
	        {
	            return new JInt((int) reader.Value);
	        }
	        if (reader.Token == JsonToken.Float)
	        {
	            return new JFloat((float) reader.Value);
	        }
	        if (reader.Token == JsonToken.Long)
	        {
	            return new JLong((long) reader.Value);
	        }
	        if (reader.Token == JsonToken.Double)
	        {
	            return new JDouble((double) reader.Value);
	        }
	        if (reader.Token == JsonToken.ArrayStart)
	        {
	            var array = new JArray();
	            while (reader.Read())
	            {
	                if (reader.Token == JsonToken.ArrayEnd)
	                {
	                    break;
	                }
	                else
	                {
	                    var item = ParseToken(reader);
	                    if (item != null)
	                    {
	                        array.Add(item);
	                    }
	                    else
	                    {
	                        break;
	                    }
	                }
	            }
	            return array;
	        }
	        else if (reader.Token == JsonToken.ObjectStart)
	        {
	            var obj = new JObject();

	            while (reader.Read())
	            {
	                if (reader.Token == JsonToken.ObjectEnd)
	                {
	                    break;
	                }
	                else if (reader.Token == JsonToken.PropertyName)
	                {
	                    var name = (string) reader.Value;
	                    if (!reader.Read())
	                    {
	                        break;
	                    }
	                    var item = ParseToken(reader);
	                    if (item != null)
	                    {
	                        obj[name] = item;
	                    }
	                    else
	                    {
	                        break;
	                    }
	                }
	            }
	            return obj;
	        }
	        return null;
	    }
	}
}
