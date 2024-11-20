using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderListenerEnter2D : MonoBehaviour, IColliderListenerEnter2D
    {
        [NonSerialized] public Collider2D Collider2D;

        public ColliderCallback<Collider2D> onTriggerEnter2D = new ColliderCallback<Collider2D>();

        public Collider2DEvent TriggerEnter2DEvent;

        public void Awake()
        {
            Collider2D = GetComponent<Collider2D>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            onTriggerEnter2D.Trigger(other);
            TriggerEnter2DEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onTriggerEnter2D.Clear();
            TriggerEnter2DEvent?.RemoveAllListeners();
        }
    }
}