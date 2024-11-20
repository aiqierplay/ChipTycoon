using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class DoubleValue : TweenValue<double>
    {
        public override double Random(double from, double to)
        {
            return RandomUtil.RandomDouble(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class DoubleValue : TweenValue<double>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawProperty(property, name);
        }
    }
#endif
}