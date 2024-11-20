using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Save any data in any object
public static class ObjectParam
{
    public static Dictionary<object, Dictionary<object, object>> Dictionary = new Dictionary<object, Dictionary<object, object>>();

    public static void SetParam(object target, object key, object value)
    {
        var dic = GetObjectDic(target);
        if (dic.ContainsKey(key)) dic[key] = value;
        else dic.Add(key, value);
    }

    public static T GetParam<T>(object target, object key, T defaultValue = default)
    {
        var dic = GetObjectDic(target);
        if (dic.TryGetValue(key, out var value))
        {
            if (value == null) return defaultValue;
            return (T) value;
        }

        return defaultValue;
    }

    public static T GetAndRemoveParam<T>(object target, object key, T defaultValue = default)
    {
        var result = GetParam(target, key, defaultValue);
        RemoveParam(target, key);
        return result;
    }

    public static void RemoveParam(object target, object key)
    {
        var dic = GetObjectDic(target);
        if (dic.ContainsKey(key)) dic.Remove(key);
    }

    public static void RemoveParam(object target)
    {
        if (Dictionary.ContainsKey(target))
        {
            Dictionary.Remove(target);
        }
    }

    private static Dictionary<object, object> GetObjectDic(object target)
    {
        if (!Dictionary.TryGetValue(target, out var dic))
        {
            dic = new Dictionary<object, object>();
            Dictionary.Add(target, dic);
        }

        return dic;
    }
}

public static class ObjectParamExtension
{
    public static void SetParam(this object target, object key, object value)
    {
        ObjectParam.SetParam(target, key, value);
    }

    public static T GetParam<T>(this object target, object key, T defaultValue = default)
    {
        return ObjectParam.GetParam<T>(target, key, defaultValue);
    }

    public static T GetAndRemoveParam<T>(this object target, object key, T defaultValue = default)
    {
        return ObjectParam.GetAndRemoveParam(target, key, defaultValue);
    }

    public static void RemoveParam(this object target, object key)
    {
        ObjectParam.RemoveParam(target, key);
    }

    public static void RemoveParam(this object target)
    {
        ObjectParam.RemoveParam(target);
    }
}