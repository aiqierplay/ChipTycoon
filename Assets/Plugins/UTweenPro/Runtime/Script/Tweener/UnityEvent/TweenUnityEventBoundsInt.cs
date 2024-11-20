using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Unity Event BoundsInt", "Unity Event", "BuildSettings.Editor")]
    [Serializable]
    public partial class TweenUnityEventBoundsInt : TweenValueBoundsInt<Object>
    {
        public override bool SupportTarget => false;

        public OnValueBoundsIntEvent Event = new OnValueBoundsIntEvent();

        public override BoundsInt Value
        {
            get => _value;
            set
            {
                _value = value;
                Event.Invoke(_value);
            }
        }

        private BoundsInt _value;

        public override void Reset()
        {
            base.Reset();
            Event.RemoveAllListeners();
        }
    }

#if UNITY_EDITOR

    public partial class TweenUnityEventBoundsInt : TweenValueBoundsInt<Object>
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