using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class BoundsIntValue : TweenValue<BoundsInt>
    {
        public override BoundsInt Random(BoundsInt from, BoundsInt to)
        {
            return RandomUtil.RandomBoundsInt(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class BoundsIntValue : TweenValue<BoundsInt>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawBoundsIntProperty(property, name, AxisX, AxisY);
        }
    }
#endif
}