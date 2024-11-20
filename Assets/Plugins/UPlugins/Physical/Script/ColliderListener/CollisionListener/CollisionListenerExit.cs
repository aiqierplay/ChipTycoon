using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionListenerExit : MonoBehaviour, ICollisionListenerExit
    {
        [NonSerialized] public Rigidbody Rigidbody;

        public ColliderCallback<Collision> onCollisionExit = new ColliderCallback<Collision>();

        public CollisionEvent CollisionExitEvent;

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnCollisionExit(Collision other)
        {
            onCollisionExit.Trigger(other);
            CollisionExitEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onCollisionExit.Clear();
            CollisionExitEvent?.RemoveAllListeners();
        }
    }
}