using System;
using System.Collections.Generic;
using System.Reflection;
using Aya.Extension;

[Serializable]
public abstract class SetValueEntityPropertyAttribute : EntityPropertyAttribute
{
    public override void Process(EntityBase entity, PropertyInfo propertyInfo)
    {
        Process(entity, propertyInfo);
    }

    public override void Process(EntityBase entity, FieldInfo fieldInfo)
    {
        Process(entity, fieldInfo);
    }

    public virtual void Process(EntityBase entity, MemberInfo memberInfo)
    {
        var type = memberInfo.GetMemberType();
        if (type.IsArray)
        {
            var elementType = type.GetElementType();
            SetValue(entity, true, elementType, value =>
            {
                var arrayValue = CastMethod.MakeGenericMethod(elementType).Invoke(null, new[] {value});
                memberInfo.SetValue(entity, arrayValue);
            });
        }
        else if (type.IsGenericList())
        {
            var elementType = type.GetGenericArguments()[0];
            SetValue(entity, true, elementType, value =>
            {
                var listType = typeof(List<>).MakeGenericType(elementType);
                var list = Activator.CreateInstance(listType);
                var method = listType.GetMethod("AddRange");
                var arrayValue = CastMethod.MakeGenericMethod(elementType).Invoke(null, new[] { value });
                method?.Invoke(list, new[] { arrayValue });
                memberInfo.SetValue(entity, list);
            });
        }
        else
        {
            SetValue(entity, false, type,  value => { memberInfo.SetValue(entity, value); });
        }
    }

    public abstract void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction);


    #region Cast Array

    public static MethodInfo CastMethod => typeof(SetValueEntityPropertyAttribute).GetMethod(nameof(CastArrayToType));

    public static T[] CastArrayToType<T>(object arrayValue)
    {
        return Array.ConvertAll((object[])arrayValue, prop => (T)prop);
    } 

    #endregion
}