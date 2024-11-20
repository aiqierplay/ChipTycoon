using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class StringValue : TweenValue<string>
    {
        public override bool SupportRandom => false;

        public override string Random(string from, string to)
        {
            return default;
        }
    }

#if UNITY_EDITOR
    public partial class StringValue : TweenValue<string>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            GUIUtil.DrawProperty(property, name);
        }
    }
#endif
}