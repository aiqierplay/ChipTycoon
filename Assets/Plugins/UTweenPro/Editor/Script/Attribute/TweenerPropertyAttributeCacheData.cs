#if UNITY_EDITOR
using System;
using System.Reflection;

namespace Aya.TweenPro
{
    [Serializable]
    public class TweenerPropertyAttributeCacheData
    {
        public FieldInfo FieldInfo;
        public TweenerPropertyAttribute TweenerPropertyAttribute;
        public string PropertyName;
        public FieldInfo SubFieldInfo;
    }
}
#endif