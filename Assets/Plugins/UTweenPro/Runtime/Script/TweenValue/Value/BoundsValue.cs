using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class BoundsValue : TweenValue<Bounds>
    {
        public override Bounds Random(Bounds from, Bounds to)
        {
            return RandomUtil.RandomBounds(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class BoundsValue : TweenValue<Bounds>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawBoundsProperty(property, name, AxisX, AxisY);
        }
    }
#endif
}