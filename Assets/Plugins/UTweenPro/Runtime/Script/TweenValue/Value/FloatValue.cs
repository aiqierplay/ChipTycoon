using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class FloatValue : TweenValue<float>
    {
        public override float Random(float from, float to)
        {
            return RandomUtil.RandomFloat(from, to);
        }
    }

#if UNITY_EDITOR
    public partial class FloatValue : TweenValue<float>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawProperty(property, name);
        }
    }
#endif
}