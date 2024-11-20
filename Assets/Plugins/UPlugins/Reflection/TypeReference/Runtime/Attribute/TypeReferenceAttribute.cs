using System;
using UnityEngine;

namespace Aya.Reflection
{
    public class TypeReferenceAttribute : SearchableDropdownAttribute
    {
        public Type Type;

        public TypeReferenceAttribute(Type type)
        {
            Type = type;
        }
    }
}
