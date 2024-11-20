using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class Vector3IntValue : TweenValue<Vector3Int>
    {
        public override Vector3Int Random(Vector3Int from, Vector3Int to)
        {
            return RandomUtil.RandomVector3Int(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class Vector3IntValue : TweenValue<Vector3Int>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawVector3IntProperty(property, name,
            AxisXName, AxisYName, AxisZName,
            AxisX, AxisY, AxisZ);
        }
    }
#endif
}