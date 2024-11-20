using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class RectOffsetValue : TweenValue<RectOffset>
    {
        public override RectOffset Random(RectOffset from, RectOffset to)
        {
            return RandomUtil.RandomRectOffset(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class RectOffsetValue : TweenValue<RectOffset>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawRectOffsetProperty(property, name,
                AxisXName, AxisYName, AxisZName, AxisWName,
                AxisX, AxisY, AxisZ, AxisW);
        }
    }
#endif
}