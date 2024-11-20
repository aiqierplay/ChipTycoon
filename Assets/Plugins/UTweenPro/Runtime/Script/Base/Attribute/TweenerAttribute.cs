using System;

namespace Aya.TweenPro
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TweenerAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string Group { get; set; }
        public string IconName { get; }
        public int Order { get; }

        public TweenerAttribute(string displayName, string group, int order = 0)
        {
            DisplayName = displayName;
            Group = group;
            IconName = null;
            Order = order;
        }

        public TweenerAttribute(string displayName, string group, string iconName, int order = 0)
        {
            DisplayName = displayName;
            Group = group;
            IconName = iconName;
            Order = order;
        }
    }
}
