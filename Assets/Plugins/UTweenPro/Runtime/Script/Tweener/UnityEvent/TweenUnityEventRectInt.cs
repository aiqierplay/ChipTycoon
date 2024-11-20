using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Unity Event RectInt", "Unity Event", "BuildSettings.Editor")]
    [Serializable]
    public partial class TweenUnityEventRectInt : TweenValueRectInt<Object>
    {
        public override bool SupportTarget => false;

        public OnValueRectIntEvent Event = new OnValueRectIntEvent();

        public override RectInt Value
        {
            get => _value;
            set
            {
                _value = value;
                Event.Invoke(_value);
            }
        }

        private RectInt _value;

        public override void Reset()
        {
            base.Reset();
            Event.RemoveAllListeners();
        }
    }

#if UNITY_EDITOR

    public partial class TweenUnityEventRectInt : TweenValueRectInt<Object>
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