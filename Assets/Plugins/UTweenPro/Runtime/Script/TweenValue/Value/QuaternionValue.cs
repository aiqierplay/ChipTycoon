using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class QuaternionValue : TweenValue<Quaternion>
    {
        public override Quaternion Random(Quaternion from, Quaternion to)
        {
            return RandomUtil.RandomQuaternion(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class QuaternionValue : TweenValue<Quaternion>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {        
            GUIUtil.DrawQuaternionProperty(property, name,
                AxisXName, AxisYName, AxisZName, AxisWName,
                AxisX, AxisY, AxisZ, AxisW);
        }
    }
#endif
}