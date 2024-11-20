using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionListenerEnter : MonoBehaviour, ICollisionListenerEnter
    {
        [NonSerialized] public Rigidbody Rigidbody;

        public ColliderCallback<Collision> onCollisionEnter = new ColliderCallback<Collision>();

        public CollisionEvent CollisionEnterEvent;

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnCollisionEnter(Collision other)
        {
            onCollisionEnter.Trigger(other);
            CollisionEnterEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onCollisionEnter.Clear();
            CollisionEnterEvent?.RemoveAllListeners();
        }
    }
}