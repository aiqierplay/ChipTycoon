using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Collider))]
    public class ColliderListenerStay : MonoBehaviour, IColliderListenerStay
    {
        [NonSerialized] public Collider Collider;

        public ColliderCallback<Collider> onTriggerStay = new ColliderCallback<Collider>();

        public ColliderEvent TriggerStayEvent;

        public void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnTriggerStay(Collider other)
        {
            onTriggerStay.Trigger(other);
            TriggerStayEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onTriggerStay.Clear();
            TriggerStayEvent?.RemoveAllListeners();
        }
    }
}