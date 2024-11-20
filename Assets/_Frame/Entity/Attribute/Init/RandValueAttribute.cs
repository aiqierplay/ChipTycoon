using System;
using Aya.Extension;
using Aya.Util;
using UnityEngine;

public class RandValueAttribute : SetValueEntityPropertyAttribute
{
    public object From;
    public object To;

    public RandValueAttribute(object from, object to)
    {
        From = from;
        To = to;
    }

    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        if (valueType == typeof(int))
        {
            var fromValue = From.AsInt();
            var toValue = To.AsInt();
            var value = RandUtil.RandInt(fromValue, toValue);
            setValueAction(value);
        }
        else if (valueType == typeof(float))
        {
            var fromValue = From.AsFloat();
            var toValue = To.AsFloat();
            var value = RandUtil.RandFloat(fromValue, toValue);
            setValueAction(value);
        }
        else if (valueType == typeof(Vector3))
        {
            var fromValue = From.CastType<Vector3>();
            var toValue = To.CastType<Vector3>();
            var value = RandUtil.RandVector3(fromValue, toValue);
            setValueAction(value);
        }
        else if (valueType == typeof(Color))
        {
            var fromValue = From.CastType<Color>();
            var toValue = To.CastType<Color>();
            var value = RandUtil.RandColor(fromValue, toValue);
            setValueAction(value);
        }
    }
}
