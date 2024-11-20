using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Collider))]
    public class ColliderListenerExit : MonoBehaviour, IColliderListenerExit
    {
        [NonSerialized] public Collider Collider;

        public ColliderCallback<Collider> onTriggerExit = new ColliderCallback<Collider>();

        public ColliderEvent TriggerExitEvent;

        public void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnTriggerExit(Collider other)
        {
            onTriggerExit.Trigger(other);
            TriggerExitEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onTriggerExit.Clear();
            TriggerExitEvent?.RemoveAllListeners();
        }
    }
}