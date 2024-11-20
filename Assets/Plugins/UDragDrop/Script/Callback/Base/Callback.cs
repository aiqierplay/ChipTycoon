using System;
using UnityEngine.Events;

namespace Aya.DragDrop
{
    [Serializable]
    public class Callback
    {
        public Action Action = delegate { };
        public UnityEvent Event = new UnityEvent();

        public void Add(Action action)
        {
            Action += action;
        }

        public void Add(UnityAction action)
        {
            Event.AddListener(action);
        }

        public void Clear()
        {
            Action = delegate { };
            Event.RemoveAllListeners();
        }

        public void Invoke()
        {
            Action?.Invoke();
            Event?.Invoke();
        }
    }
}