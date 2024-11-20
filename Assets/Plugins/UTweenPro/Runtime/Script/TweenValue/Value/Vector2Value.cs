using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class Vector2Value : TweenValue<Vector2>
    {
        public override Vector2 Random(Vector2 from, Vector2 to)
        {
            return RandomUtil.RandomVector2(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class Vector2Value : TweenValue<Vector2>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawVector2Property(property, name,
                AxisXName, AxisYName,
                AxisX, AxisY);
        }
    }
#endif
}