using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class IntegerValue : TweenValue<int>
    {
        public override int Random(int from, int to)
        {
            return RandomUtil.RandomInt(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class IntegerValue : TweenValue<int>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawProperty(property, name);
        }
    }
#endif
}