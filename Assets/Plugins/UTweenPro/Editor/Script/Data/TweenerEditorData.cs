#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    [Serializable]
    public class TweenerEditorData
    {
        public Type Type;
        public Type TargetType;
        public TweenerAttribute Info;
        public TweenerDocumentationAttribute Documentation;

        public TweenGroupEditorData GroupData;

        public void Cache(Type tweenerType)
        {
            Type = tweenerType;
            var targetField = tweenerType.GetField("Target");
            if (targetField != null)
            {
                TargetType = targetField.FieldType;
                if (TargetType == typeof(Object) || TargetType == typeof(Component) || TargetType == typeof(GameObject) || TargetType == typeof(Behaviour))
                {
                    TargetType = null;
                }
            }
            else
            {
                TargetType = null;
            }

            if (Info == null)
            {
                Info = Type.GetCustomAttribute<TweenerAttribute>();
            }

            if (Documentation == null)
            {
                Documentation = Type.GetCustomAttribute<TweenerDocumentationAttribute>();
            }

            if (GroupData == null)
            {
                GroupData = UTweenEditorSetting.Ins.GetGroupData(Info.Group);
            }
        }
    }
}
#endif