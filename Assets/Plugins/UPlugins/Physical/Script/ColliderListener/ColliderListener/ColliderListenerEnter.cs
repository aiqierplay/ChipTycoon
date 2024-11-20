using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Collider))]
    public class ColliderListenerEnter : MonoBehaviour, IColliderListenerEnter
    {
        [NonSerialized] public Collider Collider;

        public ColliderCallback<Collider> onTriggerEnter = new ColliderCallback<Collider>();

        public ColliderEvent TriggerEnterEvent;

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

        public virtual void Clear()
        {
            onTriggerEnter.Clear();
            TriggerEnterEvent?.RemoveAllListeners();
        }
    }
}