using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class LongValue : TweenValue<long>
    {
        public override long Random(long from, long to)
        {
            return RandomUtil.RandomLong(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class LongValue : TweenValue<long>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawProperty(property, name);
        }
    }
#endif
}