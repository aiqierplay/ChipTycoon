#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Aya.Reflection
{
    public abstract class SearchableDropdownDrawer<TAttribute, TValue> : SearchableDropdownDrawer<TValue>
        where TAttribute : SearchableDropdownAttribute
    {
        public bool Enable;
        public TAttribute Attribute;

        public virtual string NotFoundTip => "Not found!";
      
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Property = property;
            if (Attribute == null)
            {
                Attribute = GetAttribute<TAttribute>(property, true);
                Enable = Attribute != null;
                CacheProperty(property);
            }

            if (Attribute != null)
            {
                var labelWidth = EditorGUIUtility.labelWidth;
                var labelRect = position;
                labelRect.width = labelWidth;

                var filedRect = position;
                filedRect.x += labelWidth;
                filedRect.width -= labelWidth;

                ShowDropdownList(label.text, labelRect, filedRect);
            }
            else
            {
                var originalColor = GUI.color;
                GUI.color = Color.red;
                EditorGUI.LabelField(position, NotFoundTip, EditorStyles.helpBox);
                GUI.color = originalColor;
            }
        }
        protected virtual T GetAttribute<T>(SerializedProperty serializedProperty, bool inherit) where T : Attribute
        {
            if (serializedProperty == null)
            {
                return null;
            }

            var type = serializedProperty.serializedObject.targetObject.GetType();
            FieldInfo field = null;
            PropertyInfo property = null;
            foreach (var name in serializedProperty.propertyPath.Split('.'))
            {
                field = type.GetField(name);
                if (field == null)
                {
                    property = type.GetProperty(name);
                    if (property == null)
                    {
                        return null;
                    }

                    type = property.PropertyType;
                }
                else
                {
                    type = field.FieldType;
                }
            }

            T[] attributes;

            if (field != null)
            {
                attributes = field.GetCustomAttributes(typeof(T), inherit) as T[];
            }
            else if (property != null)
            {
                attributes = property.GetCustomAttributes(typeof(T), inherit) as T[];
            }
            else
            {
                return null;
            }

            return attributes != null && attributes.Length > 0 ? attributes[0] : null;
        }
    }
}
#endif