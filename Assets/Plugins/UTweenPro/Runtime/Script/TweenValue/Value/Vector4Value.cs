using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class Vector4Value : TweenValue<Vector4>
    {
        public override Vector4 Random(Vector4 from, Vector4 to)
        {
            return RandomUtil.RandomVector4(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class Vector4Value : TweenValue<Vector4>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawVector4Property(property, name,
                     AxisXName, AxisYName, AxisZName, AxisWName,
                     AxisX, AxisY, AxisZ, AxisW);
        }
    }
#endif
}