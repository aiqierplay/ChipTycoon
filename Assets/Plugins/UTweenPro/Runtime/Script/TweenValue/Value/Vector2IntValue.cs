using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class Vector2IntValue : TweenValue<Vector2Int>
    {
        public override Vector2Int Random(Vector2Int from, Vector2Int to)
        {
            return RandomUtil.RandomVector2Int(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class Vector2IntValue : TweenValue<Vector2Int>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawVector2IntProperty(property, name,
                AxisXName, AxisYName, 
                AxisX, AxisY);
        }
    }
#endif
}