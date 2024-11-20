﻿using System;
using System.Reflection;

namespace Aya.DataBinding
{
    public class RuntimePropertyBinder : RuntimePropertyBinder<object>
    {
        public override bool NeedUpdate => true;

        public override Type TargetType => Target.GetType();

        public RuntimePropertyBinder(string container, string key, DataDirection direction, object target, PropertyInfo propertyInfo, FieldInfo fieldInfo)
        {
            // var type = target.GetType();
            if (propertyInfo != null)
            {
                Property = propertyInfo.Name;
            }

            if (fieldInfo != null)
            {
                Property = fieldInfo.Name;
            }

            // var bindKey = type.Name + "." + Property + "." + key;
            Container = container;
            Key = key;
            Direction = direction;
            Target = target;
            PropertyInfo = propertyInfo;
            FiledInfo = fieldInfo;
        }
    }
}