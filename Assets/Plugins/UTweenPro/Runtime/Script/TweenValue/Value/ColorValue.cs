using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class ColorValue : TweenValue<Color>
    {
        public override Color Random(Color from, Color to)
        {
            return RandomUtil.RandomColor(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class ColorValue : TweenValue<Color>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawProperty(property, name);
        }
    }
#endif
}