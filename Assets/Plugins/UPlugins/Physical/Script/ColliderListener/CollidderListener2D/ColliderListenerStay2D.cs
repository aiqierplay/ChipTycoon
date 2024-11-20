using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderListenerStay2D : MonoBehaviour, IColliderListenerStay2D
    {
        [NonSerialized] public Collider2D Collider2D;

        public ColliderCallback<Collider2D> onTriggerStay2D = new ColliderCallback<Collider2D>();

        public Collider2DEvent TriggerStay2DEvent;

        public void Awake()
        {
            Collider2D = GetComponent<Collider2D>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnTriggerStay2D(Collider2D other)
        {
            onTriggerStay2D.Trigger(other);
            TriggerStay2DEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onTriggerStay2D.Clear();
            TriggerStay2DEvent?.RemoveAllListeners();
        }
    }
}