#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Aya.TweenPro
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TweenerPropertyAttribute : Attribute
    {
        public static Dictionary<Type, List<TweenerPropertyAttributeCacheData>> TypeFieldInfoDic { get; set; } = new Dictionary<Type, List<TweenerPropertyAttributeCacheData>>();

        public string PropertyName { get; set; }
        public bool HasSubProperty { get; set; }

        public TweenerPropertyAttribute()
        {
            PropertyName = null;
            HasSubProperty = false;
        }

        public TweenerPropertyAttribute(bool hasSubProperty)
        {
            PropertyName = null;
            HasSubProperty = hasSubProperty;
        }

        public TweenerPropertyAttribute(string propertyName, bool hasSubProperty)
        {
            PropertyName = propertyName;
            HasSubProperty = hasSubProperty;
        }

        public static void CacheProperty(object target, SerializedObject targetObject)
        {
            CacheProperty(target, targetObject.FindProperty);
        }

        public static void CacheProperty(object target, SerializedProperty targetProperty)
        {
            CacheProperty(target, targetProperty.FindPropertyRelative);
        }

        public static List<TweenerPropertyAttributeCacheData> GetFiledInfoList(Type type)
        {
            if (!TypeFieldInfoDic.TryGetValue(type, out var fieldInfoList))
            {
                fieldInfoList = new List<TweenerPropertyAttributeCacheData>();
                var bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                var fieldInfos = type.GetFields(bindingFlags);
                foreach (var fieldInfo in fieldInfos)
                {
                    var tweenerPropertyAttribute = fieldInfo.GetCustomAttribute(typeof(TweenerPropertyAttribute)) as TweenerPropertyAttribute;
                    if (tweenerPropertyAttribute == null) continue;
                    var propertyName = tweenerPropertyAttribute.PropertyName;
                    if (string.IsNullOrEmpty(propertyName))
                    {
                        var suffix = "Property";
                        if (fieldInfo.Name.EndsWith(suffix))
                        {
                            propertyName = fieldInfo.Name.Substring(0, fieldInfo.Name.Length - suffix.Length);
                        }
                        else
                        {
                            propertyName = fieldInfo.Name;
                        }
                    }

                    var data = new TweenerPropertyAttributeCacheData()
                    {
                        FieldInfo = fieldInfo,
                        TweenerPropertyAttribute = tweenerPropertyAttribute,
                        PropertyName = propertyName,
                    };

                    if (tweenerPropertyAttribute.HasSubProperty)
                    {
                        var subFieldInfo = type.GetField(propertyName, bindingFlags);
                        data.SubFieldInfo = subFieldInfo;
                    }

                    fieldInfoList.Add(data);
                }

                TypeFieldInfoDic.Add(type, fieldInfoList);
            }

            return fieldInfoList;
        }


        public static void CacheProperty(object target, Func<string, SerializedProperty> propertyGetter)
        {
            var targetType = target.GetType();
            var filedInfoList = GetFiledInfoList(targetType);
            foreach (var data in filedInfoList)
            {
                var fieldInfo = data.FieldInfo;
                var propertyName = data.PropertyName;
                var property = propertyGetter(propertyName);
                fieldInfo.SetValue(target, property);

                if (data.SubFieldInfo != null)
                {
                    var subFieldInfo = data.SubFieldInfo;
                    var subTarget = subFieldInfo.GetValue(target);
                    if (subTarget is ITweenerSubData subData)
                    {
                        subData.DataProperty = property;
                    }

                    CacheProperty(subTarget, property);
                }
            }
        }
    }
}
#endif