using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Collider))]
    public class ColliderListener : MonoBehaviour, IColliderListener
    {
        [NonSerialized] public Collider Collider;

        public ColliderCallback<Collider> onTriggerEnter = new ColliderCallback<Collider>();
        public ColliderCallback<Collider> onTriggerStay = new ColliderCallback<Collider>();
        public ColliderCallback<Collider> onTriggerExit = new ColliderCallback<Collider>();

        public ColliderEvent TriggerEnterEvent;
        public ColliderEvent TriggerStayEvent;
        public ColliderEvent TriggerExitEvent;

        public void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Trigger(other);
            TriggerEnterEvent?.Invoke(other);
        }

        public virtual void OnTriggerStay(Collider other)
        {
            onTriggerStay.Trigger(other);
            TriggerStayEvent?.Invoke(other);
        }

        public virtual void OnTriggerExit(Collider other)
        {
            onTriggerExit.Trigger(other);
            TriggerExitEvent?.Invoke(other);
        }
        public virtual void Clear()
        {
            onTriggerEnter.Clear();
            onTriggerStay.Clear();
            onTriggerExit.Clear();

            TriggerEnterEvent?.RemoveAllListeners();
            TriggerStayEvent?.RemoveAllListeners();
            TriggerExitEvent?.RemoveAllListeners();
        }
    }
}