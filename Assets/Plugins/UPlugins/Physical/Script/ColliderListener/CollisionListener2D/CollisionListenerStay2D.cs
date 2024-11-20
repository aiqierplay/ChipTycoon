using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CollisionListenerStay2D : MonoBehaviour, ICollisionListenerStay2D
    {
        [NonSerialized] public Rigidbody2D Rigidbody2D;

        public ColliderCallback<Collision2D> onCollisionStay2D = new ColliderCallback<Collision2D>();

        public Collision2DEvent CollisionStay2DEvent;

        public void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public virtual void OnCollisionStay2D(Collision2D other)
        {
            onCollisionStay2D.Trigger(other);
            CollisionStay2DEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onCollisionStay2D.Clear();
            CollisionStay2DEvent?.RemoveAllListeners();
        }
    }
}