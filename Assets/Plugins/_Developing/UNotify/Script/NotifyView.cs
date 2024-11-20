using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Aya.Notify
{
    public abstract class NotifyView : MonoBehaviour
    {
        public string Key;

        public GameObject Target;
        public Text TextCount;

        public UnityEvent OnActiveEvent;
        public UnityEvent OnDeActiveEvent;

        [NonSerialized] public NotifyNode Node;

        public Action<bool> SetNotifyAction;
        public Func<bool> GetNotifyAction;

        public int NotifyCount => Node.GetNotifyCount();

        protected virtual void Awake()
        {
            Node = UNotify.GetNode(Key, SetNotifyAction, GetNotifyAction);
        }

        protected virtual void OnEnable()
        {
            Node.OnActive += ActiveNotify;
            Node.OnDeActive += DeActiveNotify;
            Refresh();
        }

        protected virtual void OnDisable()
        {
            Node.OnActive -= ActiveNotify;
            Node.OnDeActive -= DeActiveNotify;
        }

        public virtual void Refresh()
        {
            var active = GetNotify();
            if (active) ActiveNotify();
            else DeActiveNotify();
        }

        public virtual bool GetNotify()
        {
            return Node.GetNotify();
        }

        public virtual void SetNotify(bool active)
        {
            Node.SetNotify(active);
        }

        protected virtual void ActiveNotify()
        {
            if (Target != null) Target.SetActive(true);
            if (TextCount != null)
            {
                TextCount.enabled = true;
                TextCount.text = NotifyCount.ToString();
            }

            OnActiveEvent.Invoke();
            OnActiveNotify();
        }

        public abstract void OnActiveNotify();

        protected virtual void DeActiveNotify()
        {
            if (Target != null) Target.SetActive(false);
            if (TextCount != null) TextCount.enabled = false;
            OnDeActiveEvent.Invoke();
            OnDeActiveNotify();
        }

        public abstract void OnDeActiveNotify();
    }
}