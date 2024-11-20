using System;

namespace Aya.TweenPro
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EaseFunctionAttribute : Attribute
    {
        public Type Type;
        public string DisplayName;
        public string Group;

        public EaseFunctionAttribute(Type type, string displayName, string group)
        {
            Type = type;
            DisplayName = displayName;
            Group = group;
        }
    }
}
