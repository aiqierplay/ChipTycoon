using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Unity Event Bounds", "Unity Event", "BuildSettings.Editor")]
    [Serializable]
    public partial class TweenUnityEventBounds : TweenValueBounds<Object>
    {
        public override bool SupportTarget => false;

        public OnValueBoundsEvent Event = new OnValueBoundsEvent();

        public override Bounds Value
        {
            get => _value;
            set
            {
                _value = value;
                Event.Invoke(_value);
            }
        }

        private Bounds _value;

        public override void Reset()
        {
            base.Reset();
            Event.RemoveAllListeners();
        }
    }

#if UNITY_EDITOR

    public partial class TweenUnityEventBounds : TweenValueBounds<Object>
    {
        [TweenerProperty, NonSerialized] public SerializedProperty EventProperty;

        public override void DrawFromToValue()
        {
            EditorGUILayout.PropertyField(EventProperty);
            base.DrawFromToValue();
        }
    }

#endif
}