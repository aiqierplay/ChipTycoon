using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class Vector3Value : TweenValue<Vector3>
    {
        public override Vector3 Random(Vector3 from, Vector3 to)
        {
            return RandomUtil.RandomVector3(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class Vector3Value : TweenValue<Vector3>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawVector3Property(property, name,
                AxisXName, AxisYName, AxisZName,
                AxisX, AxisY, AxisZ);
        }
    }
#endif
}