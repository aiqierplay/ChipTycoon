using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TweenEvent
    {
        public UnityEvent Event;
        public Action Action;

        private int _unityEventPersistentEventCount = -1;

        public void SetListener(Action action)
        {
            Action = action;
        }

        public void AddListener(Action action)
        {
            Action += action;
        }

        public void RemoveListener(Action action)
        {
            Action -= action;
        }

        public void Invoke()
        {
            if (Event != null)
            {
                if (_unityEventPersistentEventCount < 0) Refresh();
                if (_unityEventPersistentEventCount > 0) Event.Invoke();
            }

            Action?.Invoke();
        }

        public void Refresh()
        {
            _unityEventPersistentEventCount = Event.GetPersistentEventCount();
        }

        public void Reset()
        {
            if (Event == null)
            {
                Event = new UnityEvent();
            }
            else
            {
                Event.RemoveAllListeners();
            }

            Action = null;
            _unityEventPersistentEventCount = -1;
        }

        public void InitEvent()
        {
            if (Event == null)
            {
                Event = new UnityEvent();
            }
            else
            {
                Event.RemoveAllListeners();
            }

            Action = delegate { };
        }
    }

#if UNITY_EDITOR

    public partial class TweenEvent
    {
        [TweenerProperty, NonSerialized] public SerializedProperty TweenDataProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty CallbackProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty EventProperty;

        public void InitEditor(SerializedProperty dataProperty, string propertyName)
        {
            TweenDataProperty = dataProperty;
            CallbackProperty = TweenDataProperty.FindPropertyRelative(propertyName);
            EventProperty = CallbackProperty.FindPropertyRelative(nameof(Event));
        }

        public void DrawEvent(string eventName)
        {
            if (Event == null) InitEvent();
            EditorGUILayout.PropertyField(EventProperty, new GUIContent(eventName));
        }
    }

#endif

}
