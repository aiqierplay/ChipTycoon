using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Unity Event Vector3Int", "Unity Event", "BuildSettings.Editor")]
    [Serializable]
    public partial class TweenUnityEventVector3Int : TweenValueVector3Int<Object>
    {
        public override bool SupportTarget => false;

        public OnValueVector3IntEvent Event = new OnValueVector3IntEvent();

        public override Vector3Int Value
        {
            get => _value;
            set
            {
                _value = value;
                Event.Invoke(_value);
            }
        }

        private Vector3Int _value;

        public override void Reset()
        {
            base.Reset();
            Event.RemoveAllListeners();
        }
    }

#if UNITY_EDITOR

    public partial class TweenUnityEventVector3Int : TweenValueVector3Int<Object>
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