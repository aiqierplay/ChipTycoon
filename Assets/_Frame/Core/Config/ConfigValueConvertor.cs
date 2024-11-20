using System;
using Aya.Extension;
using UnityEngine;

public static class ConfigValueConvertor
{
    public static object Convert(string value, Type type)
    {
        if (type == null || string.IsNullOrEmpty(value))
        {
            return default;
        }

        if (type == typeof(int))
        {
            return value.AsInt();
        }
        else if (type == typeof(long))
        {
            return value.AsLong();
        }
        else if (type == typeof(float))
        {
            return value.AsFloat();
        }
        else if (type == typeof(string))
        {
            return value.AsString();
        }
        else if (type == typeof(double))
        {
            return value.AsDouble();
        }
        else if (type == typeof(decimal))
        {
            return value.AsDecimal();
        }
        else if (type == typeof(bool))
        {
            return value.ToLower() == "true" ? true : false;
        }
        else if (type.IsEnum)
        {
            return Enum.ToObject(type, value.AsInt());
        }
        else if (type == typeof(Color))
        {
            ColorUtility.TryParseHtmlString(value, out var color);
            return color;
        }
        else if (type == typeof(Vector3))
        {
            var xyz = value.Replace("(", "").Replace(")", "").Split(new[] { ',', ' ' });
            var x = xyz[0].CastType<float>();
            var y = xyz[1].CastType<float>();
            var z = xyz[2].CastType<float>();
            var vector3 = new Vector3(x, y, z);
            return vector3;
        }
        else if (type.IsSubclassOf(typeof(UnityEngine.Object)))
        {
            var prefab = Resources.Load(value, type);
            return prefab;
        }

        return default;
    }
}
