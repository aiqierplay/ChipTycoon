using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Unity Event Vector2Int", "Unity Event", "BuildSettings.Editor")]
    [Serializable]
    public partial class TweenUnityEventVector2Int : TweenValueVector2Int<Object>
    {
        public override bool SupportTarget => false;

        public OnValueVector2IntEvent Event = new OnValueVector2IntEvent();

        public override Vector2Int Value
        {
            get => _value;
            set
            {
                _value = value;
                Event.Invoke(_value);
            }
        }

        private Vector2Int _value;

        public override void Reset()
        {
            base.Reset();
            Event.RemoveAllListeners();
        }
    }

#if UNITY_EDITOR

    public partial class TweenUnityEventVector2Int : TweenValueVector2Int<Object>
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