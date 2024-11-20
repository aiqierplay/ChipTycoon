using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Aya.Simplify
{
    public static class Modify
    {
        public static void ModifyFieldValue<TValue>(ref TValue field, TValue newValue)
        {
            field = newValue;
        }

        public static void ModifyPropertyValue<TClass, TProperty, T>(Expression<Func<TClass, TProperty>> expression, TClass target, TProperty newValue)
        {
            var body = (MemberExpression)expression.Body;
            var property = (PropertyInfo)body.Member;
            var setMethod = property.GetSetMethod();
            setMethod.Invoke(target, new object[] { newValue });
        }
    }
}