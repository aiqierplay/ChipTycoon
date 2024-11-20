using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class BooleanValue : TweenValue<bool>
    {
        public override bool Random(bool from, bool to)
        {
            return RandomUtil.RandomBoolean();
        }
    }

#if UNITY_EDITOR
    public partial class BooleanValue : TweenValue<bool>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawProperty(property, name);
        }
    }
#endif
}