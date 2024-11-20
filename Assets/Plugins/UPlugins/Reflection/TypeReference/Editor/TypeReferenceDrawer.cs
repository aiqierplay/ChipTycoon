#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Aya.Extension;
using UnityEditor;

namespace Aya.Reflection
{
    [CustomPropertyDrawer(typeof(TypeReference))]
    public class TypeReferenceDrawer : SearchableDropdownDrawer<TypeReferenceAttribute, Type>
    {
        public override string NotFoundTip => "No [Type Reference Attribute declared!";

        public SerializedProperty AssemblyProperty;
        public SerializedProperty TypeProperty;

        public override void CacheProperty(SerializedProperty property)
        {
            AssemblyProperty = property.FindPropertyRelative("AssemblyName");
            TypeProperty = property.FindPropertyRelative("TypeName");
        }

        public override Type GetValue()
        {
            var assembly = TypeCaches.GetAssemblyByName(AssemblyProperty.stringValue);
            if (assembly == null) return default;
            var type = assembly.GetType(TypeProperty.stringValue);
            return type;
        }

        public override void SetValue(Type value)
        {
            if (value != null)
            {
                AssemblyProperty.stringValue = value.Assembly.FullName;
                TypeProperty.stringValue = value.Name;
            }
            else
            {
                AssemblyProperty.stringValue = null;
                TypeProperty.stringValue = null;
            }
        }

        public override string GetDisplayName(Type value)
        {
            return value.Name;
        }


        public override string GetRootName()
        {
            return Attribute.Type.Name;
        }

        public override IEnumerable<Type> GetValueCollection()
        {
            var typeCollection = TypeCache.GetTypesDerivedFrom(Attribute.Type).FindAll(type =>
            {
                if (type.IsAbstract) return false;
                if (type.IsInterface) return false;
                if (type.IsGenericType) return false;
                if (type.IsEnum) return false;
                return true;
            });

            return typeCollection;
        }
    }
}
#endif