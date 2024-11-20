using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TransformValue : TweenValue<Transform>
    {
        public override bool SupportRandom => false;

        public override Transform Random(Transform from, Transform to)
        {
            return default;
        }
    }

#if UNITY_EDITOR
    public partial class TransformValue : TweenValue<Transform>
    {
        public override void DrawValueProperty(SerializedProperty property, string name)
        {
            using (GUIErrorColorArea.Create(property.objectReferenceValue == null))
            {
                GUIUtil.DrawProperty(property, name);
            }
        }
    }
#endif
}