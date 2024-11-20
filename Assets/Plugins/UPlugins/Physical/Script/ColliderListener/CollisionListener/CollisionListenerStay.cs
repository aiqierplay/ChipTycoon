using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionListenerStay : MonoBehaviour, ICollisionListenerStay
    {
        [NonSerialized] public Rigidbody Rigidbody;

        public ColliderCallback<Collision> onCollisionStay = new ColliderCallback<Collision>();

        public CollisionEvent CollisionStayEvent;

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnCollisionStay(Collision other)
        {
            onCollisionStay.Trigger(other);
            CollisionStayEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onCollisionStay.Clear();
            CollisionStayEvent?.RemoveAllListeners();
        }
    }
}