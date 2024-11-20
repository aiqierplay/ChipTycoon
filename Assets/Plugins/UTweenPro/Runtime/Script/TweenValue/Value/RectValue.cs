using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class RectValue : TweenValue<Rect>
    {
        public override Rect Random(Rect from, Rect to)
        {
            return RandomUtil.RandomRect(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class RectValue : TweenValue<Rect>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawRectProperty(property, name,
                AxisXName, AxisYName, AxisZName, AxisWName,
                AxisX, AxisY, AxisZ, AxisW);
        }
    }
#endif
}