using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class RectIntValue : TweenValue<RectInt>
    {
        public override RectInt Random(RectInt from, RectInt to)
        {
            return RandomUtil.RandomRectInt(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class RectIntValue : TweenValue<RectInt>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        { 
            GUIUtil.DrawRectIntProperty(property, name,
                AxisXName, AxisYName, AxisZName, AxisWName,
                AxisX, AxisY, AxisZ, AxisW);
        }
    }
#endif
}