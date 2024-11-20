using System;
using UnityEngine.Events;

namespace Aya.DragDrop
{
    [Serializable]
    public class Callback<T>
    {
        public Action<T> Action = delegate { };
        public UnityEvent<T> Event = new UnityEvent<T>();

        public void Add(Action<T> action)
        {
            Action += action;
        }

        public void Add(UnityAction<T> action)
        {
            Event.AddListener(action);
        }

        public void Clear()
        {
            Action = delegate { };
            Event.RemoveAllListeners();
        }

        public void Invoke(T value)
        {
            Action?.Invoke(value);
            Event?.Invoke(value);
        }
    }
}